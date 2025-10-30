using StudentManagementSystem.Models.AuthModels;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.StudentsDtos;

namespace StudentManagementSystem.Repositories.AuthRepositories
{
    public interface IAuthRepository
    {
        Task<Response<object>> SignUpAsync(SignUp signUp);
        Task<Response<object>> SignInAsync(SignIn signIn);
        Task<Response<object>> GetUserByEmailAsync(string Email);
        
    }
}
