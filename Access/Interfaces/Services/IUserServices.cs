using Access.Models.Base;
using Access.Models.View;

namespace Access.Interfaces.Services;

public interface IUserServices
{
    User Access(string email, string password);
    void Delete(Guid id);
    UserView Insert(UserInsertView data);
    UserView Update(UserInsertView data, Guid id);
    UserView View(Guid id);
    IEnumerable<UserView> ViewAll();
}
