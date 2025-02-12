using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SolarWatch.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResult> RegisterAsync(string email, string username, string password, string role)
        {
            var user = new IdentityUser { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return FailedRegistration(result, email, username);
            }

            await _userManager.AddToRoleAsync(user, role); // Adding the user to a role
            return new AuthResult(true, email, username, "");
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var managedUser = await _userManager.FindByEmailAsync(email);

            if (managedUser == null)
            {
                return InvalidEmail(email);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);
            if (!isPasswordValid)
            {
                return InvalidPassword(email, managedUser.UserName);
            }


            // get the role and pass it to the TokenService
            var roles = await _userManager.GetRolesAsync(managedUser);
            if (roles == null || roles.Count == 0)
            {
                // Handle the case where there are no roles
                return NoRolesAssigned(managedUser.Email, managedUser.UserName);
            }

            var accessToken = _tokenService.CreateToken(managedUser, roles[0]);

            return new AuthResult(true, managedUser.Email, managedUser.UserName, accessToken);
        }

        private static AuthResult NoRolesAssigned(string email, string userName)
        {
            var result = new AuthResult(false, email, userName, "");
            result.ErrorMessages.Add("No roles assigned", "Please contact the administrator");
            return result;
        }

        private static AuthResult InvalidEmail(string email)
        {
            var result = new AuthResult(false, email, "", "");
            result.ErrorMessages.Add("Bad credentials", "Invalid email");
            return result;
        }

        private static AuthResult InvalidPassword(string email, string userName)
        {
            var result = new AuthResult(false, email, userName, "");
            result.ErrorMessages.Add("Bad credentials", "Invalid password");
            return result;
        }

        private static AuthResult FailedRegistration(IdentityResult result, string email, string username)
        {
            var authResult = new AuthResult(false, email, username, "");

            foreach (var error in result.Errors)
            {
                authResult.ErrorMessages.Add(error.Code, error.Description);
            }

            return authResult;
        }

        public async Task<AuthResult> RenewTokenAsync(string token)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            if (principal == null)
            {
                return new AuthResult(false, "","","" );
            }

            var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthResult(false, "", "", "");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Count == 0)
            {
                return NoRolesAssigned(user.Email, user.UserName);
            }

            var newToken = _tokenService.CreateToken(user, roles[0]); // roles[0] might fail if there are more roles / user
            return new AuthResult(true, user.Email, user.UserName, newToken);
        }
    }
}
