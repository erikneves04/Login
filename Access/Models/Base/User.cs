using Access.Models.View;

namespace Access.Models.Base;

public class User : Base
{
    public User() { }
    public User(string name, string email,string password)
    { 
        Name = name;
        Email = email;
        Password = password;
        IsActive = true;
    }
    public User(UserInsertView user)
    {
        Name = user.Name;
        Email = user.Email;
        Password = user.Password;
        IsActive = true;
    }

    public bool IsActive { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    private string Password;
    
    public void SetPassword(string password) => Password = password;
    public bool IsCorrectPassword(string password) => Password == password;
}