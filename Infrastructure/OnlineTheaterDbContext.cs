using Microsoft.EntityFrameworkCore;
using OnlineTheater.Infrastructure.Customers;
using OnlineTheater.Infrastructure.Movies;
using OnlineTheater.Logic.Customers;
using OnlineTheater.Logic.Movies;


namespace OnlineTheater.Infrastructure;

public class OnlineTheaterDbContext : DbContext
{

    public OnlineTheaterDbContext(DbContextOptions<OnlineTheaterDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Movie> Movies { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new CustomerConfiguration());
		modelBuilder.ApplyConfiguration(new MovieConfiguration());
		modelBuilder.ApplyConfiguration(new PurchasedMovieConfiguration());
	}
}
