using System.Security.Claims;
using AutoMapper;
using Domain.ApplicationUser;
using FluentResults;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.RegisterUser;

public class RegisterUserCommandHandler:IRequestHandler<RegisterUserCommand, Result<ApplicationUserDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    public async Task<Result<ApplicationUserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.UserName,
            EmailConfirmed = false
        };

        var result = await this._userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await this._userManager.AddClaimsAsync(user, new Claim[]
            {
                new(JwtClaimTypes.Name, request.FullName)
            });
            var dto = this._mapper.Map<ApplicationUserDto>(user);
            return dto;
        }

        return Result.Fail<ApplicationUserDto>(
            result.Errors
                .Select(p =>
                    new Error($"Code: {p.Code}, Description: {p.Description}"))
                .ToList()
        );
    }
}