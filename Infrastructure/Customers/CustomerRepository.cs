using Microsoft.EntityFrameworkCore;
using OnlineTheater.Infrastructure.Base;
using OnlineTheater.Logic.Customers;

namespace OnlineTheater.Infrastructure.Customers;

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
