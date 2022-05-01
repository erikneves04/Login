using Access.Interfaces.Repository;
using Access.Interfaces.Services;
using Access.Models.Base;
using Access.Models.View;

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
            throw new Exception("Users not found.");

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
            throw new Exception("Invalid email.");

        if (!IsValidPassword(user.Password))
            throw new Exception("Invalid password.");

        if (GetByEmail(user.Email) != null && !user.Email.Equals(oldEmail))
            throw new Exception("This email is already linked to a user.");
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        return true;
    }

    public static bool IsValidPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return false;

        return true;
    }
}