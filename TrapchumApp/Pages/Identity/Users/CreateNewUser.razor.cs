using Applications.Features.Branches.Dtos;
using Applications.Features.Identity.Users;
using Applications.Interfaces.Identity;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;

namespace TrapchumApp.Pages.Identity.Users;

public partial class CreateNewUser
{
    private readonly IRoleService _roleService;

    public CreateNewUser(IRoleService roleService)
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
            var roleResult = await _roleService.GetRolesAsync(string.Empty);

            if (roleResult.IsSuccess)
            {
                roles = roleResult?.Data ?? [];
            }

            var rsp = await _employeeService.GetEmployeesAsync(0, null, new(0, 0));

            if (rsp.Items.Any())
            {
                Employees = rsp.Items;
            }

            var bRsp = await _branchService.GetBranchesAsync<BranchDto>(0, new QueryParam(0, 0));

            if (bRsp!.IsSuccess)
            {
                Branches = bRsp.Data.Items;
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
            var rsp = await _employeeService.GetEmployeesAsync(model.BranchRefId, string.Empty, new(0, 0));

            bEmployees = rsp.Items ?? [];
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