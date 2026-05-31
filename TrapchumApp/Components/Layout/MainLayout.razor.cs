using System.Security.Claims;
using TrapchumApp.Shared;
using Domains.Models.Identity;
using Applications.Features.Branches.Queries.GetBranchState;

namespace TrapchumApp.Components.Layout;

public partial class MainLayout : IAsyncDisposable
{
    [CascadingParameter] Task<AuthenticationState> AuthenticateState { get; set; } = default!;
    [CascadingParameter] CascadingAppState bState { get; set; } = default!;

    bool sidebarExpanded = true;

    string? userId = string.Empty;

    int branchId;

    ClaimsPrincipal? _user;

    protected override async Task OnInitializedAsync()
    {
        var auth = await AuthenticateState;

        _user = auth.User;

        if (_user is not null && _user.Identity!.IsAuthenticated == true)
        {
            userId = _user.GetId();

            await GetBranchState(userId);

            await bState.GetAuthenticatedUser(_user);
        }

        await base.OnInitializedAsync();
    }


    async Task GetBranchState(string? userId)
    {
        try
        {
            await branchService.GetBranchState();

            bState.State = branchService.State;

            branchId = branchService.BranchId ?? 0;
        }
        catch (Exception ex)
        {
            _toastService.ShowError(ex.Message);
        }




        //var getBranch = new GetBranchStateQuery(userId);


        //var rsp = await mediator.Send(getBranch);

        //if (rsp.IsSuccess())
        //{
        //    bState.State = rsp.Value.IsOpen;

        //    branchId = rsp.Value.BranchId;
        //}
        //else
        //{
        //    bState.State = false;

        //    branchId = rsp.Value.BranchId;
        //}

    }

    public async ValueTask DisposeAsync()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;

        if (_loadingHandler is not null)
        {
            Tracker.OnLoadingChanged -= _loadingHandler;
        }

        bState.OnChange -= StateHasChanged;

        await _notiService.DisposeAsync();
    }

}