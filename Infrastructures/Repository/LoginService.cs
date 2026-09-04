using Applications.Features.Identity.Users;
using Applications.Interfaces.Identity;
using Applications.Responses;
using Domains.Models.Identity;
using Microsoft.AspNetCore.Identity;
namespace Infrastructures.Repository;

public class LoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager) :
    ILoginService
{
    public async Task<Result<string>> LoginUser(LoginRequest request)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            var userByUserName = await userManager.FindByNameAsync(request.Email);

            user ??= userByUserName;

            if (user is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("Invalid Email or UsreName."));
            }

            if (user.IsActive == false)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("Current User is not Active."));
            }

            var isPasswordChecked = await userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordChecked)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("Invalid Password."));
            }

            var result = await signinManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false);

            if (!result.Succeeded)
            {
                return Result.Failure<string>(ErrorResponse.ServerError("Something went wrong with login."));
            }
            return Result.Success("Login Success");
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
        }


    }

}
