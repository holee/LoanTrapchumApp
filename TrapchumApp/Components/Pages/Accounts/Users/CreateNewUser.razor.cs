using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Mapster;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace TrapchumApp.Components.Pages.Accounts.Users;

public partial class Create
{
    private readonly IRoleService _roleService;

    public Create(IRoleService roleService)
    {
        _roleService = roleService;
    }

    RegisterRequest model = new RegisterRequest();
    bool IsSuccess = false;
    bool IsVisible = false;
    string? message = "";
    EditForm form = default!;
    IEnumerable<string?> roles = new List<string>();
    IEnumerable<VEmployeeList> Employees = [];
    IEnumerable<BranchDto> Branches = [];
    IEnumerable<VEmployeeList> bEmployees = [];
    BranchDto SelectedBranch = default!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var roleResult = await _roleService.GetRolesAsync(String.Empty);

            if (roleResult.IsSuccess)
            {
                roles = roleResult?.Data ?? [];
            }

            var employeeResponse = await _employeeService.GetEmployeesAsync(string.Empty, 0);

            if (employeeResponse.IsSuccess)
            {
                Employees = employeeResponse.Data!;
            }

            var branchResponse = await _branchService.GetBranchesAsync<BranchDto>(0, new QueryParam(0, 25, string.Empty));

            if (branchResponse!.IsSuccess)
            {
                Branches = branchResponse.Data.Items;
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
            var employeeResponse = await _employeeService.GetEmployeesAsync(string.Empty, model.BranchRefId);

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