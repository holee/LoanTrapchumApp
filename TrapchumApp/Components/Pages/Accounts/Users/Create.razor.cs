using Applications.Interfaces.Employees;
using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Requests.Lists.Entities;
using Applications.Responses.ViewModels.Employees;
using Mapster;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using System.IdentityModel.Tokens.Jwt;
namespace TrapchumApp.Components.Pages.Accounts.Users;

public partial class Create
{

    private readonly IRoleService _roleService;
    private readonly IEmployeeService _employeeService;
    private readonly IUserService _userService;
    public Create(IRoleService roleService, IEmployeeService employeeService, IUserService userService)
    {
        _roleService = roleService;
        _employeeService = employeeService;
        _userService = userService;
    }
    RegisterRequest model = new RegisterRequest();
    readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
    string? token = string.Empty;
    bool IsSuccess = false;
    bool IsVisible = false;
    string? message = "";
    EditForm form = default!;
    IEnumerable<string?> roles = new List<string>();
    IEnumerable<VEmployees> Employees = new List<VEmployees>();
    IEnumerable<BranchDto> Branches = Enumerable.Empty<BranchDto>();
    IEnumerable<VEmployees> bEmployees = Enumerable.Empty<VEmployees>();
    BranchDto SelectedBranch = default!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var roleResult = await _roleService.GetRolesAsync();

            if (roleResult.IsSuccess)
            {
                roles = roleResult?.Data?.Select(r => r.RoleName) ?? [];
            }

            var employeeResponse = await _employeeService.GetEmployeesAsync(0, 0);

            if (employeeResponse.IsSuccess)
            {
                Employees = employeeResponse.Data!;
            }

            var branchResponse = await _branchService.GetBranchesAsync(0, 0, false);

            if (branchResponse!.IsSuccess)
            {
                Branches = branchResponse.Data!.Adapt<IEnumerable<BranchDto>>();
            }
            else
            {
                Branches = Enumerable.Empty<BranchDto>();
            }
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
        await base.OnInitializedAsync();
    }
    protected async Task SelectBranchChanged()
    {
        if (model.BranchRefId > 0)
        {
            var employeeResponse = await _employeeService.GetEmployeesAsync(0, model.BranchRefId);

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
        model.EmployeeId = 0;
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
            model.EmployeeId = 0;
        }
        Console.WriteLine(selectedEmployee!.EmployeeId);
    }

    protected async void SubmitForm()
    {
        try
        {
            form.EditContext?.Validate();

            var result = await _userService.CreateUserAsync(model);

            if (result.IsSuccess)
            {
                IsSuccess = true;
                NotificationService.Notify(NotificationSeverity.Success, "New user was created.");
                model = new();
                StateHasChanged();
            }
            else
            {

            }

        }
        catch (Exception ex)
        {
            IsSuccess = false;
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }


    }

}
