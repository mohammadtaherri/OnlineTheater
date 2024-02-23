namespace OnlineTheater.Api.Customers;

public class CustomerDto
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateTime? StatusExpirationDate { get; set; }
    public decimal MoneySpent { get; set; }
    public List<PurchasedMovieDto> PurchasedMovies { get; set; } = default!;
}
