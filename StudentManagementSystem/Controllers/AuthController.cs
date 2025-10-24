using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.AuthModels;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Repositories.AuthRepositories;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] SignUp signUp)
        {
            try
            {
                var result = await _authRepository.SignUpAsync(signUp);
                return Ok(result);

            }catch (Exception ex)
            {
                return Unauthorized(new Response<object>(false, "signup error",  ex.Message));
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] SignIn signIn)
        {
            try
            {
                var result = await _authRepository.SignInAsync(signIn);
                if(!result.Success) return BadRequest(result);
                return Ok(result);

            }catch (Exception ex)
            {
                return Unauthorized(new Response<object>(false, "Login error", ex.Message));
            }
        }

    }
}
