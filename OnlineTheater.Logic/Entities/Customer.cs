using System.ComponentModel.DataAnnotations;

namespace OnlineTheater.Logic.Entities;

public class Customer : Entity
{
	public string Name { get; set; } = default!;
	public string Email { get; set; } = default!;
	public CustomerStatus Status { get; set; }
	public DateTime? StatusExpirationDate { get; set; }
	public decimal MoneySpent { get; set; }
	public IList<PurchasedMovie> PurchasedMovies { get; set; } = 
		new List<PurchasedMovie>();
}
