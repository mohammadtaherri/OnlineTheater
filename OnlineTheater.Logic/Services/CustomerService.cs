using OnlineTheater.Logic.Entities;
using OnlineTheater.Logic.ValueObjects;

namespace OnlineTheater.Logic.Services;

public class CustomerService
{
	private readonly MovieService _movieService;

	public CustomerService(MovieService movieService)
	{
		_movieService = movieService;
	}

	public void PurchaseMovie(Customer customer, Movie movie)
	{
		ExpirationDate expirationDate = _movieService.GetExpirationDate(movie.LicensingModel);
		Dollars price = CalculatePrice(
			customer.Status, 
			customer.StatusExpirationDate, 
			movie.LicensingModel);

		customer.AddPurchasedMovie(movie, expirationDate, price);
	}

	private Dollars CalculatePrice(
		CustomerStatus status,
		ExpirationDate statusExpirationDate,
		LicensingModel licensingModel)
	{
		Dollars price;
		switch (licensingModel)
		{
			case LicensingModel.TwoDays:
				price = Dollars.Of(4);
				break;

			case LicensingModel.LifeLong:
				price = Dollars.Of(8);
				break;

			default:
				throw new ArgumentOutOfRangeException();
		}

		if (status == CustomerStatus.Advanced && !statusExpirationDate.IsExpired)
		{
			price *= 0.75m;
		}

		return price;
	}

	public bool PromoteCustomer(Customer customer)
	{
		// at least 2 active movies during the last 30 days
		if (customer.PurchasedMovies.Count(x => x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Date >= DateTime.UtcNow.AddDays(-30)) < 2)
			return false;

		// at least 100 dollars spent during the last year
		if (customer.PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 100m)
			return false;

		customer.Status = CustomerStatus.Advanced;
		customer.StatusExpirationDate = (ExpirationDate) DateTime.UtcNow.AddYears(1);

		return true;
	}
}
