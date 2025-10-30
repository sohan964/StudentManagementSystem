using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthRepository authRepository, UserManager<ApplicationUser> userManager)
        {
            _authRepository = authRepository;
            _userManager = userManager;
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

        [HttpGet("current-user")]
        public async Task<IActionResult> CurrentUser()
        {
            var email = HttpContext.User?.Claims.First().Value;
            if (email == null) return Unauthorized("Not a Valid Token");
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound("user not found");
            var role = await _userManager.GetRolesAsync(user);
            return Ok(

                new Response<object>(true, "The current user",
                new
                {
                    user.FullName,
                    user.Email,
                    user.Id,
                    role,
                    user.PhoneNumber,
                }));
        }

        [HttpGet("getuser/{email}")]
        public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
        {
            var result = await _authRepository.GetUserByEmailAsync(email);
            if(! result.Success) return NotFound(result);
            return Ok(result);
        }


    }
}
