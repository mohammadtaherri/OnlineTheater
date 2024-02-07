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

        modelBuilder.Entity<Customer>(x =>
        {
            x.Property(c => c.Name)
                .HasConversion(
                    customerName => customerName.Value, 
                    cusomerNameString => CustomerName.Create(cusomerNameString).Value)
                .HasMaxLength(50);

			x.Property(c => c.Email)
				.HasConversion(
                    email => email.Value, 
                    emailString => Email.Create(emailString).Value)
                .HasMaxLength(150);

            x.Property(c => c.MoneySpent)
                .HasConversion(
                    dollars => dollars.Value,
					dollars => Dollars.Create(dollars).Value);

			x.Property(c => c.StatusExpirationDate)
				.HasConversion(
					expirationDate => expirationDate.Date,
					expirationDate => (ExpirationDate)expirationDate); 
		});

		modelBuilder.Entity<PurchasedMovie>(x =>
		{
			x.Property(c => c.Price)
				.HasConversion(
					dollars => dollars.Value,
					dollars => Dollars.Create(dollars).Value);

			x.Property(c => c.ExpirationDate)
				.HasConversion(
					expirationDate => expirationDate.Date,
					expirationDate => (ExpirationDate)expirationDate); 
		});
	}
}
