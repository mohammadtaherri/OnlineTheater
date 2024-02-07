using OnlineTheater.Logic.ValueObjects;

namespace OnlineTheater.Logic.Entities;

public class Customer : Entity
{
	public CustomerName Name { get; set; } = default!;
	public Email Email { get; set; } = default!;
	public CustomerStatus Status { get; set; }
	public ExpirationDate StatusExpirationDate { get; set; } = default!;
	public Dollars MoneySpent { get; set; }
	public IList<PurchasedMovie> PurchasedMovies { get; set; } = 
		new List<PurchasedMovie>();
}
