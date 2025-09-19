
using Applications.Features.Identity.Users;
using Applications.Interfaces.Identity;
using Applications.Responses;

namespace Applications.Services.Identity;



public sealed class LoginService(ILoginService loginService) : ILoginServiceImpl
{
    public async Task<Result<string>> LoginUser(LoginRequest request)
    {
        return await loginService.LoginUser(request); 
    }
}
