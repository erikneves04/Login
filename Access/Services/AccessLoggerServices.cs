﻿using Access.Interfaces;
using Access.Models.Base;
using Access.Models.View;
using Microsoft.EntityFrameworkCore;

namespace Access.Services;

public class AccessLoggerServices
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

        return new AccessLoggerView(logger);
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

        return new AccessLoggerView(logger);
    }

    public void Delete(Guid id)
    {
        var logger = Get(id);
        if (logger == null)
            throw new Exception("Acess logger not found.");

        _repository.Delete(logger);
    }

    private IEnumerable<AccessLogger> GetByUserId(Guid id)
    {
        return GetAll()
                .Where(e => e.UserId == id);
    }
    private AccessLogger Get(Guid id)
    {
        return GetAll()
                .FirstOrDefault(e => e.Id == id);
    }

    private IQueryable<AccessLogger> GetAll()
    {
        return _repository
                .Queryable()
                .Include(e => e.User);
    }
}