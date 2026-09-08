namespace Shared.DTOs.Account;

public class DeactivateAccountRequest
{
    public const int MaxReasonLength = 500;

    public bool Confirm { get; set; }

    public string? Password { get; set; }

    public string? Reason { get; set; }
}

public class DeactivatedAccountDto
{
    public string UserId { get; set; } = default!;

    public int Status { get; set; }

    public string StatusName { get; set; } = default!;

    public DateTime DeactivatedAt { get; set; }

    public int ListingsHidden { get; set; }
}
