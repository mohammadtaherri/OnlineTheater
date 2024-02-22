using Microsoft.AspNetCore.Mvc;
using OnlineTheater.Api.Dtos;
using OnlineTheater.FunctionalExtensions;
using OnlineTheater.Logic.Entities;
using OnlineTheater.Logic.Utils;
using OnlineTheater.Logic.ValueObjects;

namespace OnlineTheater.Api.Controllers;

[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
	private readonly UnitOfWork _unitOfWork;

	public CustomersController(UnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
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
			Status = customer.Status.Type.ToString(),
			StatusExpirationDate = customer.Status.ExpirationDate,
			MoneySpent = customer.MoneySpent,
			PurchasedMovies = customer.PurchasedMovies.Select(pm => new PurchasedMovieDto
			{
				Price = pm.Price,
				PurchaseDate = pm.PurchaseDate,
				ExpirationDate = pm.ExpirationDate,
				Movie = new MovieDto
				{
					Id = pm.Movie.Id,
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
				Status = c.Status.Type.ToString(),
				StatusExpirationDate = c.Status.ExpirationDate,
				MoneySpent = c.MoneySpent,
			})
			.ToList();
		return Ok(customerDtos);
	}

	[HttpPost]
	public IActionResult Create([FromBody] CreateCustomerDto dto)
	{
		Result<CustomerName> customerNameOrError = CustomerName.Create(dto.Name);
		Result<Email> emailOrError = Email.Create(dto.Email);

		Result result = Result.Combine(customerNameOrError, emailOrError);
		if (result.IsFailure)
			return BadRequest(result.Error);

		if (_unitOfWork.Customers.GetByEmail(emailOrError.Value) is not null)
			return BadRequest("Email is already in use: " + dto.Email);

		var newCustomer = new Customer(
			customerNameOrError.Value, 
			emailOrError.Value);

		_unitOfWork.Customers.Add(newCustomer);
		_unitOfWork.Complete();

		return Ok();
	}

	[HttpPut]
	[Route("{id:long}")]
	public IActionResult Update([FromRoute] long id, [FromBody] UpdateCustomerDto dto)
	{
		Result<CustomerName> customerNameOrError = CustomerName.Create(dto.Name);
		if (customerNameOrError.IsFailure)
			return BadRequest(customerNameOrError.Error);

		Customer? customer = _unitOfWork.Customers.GetById(id);
		if (customer is null)
			return BadRequest("Invalid customer id: " + id);

		customer.Name = customerNameOrError.Value;
		_unitOfWork.Complete();

		return Ok();
	}

	[HttpPost]
	[Route("{id:long}/movies")]
	public IActionResult PurchaseMovie([FromRoute] long id, [FromBody] long movieId)
	{
		Movie? movie = _unitOfWork.Movies.GetById(movieId);
		if (movie is null)
			return BadRequest("Invalid movie id: " + movieId);
		
		Customer? customer = _unitOfWork.Customers.GetById(id);
		if (customer is null)
			return BadRequest("Invalid customer id: " + id);

		if (customer.CanPurchaseMovie(movie))
			return BadRequest("The movie is already purchased: " + movie.Name);

		customer.PurchaseMovie(movie);

		_unitOfWork.Complete();

		return Ok();
	}

	[HttpPost]
	[Route("{id:long}/promotion")]
	public IActionResult PromoteCustomer([FromRoute] long id)
	{
		Customer? customer = _unitOfWork.Customers.GetById(id);
		if (customer is null)
			return BadRequest("Invalid customer id: " + id);

		Result promotionCheck = customer.CanPromote();
		if (promotionCheck.IsFailure)
			return BadRequest(promotionCheck.Error);

		customer.Promote();

		_unitOfWork.Complete();

		return Ok();
	}
}