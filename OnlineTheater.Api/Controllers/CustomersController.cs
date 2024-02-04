using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTheater.Logic.Entities;
using OnlineTheater.Logic.Repositories;
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
	[Route("{id}")]
	public IActionResult GetById([FromRoute] long id)
	{
		Customer? customer = _unitOfWork.Customers.GetById(id);
		if (customer is null)
		{
			return NotFound();
		}

		return Ok(customer);
	}

	[HttpGet]
	public IActionResult GetAll()
	{
		IReadOnlyList<Customer> customers = _unitOfWork.Customers.GetAll();
		return Ok(customers);
	}

	[HttpPost]
	public IActionResult Create([FromBody] Customer item)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (_unitOfWork.Customers.GetByEmail(item.Email) is not null)
			{
				return BadRequest("Email is already in use: " + item.Email);
			}

			item.Status = CustomerStatus.Regular;
			_unitOfWork.Customers.Add(item);
			_unitOfWork.SaveChanges();

			return Ok();
		}
		catch (Exception e)
		{
			return StatusCode(500, new { error = e.Message });
		}
	}

	[HttpPut]
	[Route("{id}")]
	public IActionResult Update([FromRoute] long id, [FromBody] Customer item)
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

			customer.Name = item.Name;
			_unitOfWork.SaveChanges();

			return Ok();
		}
		catch (Exception e)
		{
			return StatusCode(500, new { error = e.Message });
		}
	}

	[HttpPost]
	[Route("{id}/movies")]
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
	[Route("{id}/promotion")]
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
