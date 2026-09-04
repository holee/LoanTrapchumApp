using Applications.Features.Settings.Dto;

namespace Web.UI.Services;

public sealed class UserServices
{
    public VUserLoanSizes? UserLoanSize { get; private set; } = new();

    public Task GetLoanSizeByUserAsync()
    {
        UserLoanSize = new();
        return Task.CompletedTask;
    }
}
