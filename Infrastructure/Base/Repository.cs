using OnlineTheater.Logic.Base;

namespace OnlineTheater.Infrastructure.Base;

public abstract class Repository<TEntity>
        where TEntity : Entity
{
    protected readonly OnlineTheaterDbContext _dbContext;

    protected Repository(OnlineTheaterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual TEntity? GetById(long id)
    {
        return _dbContext.Set<TEntity>().Find(id);
    }

    public void Add(TEntity entity)
    {
        _dbContext.Set<TEntity>().Add(entity);
    }
}