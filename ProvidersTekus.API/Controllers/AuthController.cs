using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProvidersTekus.DLL.Services.Contrato;
using ProvidersTekus.DTO;

namespace ProvidersTekus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Inicia sesión y devuelve un token JWT.
        /// </summary>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var result = await _authService.Login(loginDto);

            if (!result.Success)
                return Unauthorized(new { result.Message });

            return Ok(result);
        }

        /// <summary>
        /// Registra un nuevo usuario y devuelve un token JWT.
        /// </summary>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var result = await _authService.Register(registerDto);

            if (!result.Success)
                return BadRequest(new { result.Message });

            return Ok(result);
        }
    }
}
