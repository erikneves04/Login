using Access.Interfaces.Services;
using Access.Models.View;
using Access.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests;

public class AccessServicesTests
{
    [Fact]
    public void EncryptTest()
    {
        var publicKey = "12345678";
        var privateKey = "87654321";

        Guid  userId = Guid.NewGuid();
        string userName = "Erik Neves";
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(10);
        var baseInfo = new TokenDecryptedData(userId, userName, expiresAt);


        var token = AccessServices.GetToken(privateKey, publicKey, userId, userName, expiresAt);
        var decrypted = AccessServices.GetTokenData(privateKey, publicKey, token);

        Assert.Equal(baseInfo.UserName, decrypted.UserName);
        Assert.Equal(baseInfo.ExpiresAt.ToShortDateString(), decrypted.ExpiresAt.ToShortDateString());
        Assert.Equal(baseInfo.UserId, decrypted.UserId);
    }
}