﻿using OnlineTheater.Logic.ValueObjects;

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

	public void PurchaseMovie(Movie movie)
	{
		ExpirationDate expirationDate = movie.ExpirationDate;
		Dollars price = movie.CalculatePrice(Status);

		_purchasedMovies.Add(new PurchasedMovie(movie, this, price, expirationDate));
		MoneySpent += price;
	}

	public bool Promote()
	{
		// at least 2 active movies during the last 30 days
		if (PurchasedMovies.Count(x => x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Date >= DateTime.UtcNow.AddDays(-30)) < 2)
			return false;

		// at least 100 dollars spent during the last year
		if (PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 100m)
			return false;

		Status = Status.Promote();

		return true;
	}
}
