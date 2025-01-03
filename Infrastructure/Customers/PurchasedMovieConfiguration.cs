﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTheater.Logic.Customers;
namespace OnlineTheater.Infrastructure.Customers;

internal class PurchasedMovieConfiguration : IEntityTypeConfiguration<PurchasedMovie>
{
    public void Configure(EntityTypeBuilder<PurchasedMovie> builder)
    {
        builder.Property(pm => pm.Price)
                .HasConversion(
                    dollars => dollars.Value,
                    dollars => Dollars.Create(dollars).Value);

        builder.Property(pm => pm.ExpirationDate)
            .HasConversion(
                expirationDate => expirationDate.Date,
                expirationDate => (ExpirationDate)expirationDate)
            .IsRequired(false);
    }
}
