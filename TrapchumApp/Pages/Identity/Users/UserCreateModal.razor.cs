using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace TrapchumApp.Pages.Identity.Users
{
    public partial class UserCreateModal
    {
        [Parameter] public string? EmployeeId { get; set; } = string.Empty;

        [Parameter] public int BranchId { get; set; } = 0;

        private readonly IRoleService _roleService;

        public UserCreateModal(IRoleService roleService)
        {
            _roleService = roleService;
        }

        RegisterRequest _model = new RegisterRequest();

        EditForm _form = default!;

        IEnumerable<BranchDto> _branches = [];

        IEnumerable<VEmployeeList> _bEmployees = [];

        IEnumerable<string?> _roles = [];

        bool _showBranch = false;

        bool _showEmployee = false;

        protected override async Task OnInitializedAsync()
        {
            var selectedRoles = await _roleService.GetRolesAsync();

            _roles = selectedRoles?.Data ?? [];

            if (!string.IsNullOrEmpty(EmployeeId) && BranchId <= 0)
            {
                _model.EmployeeId = EmployeeId;

                await GetEmployees(EmployeeId);

                var emp = _bEmployees.FirstOrDefault(x => x.EmployeeId == EmployeeId);

                await GetBranches(emp!.BranchId);

                _model.BranchRefId = emp.BranchId!.Value;

                _showBranch = emp!.BranchId > 0;

                _showEmployee = !string.IsNullOrEmpty(emp?.EmployeeId);

                SetDefaultUserNameAndEmail(emp);

            }

            if (BranchId > 0 && string.IsNullOrEmpty(EmployeeId))
            {
                await GetBranches(BranchId);

                await GetEmployees(EmployeeId, BranchId);

                _model.BranchRefId = BranchId;

                _showBranch = BranchId > 0;
            }

            await base.OnInitializedAsync();

        }


        async Task GetBranches(int? branchId = 0)
        {
            var rsp = await _branchService.GetBranchesAsync<BranchDto>(branchId, new QueryParam(0, 0, string.Empty));

            _branches = rsp!.IsSuccess ? rsp.Data!.Items : [];
        }


        void SetDefaultUserNameAndEmail(VEmployeeList? emp = null)
        {
            if (emp is not null)
            {
                var username = emp.FullName!.Split(" ");

                _model.Username = $"{username[0]}{username[1]}".ToLower();

                _model.Email = $"{username[0]}{username[1]}@gmail.com".ToLower();

                _model.EmployeeId = emp.EmployeeId;
            }
            else
            {
                _model.Username = "";
                _model.Email = "";
                _model.EmployeeId = string.Empty;
            }
        }
        private async Task GetEmployees(string? employeeId, int branchId = 0)
        {
            var rsp = await _employeeService.GetEmployeesAsync(branchId, employeeId, new QueryParam(0, 0));
            
            _bEmployees = rsp.Items ?? [];

        }

        private async Task SelectBranchChanged()
        {
            if (_model.BranchRefId > 0 && string.IsNullOrWhiteSpace(EmployeeId))
            {
                await GetEmployees(null, _model.BranchRefId);
            }
            else
            {
                _model.BranchRefId = 0;
            }

            _model.Username = "";

            _model.Email = "";

            _model.EmployeeId = string.Empty;
        }

        protected async void SubmitForm()
        {
            try
            {
                _form.EditContext?.Validate();

                var result = await _userService.CreateUserAsync(_model);

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

        private void SelectedEmployeeChanged()
        {
            var selectedEmployee = _bEmployees.FirstOrDefault(x => x.EmployeeId == _model.EmployeeId);

            SetDefaultUserNameAndEmail(selectedEmployee);
        }
    }
}