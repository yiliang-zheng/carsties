using FluentResults;
using MediatR;

namespace Application.RegisterUser;

public class RegisterUserCommand : IRequest<Result<ApplicationUserDto>>
{
    public string UserName { get; init; }

    public string Email { get; init; }

    public string Password { get; init; }

    public string FullName { get; init; }
}