using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SolarWatch.Services.Authentication
{
    public interface ITokenService
    {
        public string CreateToken(IdentityUser user, string role);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
