using Microsoft.EntityFrameworkCore;
using OnlineTheater.Logic.Entities;

namespace OnlineTheater.Logic.Utils;

public class OnlineTheaterDbContext : DbContext
{

    public OnlineTheaterDbContext(DbContextOptions<OnlineTheaterDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Movie> Movies { get; set; }
}
