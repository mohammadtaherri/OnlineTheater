
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTheater.Logic.Customers;

namespace OnlineTheater.Infrastructure.Customers;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");

        builder.Property(c => c.Name)
            .HasConversion(
                customerName => customerName.Value,
                cusomerNameString => CustomerName.Create(cusomerNameString).Value)
            .HasMaxLength(50);

        builder.Property(c => c.Email)
            .HasConversion(
                email => email.Value,
                emailString => Email.Create(emailString).Value)
            .HasMaxLength(150);

        builder.Property(c => c.MoneySpent)
            .HasConversion(
                dollars => dollars.Value,
                dollars => Dollars.Create(dollars).Value);

        builder.OwnsOne(c => c.Status, statusBuilder =>
        {
            statusBuilder.Property(s => s.Type).HasColumnName("Status");
            statusBuilder.Property(s => s.ExpirationDate)
                .HasConversion(
                    expirationDate => expirationDate.Date,
                    expirationDate => (ExpirationDate)expirationDate)
                .HasColumnName("StatusExpirationDate")
                .IsRequired(false);
        });
    }
}
