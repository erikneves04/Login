using Access.Interfaces.Repository;
using Access.Interfaces.Services;
using Access.Models.Base;
using Access.Models.View;
using Microsoft.EntityFrameworkCore;

namespace Access.Services;

public class AccessLoggerServices : IAccessLoggerServices
{
    public IAccessLoggerRepository _repository;

    public AccessLoggerServices(IAccessLoggerRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<AccessLoggerView> ViewAll() 
    {
        var loggers = GetAll();
        if (!loggers.Any())
            throw new Exception("Acess logger not found.");

        return loggers.Select(e => new AccessLoggerView(e));
    }

    public IEnumerable<AccessLoggerView> ViewByUserId(Guid userId)
    {
        var loggers = GetByUserId(userId);
        if (!loggers.Any())
            throw new Exception("Acess logger not found.");

        return loggers.Select(e => new AccessLoggerView(e));
    }

    public AccessLoggerView View(Guid id)
    {
        var logger = Get(id);
        if (logger == null)
            throw new Exception("Acess logger not found.");

        return new AccessLoggerView(logger);
    }

    public AccessLoggerView Insert(AccessLoggerInsertView data)
    {
        var logger = new AccessLogger(data);
        _repository.Add(logger);

        return new AccessLoggerView(Get(logger.Id));
    }

    public AccessLoggerView Update(AccessLoggerInsertView data, Guid id)
    {
        var logger = Get(id);
        if (logger == null)
            throw new Exception("Acess logger not found.");

        logger.IpAddress = data.IpAddress;
        logger.ExpiresAt = data.ExpiresAt;
        logger.UserId = data.UserId;
        logger.Token = data.Token;

        _repository.Update(logger);

        return new AccessLoggerView(Get(logger.Id));
    }

    public void Delete(Guid id)
    {
        var logger = Get(id);
        if (logger == null)
            throw new Exception("Acess logger not found.");

        _repository.Delete(logger);
    }

    public bool IsExpired(string token)
    {
        var logger = GetByToken(token);
        if(logger == null)
            throw new Exception("Acess logger not found.");

        logger.UpdateExpireState();
        return logger.IsExpired;
    }

    public void SwitchValidStateByToken(string token)
    {
        var logger = GetAll()
            .FirstOrDefault(e => e.Token == token && !e.IsExpired);
        if (logger == null)
            throw new Exception("Acess logger not found.");

        SwitchValidState(logger);
    }

    public void SwitchValidStateByUserId(Guid userId)
    {
        var logger = GetAll()
            .FirstOrDefault(e => e.UserId == userId && !e.IsExpired);
        if (logger == null)
            throw new Exception("Acess logger not found.");

        SwitchValidState(logger);
    }

    private void SwitchValidState(AccessLogger entity)
    {
        if (entity == null)
            throw new Exception("Acess logger not found.");

        entity.ExpiresAt = DateTime.Now;
        entity.IsExpired = true;

        _repository.Update(entity);
    }

    private AccessLogger GetByToken(string token)
    {
        return GetAll()
                .Where(e => e.Token == token)
                .FirstOrDefault();
    }

    private IEnumerable<AccessLogger> GetByUserId(Guid id)
    {
        return GetAll()
                .Where(e => e.UserId == id);
    }
    private AccessLogger Get(Guid id)
    {
        var logger = GetAll()
                        .FirstOrDefault(e => e.Id == id);

        if(logger != null)
            logger.UpdateExpireState();

        return logger;
    }

    private IQueryable<AccessLogger> GetAll()
    {
        return _repository
                .Queryable()
                .Include(e => e.User);
    }
}