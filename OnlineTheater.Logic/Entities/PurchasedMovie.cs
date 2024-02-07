using OnlineTheater.Logic.ValueObjects;

namespace OnlineTheater.Logic.Entities;

public class PurchasedMovie : Entity
{
	public Movie Movie { get; private set; }
	public Customer Customer { get; private set; }
	public Dollars Price { get; private set; }
	public DateTime PurchaseDate { get; private set; }
	public ExpirationDate ExpirationDate { get; private set; }

	internal PurchasedMovie(Movie movie, Customer customer, Dollars price, ExpirationDate expirationDate)
	{
		if (price is null || price.IsZero)
			throw new ArgumentException(nameof(price));

		if(expirationDate is null || expirationDate.IsExpired)
			throw new ArgumentException(nameof(expirationDate));

		Movie = movie ?? throw new ArgumentNullException(nameof(movie));
		Customer = customer ?? throw new ArgumentNullException(nameof(customer));
		Price = price;
		ExpirationDate = expirationDate;
		PurchaseDate = DateTime.Now;
	}
}
