using OnlineTheater.Logic.ValueObjects;

namespace OnlineTheater.Logic.Entities;

public class Customer : Entity
{
	public CustomerName Name { get; set; } = default!;
	public Email Email { get; private set; } = default!;
	public CustomerStatus Status { get; set; }
	public Dollars MoneySpent { get; private set; }

	private readonly IList<PurchasedMovie> _purchasedMovies;
	public IReadOnlyList<PurchasedMovie> PurchasedMovies => _purchasedMovies.ToList();

	public Customer(CustomerName name, Email email)
	{
		Name = name ?? throw new ArgumentNullException(nameof(name));
		Email = email ?? throw new ArgumentNullException(nameof(email));
		Status = CustomerStatus.Regular;
		MoneySpent = Dollars.Of(0);

		_purchasedMovies = new List<PurchasedMovie>();
	}

	public void AddPurchasedMovie(Movie movie, ExpirationDate expirationDate, Dollars price)
	{
		var purchasedMovie = new PurchasedMovie(movie, this, price, expirationDate);
		_purchasedMovies.Add(purchasedMovie);
		MoneySpent += price;
	}
}
