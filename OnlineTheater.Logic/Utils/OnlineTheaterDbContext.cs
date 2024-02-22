using Microsoft.EntityFrameworkCore;
using OnlineTheater.Logic.Entities;
using OnlineTheater.Logic.ValueObjects;

namespace OnlineTheater.Logic.Utils;

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
