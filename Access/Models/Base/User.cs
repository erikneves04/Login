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
    public string Password { get; set; }

    public static readonly int MaxAccessAttempts = 4;
    public int AccessAttemptsCount { get; set; } = 0;
    public bool AccessIsBlocked { get; private set; } = false;
    public DateTime BlockExpiresAt { get; private set; } = DateTime.MinValue;

    public void SetPassword(string password) => Password = password;
    public bool IsCorrectPassword(string password) 
    {
        if (AccessIsBlocked)
            return false;

        var isCorrect = (Password == password);
        if (!isCorrect)
            AccessAttemptsCount++;
        
        if (AccessAttemptsCount >= MaxAccessAttempts)
        {
            AccessIsBlocked = true;
            BlockExpiresAt = DateTime.Now.AddMinutes(15);
        }

        return isCorrect;
    }
    public bool AccessBlockUpdate()
    {
        if (BlockExpiresAt == DateTime.MinValue)
            return false;

        if (BlockExpiresAt >= DateTime.Now)
            return false;

        BlockExpiresAt = DateTime.MinValue;
        AccessAttemptsCount = 0;
        AccessIsBlocked = false;

        return true;
    }
}