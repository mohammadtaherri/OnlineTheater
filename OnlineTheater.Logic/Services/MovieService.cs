using OnlineTheater.Logic.Entities;

namespace OnlineTheater.Logic.Services;

public class MovieService
{
	public DateTime? GetExpirationDate(LicensingModel licensingModel)
	{
		DateTime? result;

		switch (licensingModel)
		{
			case LicensingModel.TwoDays:
				result = DateTime.UtcNow.AddDays(2);
				break;

			case LicensingModel.LifeLong:
				result = null;
				break;

			default:
				throw new ArgumentOutOfRangeException();
		}

		return result;
	}
}
