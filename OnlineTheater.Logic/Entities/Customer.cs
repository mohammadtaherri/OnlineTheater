using OnlineTheater.FunctionalExtensions;
using OnlineTheater.Logic.ValueObjects;

namespace OnlineTheater.Logic.Entities;

public class Customer : Entity
{
	public CustomerName Name { get; set; }
	public Email Email { get; }
	public CustomerStatus Status { get; private set; }
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

	public virtual bool CanPurchasedMovie(Movie movie)
	{
		return PurchasedMovies.Any(x => x.Movie == movie && !x.ExpirationDate.IsExpired);
	}

	public virtual void PurchaseMovie(Movie movie)
	{
		if (CanPurchasedMovie(movie))
			throw new Exception();

		ExpirationDate expirationDate = movie.ExpirationDate;
		Dollars price = movie.CalculatePrice(Status);

		var purchasedMovie = new PurchasedMovie(movie, this, price, expirationDate);
		_purchasedMovies.Add(purchasedMovie);

		MoneySpent += price;
	}

	public virtual Result CanPromote()
	{
		if (Status.IsAdvanced)
			return Result.Fail("The customer already has the Advanced status");

		if (PurchasedMovies.Count(x =>
			x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Date >= DateTime.UtcNow.AddDays(-30)) < 2)
			return Result.Fail("The customer has to have at least 2 active movies during the last 30 days");

		if (PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 100m)
			return Result.Fail("The customer has to have at least 100 dollars spent during the last year");

		return Result.Ok();
	}

	public virtual void Promote()
	{
		if (CanPromote().IsFailure)
			throw new Exception();

		Status = Status.Promote();
	}
}
