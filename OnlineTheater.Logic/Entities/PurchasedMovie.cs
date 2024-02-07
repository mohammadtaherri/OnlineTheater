using OnlineTheater.Logic.ValueObjects;

namespace OnlineTheater.Logic.Entities;

public class PurchasedMovie : Entity
{
	public long MovieId { get; set; }
	public Movie Movie { get; set; } = default!;
	public long CustomerId { get; set; }
	public Dollars Price { get; set; }
	public DateTime PurchaseDate { get; set; }
	public ExpirationDate ExpirationDate { get; set; } = default!;
}
