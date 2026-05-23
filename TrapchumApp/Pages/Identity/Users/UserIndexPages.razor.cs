using Applications.Bases;
using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;
using Applications.Features.Employees.Queries.GetEmployeeAsPaging;
using Applications.Features.Identity.Command.DeleteUser;
using Applications.Features.Identity.Command.ToggleUserActive;
using Applications.Features.Identity.Users;
using Applications.Interfaces.Employees;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Employees;
using Applications.Services.Identity;
using Domains.Models.Identity;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Identity.Users;

public partial class UserIndexPages
{
    protected IEnumerable<VUser>? AllUsers = [];
    protected bool IsLoading = true;
    protected int index = 1;
    private bool cancelClose = false;
    private string? searchText = "";
    private readonly IUserServiceImpl _authService;
    private readonly IEmployeeService _empService;
    private LoadDataArgs? dataGridArgs;
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [CascadingParameter] private CascadingAppState AppState { get; set; } = default!;
    protected int selectedTabIndex = 0;
    IList<VUser>? Users = [];
    string? authenticatedUserId = string.Empty;
    bool isLoading = false;
    int counts = 0;
    bool canDeleteUser = false;
    bool canUpdateUser = false;
    public UserIndexPages(IUserServiceImpl authService, IEmployeeService empService)
    {
        _authService = authService;
        _empService = empService;
    }

    protected RegisterRequest model = new RegisterRequest();
    protected bool IsSuccess = false;
    protected bool IsVisible = false;
    protected string? message = "";
    protected EditForm form = default!;
    protected IEnumerable<string?> roles = [];
    protected IEnumerable<VEmployeeList> Employees = [];
    protected List<VBranch> Branches = [];
    protected IEnumerable<VEmployeeList> bEmployees = [];
    protected VBranch SelectedBranch = default!;
    string? filterText = string.Empty;
    int pageIndex = 0;
    int pageSize = 15;
    string sortField = nameof(VUser.BranchName);
    bool includeInactive = false;
    bool sortDescending = false;
    int selectedBranchId = 0;
    protected override async Task OnInitializedAsync()
    {
        try
        {
            var user = (await AuthenticationState).User;

            await AppState.GetAuthenticatedUser(user);

            authenticatedUserId = user.GetId();

            canDeleteUser = await _authorizeService.MustHasPermissionAsync(user, Modules.Users, Actions.Delete);

            canUpdateUser = await _authorizeService.MustHasPermissionAsync(user, Modules.Users, Actions.Update);

            await GetBranches();

            await LoadAllUsers();


        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }



        await base.OnInitializedAsync();
    }


    private async Task GetBranches()
    {

        var command = new GetBranchAsPagingQuery(0, new QueryParam(0, 0, ""));

        var rsp = await mediator.Send(command);

        var unionBranches = new List<VBranch>()
        {
            new VBranch
            {
                BranchId=0,

                BranchName="All"
            }
        };


        var branches = rsp.Items.Union(unionBranches);

        Branches = branches.OrderBy(x => x.BranchId).ToList();

    }

    async Task LoadAllUsers(string? userId = null, string? filter = null)
    {

        Tracker.Show();

        var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInactive, filter);

        var resp = await _authService.GetUsersAsync(selectedBranchId, userId, param);

        if (resp.IsSuccess)
        {
            counts = resp.Data.Count;

            AllUsers = resp.Data.Items;
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, "Error");
        }

        Tracker.Hide();

        StateHasChanged();

    }



    async Task LoadAllUsers(LoadDataArgs args)
    {

        pageIndex = args.Skip ?? 0;

        if (args?.Sorts?.Any() == true)
        {
            var sort = args.Sorts.First();

            sortField = sort.Property ?? "Id";

            sortDescending = sort.SortOrder == SortOrder.Descending;
        }

        await LoadAllUsers(string.Empty, filterText);

    }
    private async Task SelectedBranchChanged(object? args)
    {
        pageIndex = 0;

        if (args is not null)
        {
            selectedBranchId = ((VBranch)args).BranchId!.Value;

            await LoadAllUsers(string.Empty);
        }
    }

    private async Task DeleteUser(string? userId)
    {
        ContextMenuService.Close();

        if (string.IsNullOrWhiteSpace(userId)) return;

        if (!canDeleteUser)
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Access Denied", "You do not have permission to delete users.");
            return;
        }

        if (userId == authenticatedUserId)
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Not Allowed", "You cannot delete your own account.");
            return;
        }

        var confirm = await MsgBox.ShowAsync("Deleting this user cannot be undone. Would you like to continue?",
            "Confirm Delete",
            MsgBoxButtons.YesNo,
            MsgBoxBoxIcon.Question);

        if (confirm == MsgBoxResult.No) return;

        var command = new DeleteUserCommand(userId);

        var result = await mediator.Send(command);

        if (result.Value.Status == DeleteStatus.Success)
        {
            _toastService.ShowSuccess("User was deleted successfully!");

            await LoadAllUsers(filter: filterText);

            return;
        }




        _toastService.ShowError("Failed to delete user.", "Delete user");

    }

    private async Task ToggleUserActive(string? userId, bool isCurrentlyActive)
    {
        ContextMenuService.Close();

        if (string.IsNullOrWhiteSpace(userId)) return;

        if (!canUpdateUser)
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Access Denied", "You do not have permission to activate/deactivate users.");

            return;
        }

        if (userId == authenticatedUserId)
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Not Allowed", "You cannot change the status of your own account.");
            return;
        }

        var action = isCurrentlyActive ? "deactivate" : "activate";

        var confirm = await MsgBox.ShowAsync(
            $"Are you sure you want to {action} this user?",
            "Confirm",
            MsgBoxButtons.YesNo,
            MsgBoxBoxIcon.Question);

        if (confirm == MsgBoxResult.No) return;

        var command = new ToggleUserActiveCommand(userId);

        var result = await mediator.Send(command);

        if (result.IsSuccess())
        {
            var label = isCurrentlyActive ? "deactivated" : "activated";

            NotificationService.Notify(NotificationSeverity.Success, "User Status", $"User was {label} successfully!");

            await LoadAllUsers(filter: filterText);
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, "User Status", result.FirstError.Description);
        }
    }

}