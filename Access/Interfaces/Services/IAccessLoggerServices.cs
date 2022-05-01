using Access.Models.View;

namespace Access.Interfaces.Services;

public interface IAccessLoggerServices
{
    void Delete(Guid id);
    AccessLoggerView Insert(AccessLoggerInsertView data);
    AccessLoggerView Update(AccessLoggerInsertView data, Guid id);
    AccessLoggerView View(Guid id);
    IEnumerable<AccessLoggerView> ViewAll();
    IEnumerable<AccessLoggerView> ViewByUserId(Guid userId);
}
