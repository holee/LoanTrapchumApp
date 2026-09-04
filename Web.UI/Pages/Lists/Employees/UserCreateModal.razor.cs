using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;
using Applications.Features.Employees.Queries.GetEmployeeAsPaging;
using Applications.Features.Identity.Command.CreateNewUser;
using Applications.Features.Identity.Roles.Queries.GetRoles;
using Applications.Features.Identity.Users;
using Applications.Responses.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace Web.UI.Pages.Lists.Employees
{
    public partial class UserCreateModal
    {
        [Parameter] public string? EmployeeId { get; set; } = string.Empty;

        [Parameter] public int BranchId { get; set; } = 0;

        RegisterRequest _model = new RegisterRequest();

        EditForm _form = default!;

        IEnumerable<VBranch> _branches = [];

        IEnumerable<VEmployeeList> _bEmployees = [];

        IEnumerable<string?> _roles = [];

        bool _showBranch = false;

        bool _showEmployee = false;

        private bool _isBusy = false;
        protected override async Task OnInitializedAsync()
        {
            await GetRoles();

            if (!string.IsNullOrEmpty(EmployeeId) && BranchId <= 0)
            {
                _model.EmployeeId = EmployeeId;

                await GetEmployees(EmployeeId);

                var emp = _bEmployees.FirstOrDefault(x => x.EmployeeId == EmployeeId);

                if (emp != null)
                {
                    await GetBranches(emp.BranchId);

                    _model.BranchRefId = emp.BranchId ?? 0;

                    _showBranch = emp.BranchId > 0;

                    _showEmployee = !string.IsNullOrEmpty(emp.EmployeeId);

                    SetDefaultUserNameAndEmail(emp);
                }
            }

            if (BranchId > 0 && string.IsNullOrEmpty(EmployeeId))
            {
                await GetBranches(BranchId);

                await GetEmployees(EmployeeId, BranchId);

                _model.BranchRefId = BranchId;

                _showBranch = BranchId > 0;
            }

            if (BranchId <= 0 && string.IsNullOrEmpty(EmployeeId))
            {
                await GetBranches(BranchId);

                await GetEmployees(EmployeeId, BranchId);

                _model.BranchRefId = BranchId;

                _showBranch = BranchId > 0;
            }

            await base.OnInitializedAsync();
        }

        private async Task GetRoles()
        {
            var rsp = await mediator.Send(new GetRolesQuery(null));

            _roles = rsp.ToList();
        }

        private async Task GetBranches(int? branchId = 0)
        {
            var command = new GetBranchAsPagingQuery(branchId, new QueryParam(0, 0, string.Empty));

            var rsp = await mediator.Send(command);

            _branches = rsp.Items;
        }

        void SetDefaultUserNameAndEmail(VEmployeeList? emp = null)
        {
            if (emp is not null && !string.IsNullOrWhiteSpace(emp.FullName))
            {
                var username = emp.FullName.Split(" ");

                _model.Username = $"{username[0]}{(username.Length > 1 ? username[1] : "")}".ToLower();

                _model.Email = $"{username[0]}{(username.Length > 1 ? username[1] : "")}@gmail.com".ToLower();

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
            var command = new GetEmployeeAsPagingQuery(branchId, employeeId, new QueryParam(0, 0));

            var rsp = await mediator.Send(command); 

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
                var command = new CreateNewUserCommand(_model);

                var result = await mediator.Send(command);

                if (result.IsSuccess())
                {
                    NotificationService.Notify(NotificationSeverity.Success, result.Value);

                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, result.GetMessage());
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
