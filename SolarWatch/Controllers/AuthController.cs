using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Contracts;
using SolarWatch.Services.Authentication;

namespace SolarWatch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authenticationService;

        public record AuthRequest(string Email, string Password);
        public record AuthResponse(string Email, string UserName, string Token);

        public AuthController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("renew-token")]
        [Authorize]
        public async Task<ActionResult<AuthResponse>> RenewToken()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = await _authenticationService.RenewTokenAsync(token);

            if (!result.Success)
            {
                return BadRequest("Failed to renew token");
            }

            return Ok(new AuthResponse(result.Email, result.UserName, result.Token));
        }

        [HttpPost("Logout")]
        [Authorize]
        public IActionResult Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var tokenBlacklistService = HttpContext.RequestServices.GetRequiredService<ITokenBlacklistService>();
            tokenBlacklistService.AddTokenToBlacklist(token);

            return Ok(new { message = "Logged out successfully" });
        }


        [HttpGet("testUser")]
        [Authorize(Roles = "User, Admin")]
        public ActionResult<bool> Test()
        {
            return Ok(true);
        }

        [HttpGet("testAdmin"), Authorize(Roles = "Admin")]
        public ActionResult<bool> TestAdmin()
        {
            return Ok(true);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.RegisterAsync(request.Email, request.Username, request.Password, "User");

            if (!result.Success)
            {
                AddErrors(result);
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(Register), new RegistrationResponse(result.Email, result.UserName));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.LoginAsync(request.Email, request.Password);

            if (!result.Success)
            {
                AddErrors(result);
                return BadRequest(ModelState);
            }

            return Ok(new AuthResponse(result.Email, result.UserName, result.Token));
        }

        private void AddErrors(AuthResult result)
        {
            foreach (var error in result.ErrorMessages)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }
    }
}
