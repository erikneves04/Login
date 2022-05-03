using Access.Interfaces.Repository;
using Access.Interfaces.Services;
using Access.Models.Base;
using Access.Models.View;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Access.Services;

public class UserServices : IUserServices
{
    private readonly IUserRepository _reporitory;

    public UserServices(IUserRepository reporitory)
    {
        _reporitory = reporitory;
    }

    public IEnumerable<UserView> ViewAll()
    {
        var users = GetAll().ToList();
        if (!users.Any())
            throw new Exception("Users not found");

        return users.Select(e => new UserView(e));
    }

    public UserView View(Guid id)
    {
        var user = Get(id);
        if (user == null)
            throw new Exception("User not found");

        return new UserView(user);
    }

    public UserView Insert(UserInsertView data)
    {
        ValidateUser(data);

        var user = new User(data);
        _reporitory.Add(user);

        return new UserView(user);
    }

    public UserView Update(UserInsertView data, Guid id)
    {
        var user = Get(id);
        if (user == null)
            throw new Exception("User not found");

        ValidateUser(data, user.Email);

        user.Name = data.Name;
        user.Email = data.Email;
        user.SetPassword(data.Password);

        _reporitory.Update(user);

        return new UserView(user);
    }

    public void Delete(Guid id)
    {
        var user = Get(id);
        if (user == null)
            throw new Exception("User not found");

        user.IsActive = false;
        _reporitory.Update(user);
    }

    public UserView Access(string email, string password)
    {
        var user = GetByEmail(email);
        if (user == null)
            throw new InvalidDataException("Email or password is incorrect!");

        if (user.AccessBlockUpdate())
            _reporitory.Update(user);
        
        if (user.AccessIsBlocked)
            throw new InvalidDataException($"Too many attempts, wait until {user.BlockExpiresAt} to try again!");

        if (!user.IsCorrectPassword(password))
        {
            _reporitory.Update(user);
            throw new InvalidDataException("Email or password is incorrect!");
        }
            

        return new UserView(user);
    }

    private User Get(Guid id)
    {
        return GetAll()
                .FirstOrDefault(e => e.Id == id);
    }

    private User GetByEmail(string email)
    {
        return GetAll()
                .FirstOrDefault(e => e.Email == email);
    }

    private IQueryable<User> GetAll()
    {
        return _reporitory.Queryable();
    }

    private void ValidateUser(UserInsertView user, string oldEmail = null)
    {
        if (!IsValidEmail(user.Email))
            throw new InvalidDataException("Invalid email.");

        if (GetByEmail(user.Email) != null && !user.Email.Equals(oldEmail))
            throw new InvalidDataException("This email is already linked to a user.");

        ValidatePassword(user.Password);
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        return new EmailAddressAttribute().IsValid(email);
    }

    public static void ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new Exception("Password should not be empty");

        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasMiniMaxChars = new Regex(@".{8,15}");
        var hasLowerChar = new Regex(@"[a-z]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");


        if (!hasLowerChar.IsMatch(password))
            throw new InvalidDataException("Password should contain at least one lower case letter.");

        if (!hasUpperChar.IsMatch(password))
            throw new InvalidDataException("Password should contain at least one upper case letter.");

        if (!hasMiniMaxChars.IsMatch(password))
            throw new InvalidDataException("Password should not be lesser than 8 or greater than 15 characters.");

        if (!hasNumber.IsMatch(password))
            throw new InvalidDataException("Password should contain at least one numeric value.");

        if (!hasSymbols.IsMatch(password))
            throw new InvalidDataException("Password should contain at least one special case character.");
    }
}