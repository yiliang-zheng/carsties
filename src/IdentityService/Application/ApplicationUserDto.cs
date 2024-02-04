namespace Application;

public class ApplicationUserDto
{
    public string Id { get; init; }

    public string UserName { get; init; }

    public string Email { get; init; }

    public bool EmailConfirmed { get; init; }

    public string PhoneNumber { get; init; }

    public bool PhoneNumberConfirmed { get; init; }

    public bool TwoFactorEnabled { get; init; }

}