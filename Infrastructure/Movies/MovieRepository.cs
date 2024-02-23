
using OnlineTheater.Infrastructure.Base;
using OnlineTheater.Logic.Movies;



namespace OnlineTheater.Infrastructure.Movies;

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
