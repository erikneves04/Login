using Access.Models.View;

namespace Access.Interfaces.Services;

public interface IAccessServices
{
    TokenView Login(LoginView login);
}