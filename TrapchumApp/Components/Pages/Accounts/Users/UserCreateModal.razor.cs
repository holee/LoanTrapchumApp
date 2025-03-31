using Applications.Interfaces.Employees;
using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Requests.Lists.Entities;
using Applications.Responses.ViewModels.Employees;
using Mapster;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace TrapchumApp.Components.Pages.Accounts.Users
{
    public partial class UserCreateModal
    {
        private readonly IRoleService _roleService;
        
        public UserCreateModal(IRoleService roleService)
        {
            _roleService = roleService;
        }

        RegisterRequest model = new RegisterRequest();
        EditForm form = default!;
        IEnumerable<BranchDto> Branches = [];
        IEnumerable<VEmployees> bEmployees = [];
        IEnumerable<string?> roles = [];
        IEnumerable<VEmployees> Employees = [];
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var selectedRoles = await _roleService.GetRolesAsync();

            roles = selectedRoles?.Data?.Select(r => r.RoleName) ?? [];

            var employeeResponse = await _employeeService.GetEmployeesAsync(null, 0);

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


        protected async Task SelectBranchChanged()
        {
            if (model.BranchRefId > 0)
            {
                var employeeResponse = await _employeeService.GetEmployeesAsync(null, model.BranchRefId);
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

        protected async void SubmitForm()
        {
            try
            {
                form.EditContext?.Validate();
                var result = await _userService.CreateUserAsync(model);
                if (result.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, result.Data);
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, result.ErrMessage);
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }


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
        }



    }
}