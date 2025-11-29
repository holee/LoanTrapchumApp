using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Extensions.Identity.Policies;
using Applications.Features.Entities;
using Applications.Features.Identity.Users;
using Applications.Interfaces.Employees;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Employees;
using Applications.Services.Identity;
using Domains.Models.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace TrapchumApp.Pages.Identity.Users;

public partial class Index
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
    protected int selectedTabIndex = 0;
    IList<VUser>? Users = [];
    string? authenticatedUserId = string.Empty;
    bool isLoading = false;
    int counts = 0;
    bool AdminOnly = false;
    public Index(IUserServiceImpl authService, IEmployeeService empService)
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
    protected List<BranchDto> Branches = [];
    protected IEnumerable<VEmployeeList> bEmployees = [];
    protected BranchDto SelectedBranch = default!;
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
            await GetBranches();

            await LoadAllUsers();

            var user = (await AuthenticationState).User;

            AdminOnly = await _authorizeService.MustHasPermissionAsync(user, Policies.AdminOnly);

            if (user is not null && user.Identity!.IsAuthenticated)
            {
                authenticatedUserId = user.GetId();
            }

        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }



        await base.OnInitializedAsync();
    }


    private async Task GetBranches()
    {
        var rsp =await _branchService.GetBranchesAsync<BranchDto>(0,new QueryParam(1,0,""));

        var unionBranches = new List<BranchDto>()
        {
            new BranchDto
            {
                BranchId=0,
                BranchName="All"
            }
        };

        if (rsp.IsSuccess)
        {
            var branches = rsp.Data.Items.Union(unionBranches);

            Branches=branches.OrderBy(x=>x.BranchId).ToList();
        }
    }

    async Task LoadAllUsers(string? userId=null, string? filter = null)
    {
        try
        {
            isLoading = true;

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

            isLoading = false;

            StateHasChanged();
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
        }
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

    protected async void SubmitForm()
    {
        try
        {
            form.EditContext?.Validate();

            var result = await _authService.CreateUserAsync(model);

            if (result.IsSuccess)
            {
                IsSuccess = true;
                NotificationService.Notify(NotificationSeverity.Success, "New user was created.");
                model = new();
                StateHasChanged();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, result.ErrMessage);
            }

        }
        catch (Exception ex)
        {
            IsSuccess = false;
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }


    }

    protected async Task SelectBranchChanged()
    {
        if (model.BranchRefId > 0)
        {
            var employeeResponse = await _empService.GetEmployeesAsync(string.Empty, model.BranchRefId);
            if (employeeResponse.IsSuccess)
            {
                bEmployees = employeeResponse.Data!;
            }
        }
        else
        {
            model.BranchRefId = 0;
        }
        model.Username = "";
        model.Email = "";
        model.EmployeeId = string.Empty;
    }
    protected void SelectedEmployeeChanged()
    {

        var selectedEmployee = bEmployees.FirstOrDefault(x => x.EmployeeId == model.EmployeeId);
        if (selectedEmployee is not null)
        {
            var username = selectedEmployee.FullName!.Split(" ");

            model.Username = $"{username[0]}{username[1]}".ToLower();

            model.Email = $"{username[0]}{username[1]}@gmail.com".ToLower();

            model.EmployeeId = selectedEmployee.EmployeeId;
        }
        else
        {
            model.Username = "";
            model.Email = "";
            model.EmployeeId = string.Empty;
        }
        Console.WriteLine(selectedEmployee!.EmployeeId);
    }
    private async Task SelectedBranchChanged(object? args)
    {
        pageIndex = 0;

        if (args is not null)
        {
           selectedBranchId=((BranchDto?)args)!.BranchId;

           await LoadAllUsers(string.Empty);
        }
    }

}