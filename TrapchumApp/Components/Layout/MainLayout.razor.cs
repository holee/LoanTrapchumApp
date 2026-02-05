using System.Security.Claims;
using TrapchumApp.Shared;
using Domains.Models.Identity;
using Applications.Features.Notifications.Queries.GetNotification;
using Domains.Enumerations;
using TrapchumApp.Helpers.Extensions;
using Applications.Features.Branches.Queries.GetBranchState;

namespace TrapchumApp.Components.Layout
{
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;

            if (_user?.Identity?.IsAuthenticated == true && !string.IsNullOrEmpty(userId))
            {
                // Start SignalR connection via NotificationServices
                var baseUrl = NavigationManager.BaseUri.TrimEnd('/');

                await _notiService.StartConnectionAsync(baseUrl, userId, branchId);

                // Load initial notifications
                //await LoadInitialNotifications();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadInitialNotifications()
        {
            try
            {
                var query = new GetNotificationAsPagingQuery(userId, branchId, NotificationStatus.Unread, new(0, 15, "", false));

                var result = await mediator.Send(query);

                _notiService.Notifications = result.Items;

                _notiService.UnReadCount = result.Count; 

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading notifications: {ex.Message}");
            }
        }

        async Task GetBranchState(string? userId)
        {
            var getBranch = new GetBranchStateQuery(userId);

            var rsp = await mediator.Send(getBranch);

            if (rsp.IsSuccess())
            {
                bState.GetBranchState(rsp.Value.IsOpen);
            }
            else
            {
                bState.GetBranchState(false);
            }

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
}