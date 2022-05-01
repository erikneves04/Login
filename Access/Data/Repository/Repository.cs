using Access.Interfaces.Models;
using Access.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Access.Data.Repository;

public class Repository<EntityType> : IRepository<EntityType>
    where EntityType : class, IBaseModel
{
    private readonly Context _context;
    private readonly DbSet<EntityType> _table;

    public Repository(Context context)
    {
        _context = context;
        _table = _context.Set<EntityType>();
    }

    public void Add(EntityType entity)
    {
        _table.Add(entity);
    }

    public void Update(EntityType entity)
    {
        entity.LastUpdate = DateTime.Now;
        _context.Entry(entity).State = EntityState.Modified;
        _table.Update(entity);
    }

    public void Delete(EntityType entity)
    {
        _table.Remove(entity);
    }

    public IQueryable<EntityType> Get()
    {
        return _table.AsNoTracking();
    }

    public IQueryable<EntityType> Get(Expression<Func<EntityType, bool>> expression)
    {
        return _table.Where(expression);
    }

    public IQueryable<EntityType> Queryable()
    {
        return _table.AsNoTracking().AsQueryable();
    }

    public EntityType GetById(Guid id)
    {
        return _table.Find(id);
    }

    public bool Exists(Guid id)
    {
        return (GetById(id) != null);
    }
}