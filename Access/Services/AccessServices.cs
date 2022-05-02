using Access.Interfaces.Services;
using Access.Models.View;
using System.Security.Cryptography;
using System.Text;
using System;

namespace Access.Services;

public class AccessServices : IAccessServices
{
    private readonly IAccessLoggerServices _loggerServices;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUserServices _userServices;

    private readonly string PrivateEncryptKey;
    private readonly string PublicEncryptKey;

    public AccessServices(IAccessLoggerServices loggerServices, IHttpContextAccessor contextAccessor, IUserServices userServices, IConfiguration configuration)
    {
        _loggerServices = loggerServices;
        _contextAccessor = contextAccessor;
        _userServices = userServices;

        PrivateEncryptKey = configuration["Keys:PrivateTokenEncrypt"];
        PublicEncryptKey = configuration["Keys:PublicTokenEncrypt"];
    }

    public TokenView Login(LoginView login)
    {
        if (string.IsNullOrEmpty(login.Email))
            throw new InvalidDataException("Email is required");

        if (string.IsNullOrEmpty(login.Password))
            throw new InvalidDataException("Password is required");
    
        var user = _userServices.Access(login.Email, login.Password);


        var expiresAt = DateTime.UtcNow.AddMinutes(60);
        var ipAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        var token = GetToken(PrivateEncryptKey, PublicEncryptKey, user.Id, user.Name, expiresAt);

        _loggerServices.Insert(new AccessLoggerInsertView(user.Id, token, ipAddress, expiresAt));
        return new TokenView("authorized access", token, expiresAt);
    }

    public void Logout()
    {
        _loggerServices.SwitchValidStateByToken(GetRequestToken());
    }

    private string GetRequestToken()
    {
        string token = _contextAccessor.HttpContext.Request.Headers["Authorization"];
        return token?.Substring(7);
    }

    public static string GetToken(string privateKey, string publicKey, Guid  userId,string userName, DateTime expiresAt)
    {
        string text = Guid.NewGuid().ToString() + userId + expiresAt.ToString() + userName;

        try
        {
            string textToEncrypt = text;
            string ToReturn = "";
            string publickey = publicKey;
            string secretkey = privateKey;
            byte[] secretkeyByte = { };
            secretkeyByte = Encoding.UTF8.GetBytes(secretkey);
            byte[] publickeybyte = { };
            publickeybyte = Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms = null;
            CryptoStream cs = null;

            byte[] inputbyteArray = Encoding.UTF8.GetBytes(textToEncrypt);
            using (var des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                ToReturn = Convert.ToBase64String(ms.ToArray());
            }
            return ToReturn;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex.InnerException);
        }
    }
    public static TokenDecryptedData GetTokenData(string privateKey, string publicKey, string token)
    {
        string decrypted;
        try
        {
            string textToDecrypt = token;
            string publickey = publicKey;
            string secretkey = privateKey;
            byte[] privatekeyByte = { };
            privatekeyByte = Encoding.UTF8.GetBytes(secretkey);
            byte[] publickeybyte = { };
            publickeybyte = Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
            inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
            using (var des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                decrypted = encoding.GetString(ms.ToArray());
            }
        }
        catch (Exception ae)
        {
            throw new Exception(ae.Message, ae.InnerException);
        }

        var guidLenght = Guid.NewGuid().ToString().Length;
        var dateTimeLenght = DateTime.Now.ToString().Length;
        var userId = Guid.Parse(new string(decrypted.Skip(guidLenght).Take(guidLenght).ToArray()));
        var expiresAt = DateTime.Parse(decrypted.Skip(guidLenght * 2).Take(dateTimeLenght).ToArray());
        var userName = new string(decrypted.Skip((guidLenght * 2) + dateTimeLenght).ToArray());

        return new TokenDecryptedData(userId, userName, expiresAt);
    }
}