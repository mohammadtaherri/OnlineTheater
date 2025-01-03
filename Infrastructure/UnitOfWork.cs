﻿

using OnlineTheater.Infrastructure.Customers;
using OnlineTheater.Infrastructure.Movies;

namespace OnlineTheater.Infrastructure;

public class UnitOfWork : IDisposable
{
    private readonly OnlineTheaterDbContext _dbContext;
    private CustomerRepository? _customerRepository;
    private MovieRepository? _movieRepository;

    public UnitOfWork(OnlineTheaterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public CustomerRepository Customers => 
        _customerRepository ??= new CustomerRepository(_dbContext);  

    public MovieRepository Movies =>
        _movieRepository ??= new MovieRepository(_dbContext);   

    public void Complete()
    {
        _dbContext.SaveChanges();
    }

    public void Dispose()
	{
		_dbContext.Dispose();
	}
}
