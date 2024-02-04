using Microsoft.EntityFrameworkCore;
using OnlineTheater.Logic.Entities;
using OnlineTheater.Logic.Utils;

namespace OnlineTheater.Logic.Repositories;

public class CustomerRepository : Repository<Customer>
{
	public CustomerRepository(OnlineTheaterDbContext dbContext)
		: base(dbContext)
	{
	}

	public IReadOnlyList<Customer> GetAll()
	{
		return _dbSet
			.Include(c => c.PurchasedMovies)
			.ToList();
	}

	public Customer? GetByEmail(string email)
	{
		return _dbSet.SingleOrDefault(c => c.Email == email);
	}
}
