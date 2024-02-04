using Microsoft.AspNetCore.Mvc;
using OnlineTheater.Api.Dtos;
using OnlineTheater.Logic.Entities;
using OnlineTheater.Logic.Services;
using OnlineTheater.Logic.Utils;

namespace OnlineTheater.Api.Controllers;

[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
	private readonly UnitOfWork _unitOfWork;
	private readonly CustomerService _customerService;

	public CustomersController(
		UnitOfWork unitOfWork,
		CustomerService customerService)
	{
		_unitOfWork = unitOfWork;
		_customerService = customerService;
	}

	[HttpGet]
	[Route("{id:long}")]
	public IActionResult GetById([FromRoute] long id)
	{
		Customer? customer = _unitOfWork.Customers.GetById(id);
		if (customer is null)
			return NotFound();

		var customerDto = new CustomerDto
		{
			Id = customer.Id,
			Name = customer.Name,
			Email = customer.Email,
			Status = customer.Status.ToString(),
			StatusExpirationDate = customer.StatusExpirationDate,
			MoneySpent = customer.MoneySpent,
			PurchasedMovies = customer.PurchasedMovies.Select(pm => new PurchasedMovieDto
			{
				Price = pm.Price,
				PurchaseDate = pm.PurchaseDate,
				ExpirationDate = pm.ExpirationDate,
				Movie = new MovieDto
				{
					Id = pm.MovieId,
					Name = pm.Movie.Name
				}
			}).ToList(),
		};

		return Ok(customerDto);
	}

	[HttpGet]
	public IActionResult GetAll()
	{
		IReadOnlyList<Customer> customers = _unitOfWork.Customers.GetAll();

		var customerDtos = customers
			.Select(c => new CustomerInListDto
			{
				Id = c.Id,
				Name = c.Name,
				Email = c.Email,
				Status = c.Status.ToString(),
				StatusExpirationDate = c.StatusExpirationDate,
				MoneySpent = c.MoneySpent,
			})
			.ToList();
		return Ok(customerDtos);
	}

	[HttpPost]
	public IActionResult Create([FromBody] CreateCustomerDto dto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (_unitOfWork.Customers.GetByEmail(dto.Email) is not null)
			{
				return BadRequest("Email is already in use: " + dto.Email);
			}

			var newCustomer = new Customer
			{
				Name = dto.Name,
				Email = dto.Email,
				Status = CustomerStatus.Regular,
				MoneySpent = 0,
				StatusExpirationDate = null,
			};

			_unitOfWork.Customers.Add(newCustomer);
			_unitOfWork.SaveChanges();

			return Ok();
		}
		catch (Exception e)
		{
			return StatusCode(500, new { error = e.Message });
		}
	}

	[HttpPut]
	[Route("{id:long}")]
	public IActionResult Update([FromRoute] long id, [FromBody] UpdateCustomerDto dto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			Customer? customer = _unitOfWork.Customers.GetById(id);
			if (customer is null)
			{
				return BadRequest("Invalid customer id: " + id);
			}

			customer.Name = dto.Name;
			_unitOfWork.SaveChanges();

			return Ok();
		}
		catch (Exception e)
		{
			return StatusCode(500, new { error = e.Message });
		}
	}

	[HttpPost]
	[Route("{id:long}/movies")]
	public IActionResult PurchaseMovie([FromRoute] long id, [FromBody] long movieId)
	{
		try
		{
			Movie? movie = _unitOfWork.Movies.GetById(movieId);
			if (movie is null)
			{
				return BadRequest("Invalid movie id: " + movieId);
			}

			Customer? customer = _unitOfWork.Customers.GetById(id);
			if (customer is null)
			{
				return BadRequest("Invalid customer id: " + id);
			}

			if (customer.PurchasedMovies.Any(x => x.MovieId == movie.Id && (x.ExpirationDate == null || x.ExpirationDate.Value >= DateTime.UtcNow)))
			{
				return BadRequest("The movie is already purchased: " + movie.Name);
			}

			_customerService.PurchaseMovie(customer, movie);

			_unitOfWork.SaveChanges();

			return Ok();
		}
		catch (Exception e)
		{
			return StatusCode(500, new { error = e.Message });
		}
	}

	[HttpPost]
	[Route("{id:long}/promotion")]
	public IActionResult PromoteCustomer([FromRoute] long id)
	{
		try
		{
			Customer? customer = _unitOfWork.Customers.GetById(id);
			if (customer is null)
			{
				return BadRequest("Invalid customer id: " + id);
			}

			if (customer.Status == CustomerStatus.Advanced && (customer.StatusExpirationDate == null || customer.StatusExpirationDate.Value < DateTime.UtcNow))
			{
				return BadRequest("The customer already has the Advanced status");
			}

			bool success = _customerService.PromoteCustomer(customer);
			if (!success)
			{
				return BadRequest("Cannot promote the customer");
			}

			_unitOfWork.SaveChanges();

			return Ok();
		}
		catch (Exception e)
		{
			return StatusCode(500, new { error = e.Message });
		}
	}
}
