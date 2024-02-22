using Microsoft.EntityFrameworkCore;
using OnlineTheater.Logic.Entities;
using OnlineTheater.Logic.Utils;

namespace OnlineTheater.Logic.Repositories;

public abstract class Repository<TEntity>
		where TEntity : Entity
{
	protected readonly OnlineTheaterDbContext _dbContext;

	protected Repository(OnlineTheaterDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public TEntity? GetById(long id)
	{
		return _dbContext.Set<TEntity>().Find(id);
	}

	public void Add(TEntity entity)
	{
		__dbContext.Set<TEntity>().Add(entity);
	}
}