using System.ComponentModel.DataAnnotations;

namespace OnlineTheater.Logic.Entities;

public class Customer : Entity
{
	[MaxLength(100, ErrorMessage = "Name is too long")]
	public string Name { get; set; } = default!;

	[EmailAddress(ErrorMessage = "Email is invalid")]
	public string Email { get; set; } = default!;

	//[JsonConverter(typeof(StringEnumConverter))]
	public CustomerStatus Status { get; set; }

	public DateTime? StatusExpirationDate { get; set; }

	public decimal MoneySpent { get; set; }

	public IList<PurchasedMovie> PurchasedMovies { get; set; } = 
		new List<PurchasedMovie>();
}
