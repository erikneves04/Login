using Access.Interfaces;
using Access.Models.Base;

namespace Access.Data.Repository;

public class AccessLoggerRepository : Repository<AccessLogger>, IAccessLoggerRepository
{
    public AccessLoggerRepository(Context context) : base(context)
    {
    }
}
