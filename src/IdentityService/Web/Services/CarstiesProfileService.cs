using System.Security.Claims;
using Domain.ApplicationUser;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace Web.Services;

public class CarstiesProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CarstiesProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await this._userManager.GetUserAsync(context.Subject);
        if (user is not null)
        {
            var claims = await this._userManager.GetClaimsAsync(user);

            var fullnameClaim = claims.FirstOrDefault(p => p.Type.Equals(JwtClaimTypes.Name));

            if (!string.IsNullOrEmpty(user.UserName))
            {
                var newClaims = new List<Claim>
                {
                    new Claim("username", user.UserName)
                };
                context.IssuedClaims.AddRange(newClaims);
            }

            if (fullnameClaim is { Value.Length: > 0 }) context.IssuedClaims.Add(fullnameClaim);
        }
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}