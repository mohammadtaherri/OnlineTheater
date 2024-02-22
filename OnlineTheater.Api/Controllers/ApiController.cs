using Microsoft.AspNetCore.Mvc;
using OnlineTheater.Api.Utils;
using OnlineTheater.Logic.Utils;

namespace OnlineTheater.Api.Controllers;

public class ApiController : ControllerBase
{
	private readonly UnitOfWork _unitOfWork;

	public ApiController(UnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public override OkResult Ok()
	{
		_unitOfWork.Complete();
		return base.Ok(); 
	}

	public new OkObjectResult Ok<T>(T result)
	{
		_unitOfWork.Complete();
		return base.Ok(result); 
	}


	public new CreatedAtActionResult CreatedAtAction(
		string? actionName, object? routeValues)
	{
		_unitOfWork.Complete();
		return base.CreatedAtAction(actionName, routeValues, null);
	}

	public new CreatedAtActionResult CreatedAtAction<T>(
		string? actionName, object? routeValues, T result)
	{
		_unitOfWork.Complete();
		return base.CreatedAtAction(actionName, routeValues, Envelope.Ok(result));
	}

	public override NoContentResult NoContent()
	{
		_unitOfWork.Complete();
		return base.NoContent();
	}

	public new NotFoundObjectResult NotFound(string errorMessage)
	{
		return base.NotFound(Envelope.Error(errorMessage));
	}

	public new BadRequestObjectResult BadRequest(string errorMessage)
	{
		return base.BadRequest(Envelope.Error(errorMessage));
	}
}
