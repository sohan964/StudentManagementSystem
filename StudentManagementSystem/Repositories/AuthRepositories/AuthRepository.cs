using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StudentManagementSystem.Models;
using StudentManagementSystem.Models.AuthModels;
using StudentManagementSystem.Models.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagementSystem.Repositories.AuthRepositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthRepository(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        //signup
        public async Task<Response<object>> SignUpAsync(SignUp signUp)
        {
            var newUser = new ApplicationUser()
            {
                FullName = signUp.FullName,
                Email = signUp.Email,
                UserName = signUp.Email,
                UpdatedAt = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await _userManager.CreateAsync(newUser, signUp.Password!);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "User");
            }
            else
            {
                return new Response<object> (false, "SignUp error", result);
            }
            return new Response<object> (true, "Signup Success", result);

        }

        //signin
        public async Task<Response<object>> SignInAsync(SignIn signIn)
        {
            var user = await _userManager.FindByEmailAsync(signIn.Email!);
            if(user == null)  return new Response<object> (false,"User not found", user);
            if(!await _userManager.CheckPasswordAsync(user, signIn.Password!))
            {
                return new Response<object>(false, "Wrong Password", user);
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var jwtToken = GetToken(authClaims);
            return new Response<object>(true, "Login Success", new{
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = jwtToken.ValidTo,
            });
        }

        //JWT token Generator
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

    }
}
