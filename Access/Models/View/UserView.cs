using Access.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Access.Models.View;

public class UserView
{
    public UserView() { }
    public UserView(Guid id, string name, string email, bool isActive)
    {
        Id = id;
        Name = name;
        Email = email;
        IsActive = isActive;
    }
    public UserView(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        IsActive = user.IsActive;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
}

public class UserInsertView
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Password { get; set; }
}
