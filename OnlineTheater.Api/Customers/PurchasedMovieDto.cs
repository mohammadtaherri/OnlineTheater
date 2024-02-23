namespace OnlineTheater.Api.Customers;

public class PurchasedMovieDto
{
    public MovieDto Movie { get; set; } = default!;
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
}
