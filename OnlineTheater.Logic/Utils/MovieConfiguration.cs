using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTheater.Logic.Entities;

namespace OnlineTheater.Logic.Utils;

internal class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
	public void Configure(EntityTypeBuilder<Movie> builder)
	{
		builder.ToTable("Movie");

		builder.HasDiscriminator<int>("LicensingModel")
			.HasValue<TwoDaysMovie>(1)
			.HasValue<LifeLongMovie>(2);
	}
}
