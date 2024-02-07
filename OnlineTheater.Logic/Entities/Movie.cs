using OnlineTheater.Logic.ValueObjects;

namespace OnlineTheater.Logic.Entities;

public abstract class Movie : Entity
{
	public string Name { get; private set; } = default!;

	public abstract ExpirationDate ExpirationDate { get; }

	public Dollars CalculatePrice(CustomerStatus status)
	{
		return BasePrice * (1 - status.Discount);
	}

	protected abstract Dollars BasePrice { get; }
}

public class TwoDaysMovie : Movie
{
	public override ExpirationDate ExpirationDate => (ExpirationDate)DateTime.UtcNow.AddDays(2);

	protected override Dollars BasePrice => Dollars.Of(4);
}

public class LifeLongMovie : Movie
{
	public override ExpirationDate ExpirationDate => ExpirationDate.Infinite;

	protected override Dollars BasePrice => Dollars.Of(8);
}
