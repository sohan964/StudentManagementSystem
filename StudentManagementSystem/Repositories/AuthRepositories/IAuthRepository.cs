using StudentManagementSystem.Models.AuthModels;
using StudentManagementSystem.Models.Components;

namespace StudentManagementSystem.Repositories.AuthRepositories
{
    public interface IAuthRepository
    {
        Task<Response<object>> SignUpAsync(SignUp signUp);
        Task<Response<object>> SignInAsync(SignIn signIn);
    }
}
