using Applications.Interfaces.Employees;
using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Employees;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using System.IdentityModel.Tokens.Jwt;
using Applications.Extensions.Identity;
namespace TrapchumApp.Components.Pages.Accounts.Users;

public partial class Index
{
    protected IEnumerable<VUser>? AllUsers = [];
    protected bool IsLoading = true;
    protected int index = 1;
    //protected IEnumerable<RoleDto> roles = [];
    private bool cancelClose = false;
    private string? searchText = "";
    private readonly IUserService _authService;
    private readonly IEmployeeService _empService;
    [CascadingParameter]
    Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    protected int selectedTabIndex = 0;
    IList<VUser>? Users = [];
    string? authenticatedUserId = string.Empty;
    bool isLoading = false;
    int counts = 0;
    public Index(IUserService authService, IEmployeeService empService)
    {
        _authService = authService;
        _empService = empService;
    }

    protected RegisterRequest model = new RegisterRequest();
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
    private string? token = string.Empty;
    protected bool IsSuccess = false;
    protected bool IsVisible = false;
    protected string? message = "";
    protected EditForm form = default!;
    protected IEnumerable<string?> roles = new List<string>();
    protected IEnumerable<VEmployees> Employees = new List<VEmployees>();
    protected IEnumerable<BranchDto> Branches = Enumerable.Empty<BranchDto>();
    protected IEnumerable<VEmployees> bEmployees = Enumerable.Empty<VEmployees>();
    protected BranchDto SelectedBranch = default!;
    string? filterText = string.Empty;
    int pageIndex = 0;
    int pageSize = 10;
    string sortField = nameof(VUser.BranchName);
    bool includeInactive = false;
    bool sortDescending = false;
    protected override async Task OnInitializedAsync()
    {
        try
        {
            await LoadAllUsers();

            var user = (await AuthenticationState).User;

            if (user is not null && user.Identity!.IsAuthenticated)
            {
                authenticatedUserId = user.GetId();
            }

            //var getRoles = await _roleService.GetRolesAsync(0);

            //roles = getRoles?.Select(r => r.RoleName) ?? [];

            //var employeeResponse = await _employeeService.GetEmployeesAsync(0, 0);

            //if (employeeResponse.IsSuccess)
            //{
            //    Employees = employeeResponse.Data!;
            //}
            //var branchResponse = await _branchService.GetBranchesAsync(0, 0, false);

            //if (branchResponse!.IsSuccess)
            //{
            //    Branches = branchResponse.Data!;
            //}
            //else
            //{
            //    Branches = Enumerable.Empty<BranchDto>();
            //}
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }



        await base.OnInitializedAsync();
    }

    async Task LoadAllUsers(long userId = 0, string? filterText = null)
    {
        try
        {
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInactive, filterText);

            var resp = await _authService!.GetUsersAsync(param);

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
        pageIndex = args.Skip!.Value;

        if (!string.IsNullOrEmpty(args.OrderBy))
        {
            sortField = args.OrderBy.Split(" ")[0];

            string dir = args.OrderBy.Split(" ")[1];

            sortDescending = dir.Contains("asc") ? false : true;
        }
        await LoadAllUsers(0, filterText);

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
            var employeeResponse = await _empService.GetEmployeesAsync(Guid.Empty, model.BranchRefId);
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
        model.EmployeeId = Guid.Empty;
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
            model.EmployeeId = Guid.Empty;
        }
        Console.WriteLine(selectedEmployee!.EmployeeId);
    }
}