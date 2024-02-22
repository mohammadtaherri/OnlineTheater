using Microsoft.EntityFrameworkCore;
using OnlineTheater.Logic.Entities;
using OnlineTheater.Logic.Utils;
using OnlineTheater.Logic.ValueObjects;

namespace OnlineTheater.Logic.Repositories;

public class CustomerRepository : Repository<Customer>
{
	public CustomerRepository(OnlineTheaterDbContext dbContext)
		: base(dbContext)
	{
	}

	public override Customer? GetById(long id)
	{
		return _dbContext.Customers
			.Include(c => c.PurchasedMovies)
			.ThenInclude(pm => pm.Movie)
			.SingleOrDefault(c => c.Id == id);
	}

	public IReadOnlyList<Customer> GetAll()
	{
		return _dbContext.Customers
			.ToList();
	}

	public Customer? GetByEmail(Email email)
	{
		return _dbContext.Customers
			.SingleOrDefault(c => c.Email == email);
	}
}
