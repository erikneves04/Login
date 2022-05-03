using System.Linq.Expressions;

namespace Access.Interfaces.Repository;

public interface IRepository<EntityType>
    where EntityType : class
{
    void Add(EntityType entity);
    void Update(EntityType entity);
    void Delete(EntityType entity);
    bool Exists(Guid id);
    IQueryable<EntityType> Get();
    IQueryable<EntityType> Get(Expression<Func<EntityType, bool>> expression);
    EntityType GetById(Guid id);
    IQueryable<EntityType> Queryable();
}
