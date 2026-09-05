using System.Security.Claims;
using Domains.Models.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Web.UI.Shared;

namespace Web.UI.Layout;

public partial class MainLayout : IDisposable
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

        if (_user?.Identity?.IsAuthenticated == true)
        {
            userId = _user.GetId();

            await GetBranchState();

            await bState.GetAuthenticatedUser(_user);
        }

        await base.OnInitializedAsync();
    }

    async Task GetBranchState()
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
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;

        if (_loadingHandler is not null)
        {
            Tracker.OnLoadingChanged -= _loadingHandler;
        }

        if (bState is not null)
        {
            bState.OnChange -= StateHasChanged;
        }
    }
}
