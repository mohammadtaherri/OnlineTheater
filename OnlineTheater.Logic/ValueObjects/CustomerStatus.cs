﻿namespace OnlineTheater.Logic.ValueObjects;

public class CustomerStatus : ValueObject<CustomerStatus>
{
	public static readonly CustomerStatus Regular = new(
		CustomerStatusType.Regular, 
		ExpirationDate.Infinite);

    public CustomerStatusType Type { get; }

	private readonly DateTime? _expirationDate;
	public ExpirationDate ExpirationDate => (ExpirationDate)_expirationDate;

	public bool IsAdvanced => Type == 
		CustomerStatusType.Advanced && 
		!ExpirationDate.IsExpired;

	private CustomerStatus(CustomerStatusType type, ExpirationDate expirationDate)
	{
		Type = type;
		_expirationDate = expirationDate 
			?? throw new ArgumentNullException(nameof(expirationDate));
	}

	public CustomerStatus Promote()
	{
		return new CustomerStatus(
			CustomerStatusType.Advanced,
			(ExpirationDate)DateTime.UtcNow.AddYears(1));
	}

	protected override bool EqualsCore(CustomerStatus other)
	{
		return Type == other.Type && ExpirationDate == other.ExpirationDate;
	}

	protected override int GetHashCodeCore()
	{
		return Type.GetHashCode() ^ ExpirationDate.GetHashCode();
	}
}

public enum CustomerStatusType
{
    Regular = 1,
    Advanced = 2
}