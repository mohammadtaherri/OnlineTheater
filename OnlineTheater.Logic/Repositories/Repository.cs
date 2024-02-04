using Microsoft.EntityFrameworkCore;
using OnlineTheater.Logic.Entities;
using OnlineTheater.Logic.Utils;

namespace OnlineTheater.Logic.Repositories;

public abstract class Repository<TEntity>
		where TEntity : Entity
{
	protected readonly DbSet<TEntity> _dbSet;

	protected Repository(OnlineTheaterDbContext dbContext)
	{
		_dbSet = dbContext.Set<TEntity>();
	}

	public TEntity? GetById(long id)
	{
		return _dbSet.Find(id);
	}

	public void Add(TEntity entity)
	{
		_dbSet.Add(entity);
	}
}