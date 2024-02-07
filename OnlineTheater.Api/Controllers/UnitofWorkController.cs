using Microsoft.AspNetCore.Mvc;
using OnlineTheater.Logic.Utils;

namespace OnlineTheater.Api.Controllers;

public class UnitofWorkController : ControllerBase
{
	private readonly UnitOfWork _unitOfWork;

	public UnitofWorkController(UnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	protected new IActionResult Ok()
	{
		_unitOfWork.SaveChanges();
		return base.Ok(); ;
	}

	protected new IActionResult Ok<T>(T result)
	{
		_unitOfWork.SaveChanges();
		return base.Ok(result); ;
	}
}
