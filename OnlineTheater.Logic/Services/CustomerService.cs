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
		Dollars price = CalculatePrice(customer.Status, movie.LicensingModel);

		customer.AddPurchasedMovie(movie, expirationDate, price);
	}

	private Dollars CalculatePrice(CustomerStatus status, LicensingModel licensingModel)
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

		if (status.IsAdvanced)
		{
			price *= 0.75m;
		}

		return price;
	}

}
