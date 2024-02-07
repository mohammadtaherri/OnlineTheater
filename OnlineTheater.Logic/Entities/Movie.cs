using OnlineTheater.Logic.ValueObjects;
using System.Text.Json.Serialization;

namespace OnlineTheater.Logic.Entities;

public class Movie : Entity
{
	public string Name { get; private set; }
	public LicensingModel LicensingModel { get; private set; }

	public ExpirationDate ExpirationDate
	{
		get
		{
			switch (LicensingModel)
			{
				case LicensingModel.TwoDays:
					return (ExpirationDate)DateTime.UtcNow.AddDays(2);

				case LicensingModel.LifeLong:
					return ExpirationDate.Infinite;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		
	}

	public Dollars CalculatePrice(CustomerStatus status)
	{
		decimal modifier = 1 - status.Discount;
		switch (LicensingModel)
		{
			case LicensingModel.TwoDays:
				return Dollars.Of(4) * modifier;

			case LicensingModel.LifeLong:
				return Dollars.Of(8) * modifier;

			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}
