using System.ComponentModel.DataAnnotations;

namespace OnlineTheater.Api.Dtos;

public class CreateCustomerDto
{
	[MaxLength(100, ErrorMessage = "Name is too long")]
	public string Name { get; set; } = default!;

	[EmailAddress(ErrorMessage = "Email is invalid")]
	public string Email { get; set; } = default!;
}

public class UpdateCustomerDto
{
	[MaxLength(100, ErrorMessage = "Name is too long")]
	public string Name { get; set; } = default!;
}
