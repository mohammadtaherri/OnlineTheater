using OnlineTheater.Logic.Entities;
using OnlineTheater.Logic.Utils;


namespace OnlineTheater.Logic.Repositories;

public class MovieRepository : Repository<Movie>
{
	public MovieRepository(OnlineTheaterDbContext dbContext)
		: base(dbContext)
	{
	}

	public IReadOnlyList<Movie> GetAll()
	{
		return _dbContext.Movies
			.ToList();
	}
}
