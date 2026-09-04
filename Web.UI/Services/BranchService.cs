using Applications.Features.Branches.Queries.GetBranchByUser;
using Applications.Features.Branches.Queries.GetBranchState;
using Applications.Utils;

namespace Web.UI.Services;

public interface IBranchServiceBase
{
    event Action<bool>? OnStateChanged;
    int? BranchId { get; set; }
    string? BranchName { get; set; }
    bool State { get; set; }
    Task GetBranchState();
    Task GetCurrentBranchIdAsync();
    Task GetCurrentBranchNameAsync();
    void NotifyBranchStateChanged();
}

public sealed class BranchService
{
    public event Action<bool>? OnStateChanged;
    public bool State { get; internal set; } = true;
    public string? BranchName { get; private set; } = "Main Branch";
    public int? BranchId { get; private set; } = 1;
    public bool? IsHeadOffice { get; private set; } = true;

    public Task GetCurrentBranchIdAsync()
    {
        BranchId = 1;
        return Task.CompletedTask;
    }

    public Task GetCurrentBranchNameAsync()
    {
        BranchName = "Main Branch";
        IsHeadOffice = true;
        return Task.CompletedTask;
    }

    public Task GetBranchState()
    {
        State = true;
        BranchId = 1;
        NotifyBranchStateChanged();
        return Task.CompletedTask;
    }

    public void NotifyBranchStateChanged() => OnStateChanged?.Invoke(State);
}
