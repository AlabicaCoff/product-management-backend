using ProductManagement.Api.DTOs;
using ProductManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ProductManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        // POST: {apiBaseUrl}/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await _authService.LoginAsync(request);

            if (response.IsLoginSuccess)
            {
                return Ok(response);
            }

            ModelState.AddModelError("", response.FailureMessage ?? "Login failed.");
            return ValidationProblem(ModelState);
        }
    }
}
