using Access.Interfaces.Repository;
using Access.Models.Base;

namespace Access.Data.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(Context context) : base(context)
    {
    }
}
