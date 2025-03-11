
using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Responses;

namespace Applications.Services.Identity;

public class LoginServiceImpl : ILoginServiceImpl
{
    private readonly ILoginService _loginService;

    public LoginServiceImpl(ILoginService loginService)
    {
        _loginService = loginService;
    }

    public async Task<Result<string>> LoginUser(LoginRequest request)
    {
        return await _loginService.LoginUser(request);
    }
}
