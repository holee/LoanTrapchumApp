using Applications.Features.Identity.Users;
using Applications.Responses;

namespace Applications.Services.Identity;

public interface ILoginServiceImpl 
{
    Task<Result<string>> LoginUser(LoginRequest request); 
}
