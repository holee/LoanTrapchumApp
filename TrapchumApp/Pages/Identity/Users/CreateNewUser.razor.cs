using Applications.Features.Branches.Dtos;
using Applications.Features.Identity.Roles.Queries.GetRoles;
using Applications.Features.Identity.Users;
using Applications.Responses.ViewModels.Employees;

namespace TrapchumApp.Pages.Identity.Users;

public partial class CreateNewUser(ILogger<CreateNewUser> logger)
{
    RegisterRequest model = new RegisterRequest();
    bool IsSuccess = false;
    bool IsVisible = false;
    string? message = "";
    EditForm form = default!;
    IEnumerable<string?> roles = new List<string>();
    IEnumerable<VEmployeeList> Employees = [];
    IEnumerable<VBranch> Branches = [];
    IEnumerable<VEmployeeList> bEmployees = [];
    BranchDto SelectedBranch = default!;
    private bool _isBusty = false;
    protected override async Task OnInitializedAsync()
    {
        await GetRoles();

        await base.OnInitializedAsync();
    }

    private async Task GetRoles()
    {
        var command = new GetRolesQuery(string.Empty);

        var roleResult = await mediator.Send(command); //_roleService.GetRolesAsync(string.Empty);

        roles = roleResult ?? [];

    }

    protected void SelectedEmployeeChanged()
    {

        logger.LogInformation("Selected Employee: {EmployeeId}", model.EmployeeId);

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
    }
    protected async void HanldeSubmitForm()
    {
        try
        {
            logger.LogInformation("Creating new user with username: {Username}", model.Username);

            _isBusty = true;

            var result = await _userService.CreateUserAsync(model);

            if (result.IsSuccess)
            {
                _toastService.ShowSuccess("New user was created.");

                model = new();

                StateHasChanged();
            }

            _isBusty = false;
        }
        catch (Exception ex)
        {
            _isBusty = false;

            logger.LogError(ex, "Error creating new user.");
        }
    }
}