
using Application.Features.Identity.Users;
using Application.Interfaces.Identity;
using Application.Responses;

namespace Application.Services.Identity;



public sealed class LoginService(ILoginService loginService) : ILoginServiceImpl
{
    public async Task<Result<string>> LoginUser(LoginRequest request)
    {
        return await loginService.LoginUser(request); 
    }
}
