using System.Text.Json.Serialization;

namespace OnlineTheater.Logic.Entities;

public class Movie : Entity
{
	public string Name { get; set; } = default!;
	public LicensingModel LicensingModel { get; set; }
}
