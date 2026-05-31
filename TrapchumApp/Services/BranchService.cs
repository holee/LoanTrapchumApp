using Applications.Features.Branches.Queries.GetBranchByUser;
using Applications.Features.Branches.Queries.GetBranchState;
using Applications.Interfaces.Identity;
using Applications.Utils;

namespace TrapchumApp.Services;

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

public sealed class BranchService(ICurrentUserService currentUserService, IServiceScopeFactory _scopeFactory)
{
    public event Action<bool>? OnStateChanged;
    public bool State { get; internal set; } = false;
    public string? BranchName { get; private set; } = string.Empty;
    public int? BranchId { get; private set; }
    public bool? IsHeadOffice { get; private set; } = false;

    public async Task GetCurrentBranchIdAsync()
    {

        var currentUserId = currentUserService.UserId;

        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var currentBranch = await mediator.Send(new GetBranchByUserQuery(currentUserId));

        if (currentBranch.IsError)
        {
            BranchId = 0;
        }
        else
        {
            BranchId = currentBranch.Value.BranchId;
        }
    }

    public async Task GetCurrentBranchNameAsync()
    {

        var currentUserId = currentUserService.UserId;

        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var currentBranch = await mediator.Send(new GetBranchByUserQuery(currentUserId));

        if (currentBranch.IsError)
        {
            BranchName = string.Empty;

            IsHeadOffice = false;
        }
        else
        {

            BranchName = currentBranch.Value.BranchName;

            IsHeadOffice = currentBranch.Value.IsHeadOffice;
        }
    }

    public async Task GetBranchState()
    {

        var getBranch = new GetBranchStateQuery(currentUserService.UserId);

        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var rsp = await mediator.Send(getBranch);

        if (!rsp.IsError)
        {
            State = rsp.Value.IsOpen;

            BranchId = rsp.Value.BranchId;
        }
        else
        {
            State = false;

            BranchId = rsp.Value.BranchId;
        }

        NotifyBranchStateChanged();

    }


    //public async Task IsHeadOffice() 
    //{

    //    var getBranch = new GetBranchStateQuery(currentUserService.UserId);

    //    using var scope = _scopeFactory.CreateScope();

    //    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

    //    var rsp = await mediator.Send(getBranch);

    //    if (!rsp.IsError)
    //    {
    //        State = rsp.Value.IsOpen;

    //        BranchId = rsp.Value.BranchId;
    //    }
    //    else
    //    {
    //        State = false;

    //        BranchId = rsp.Value.BranchId;
    //    }

    //}

    public void NotifyBranchStateChanged() => OnStateChanged?.Invoke(State);

}
