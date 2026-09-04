using Application.Features.Identity.Users;
using Application.Responses;

namespace Application.Services.Identity;

public interface ILoginServiceImpl 
{
    Task<Result<string>> LoginUser(LoginRequest request); 
}
