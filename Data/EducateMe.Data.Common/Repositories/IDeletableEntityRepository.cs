using System.Linq;

using EducateMe.Data.Common.Models;

namespace EducateMe.Data.Common.Repositories;

public interface IDeletableEntityRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IDeletableEntity
{
    IQueryable<TEntity> AllWithDeleted();

    IQueryable<TEntity> AllAsNoTrackingWithDeleted();

    void HardDelete(TEntity entity);

    void Undelete(TEntity entity);
}
