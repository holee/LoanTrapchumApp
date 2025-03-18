
using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Responses;

namespace Applications.Services.Identity;

public sealed class LoginService : ILoginService
{
    private readonly ILoginService _loginService;

    public LoginService(ILoginService loginService)
    {
        _loginService = loginService;
    }

    public async Task<Result<string>> LoginUser(LoginRequest request)
    {
        return await _loginService.LoginUser(request);
    }
}
