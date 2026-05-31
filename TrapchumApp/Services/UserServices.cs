using Applications.Extensions.Identity.Enumerations;
using Applications.Features.Settings.Dto;
using Applications.Features.Settings.Queries.UserLoanSizes.GetLoanSizeByUser;
using Applications.Features.Settings.Queries.UserLoanSizes.GetUserLoanSizeById;
using Applications.Interfaces.Identity;
using Applications.Utils;

namespace TrapchumApp.Services;

public sealed class UserServices(ICurrentUserService currentUserService, IServiceScopeFactory scopeFactory)
{

    public VUserLoanSizes? UserLoanSize { get; private set; } = null;

    public async Task GetLoanSizeByUserAsync()
    {
        var userId = currentUserService.UserId;

        if (userId is null)
        {
            throw new Exception("User not found");
        }


        using var scope = scopeFactory.CreateScope();

        var med = scope.ServiceProvider.GetRequiredService<IMediator>();

        var branchResult = await med.Send(new GetLoanSizeByUserQuery(userId));

        if (branchResult.IsError)
        {

            UserLoanSize = new();

            return;
        }

        UserLoanSize = branchResult.Value;
    }


}
