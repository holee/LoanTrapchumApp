using Applications.Features.Employees.Commands.EditEmployee;
using Applications.Features.Employees.Dto;
using Applications.Features.Employees.Queries.GetEditEmployee;
using Applications.Responses.ViewModels.Employees;

namespace Web.UI.Pages.Lists.Employees
{
    public partial class EmployeeEditPage
    {
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback<VEmployeeList> UpdateChanged { get; set; }
        [Parameter] public string? EmployeeId { get; set; }
        [Parameter] public EventCallback Reloaded { get; set; }

        protected EntityEmployeeCreate model = new EntityEmployeeCreate
        {
            Id = $"emp-{Guid.CreateVersion7()}"
        };
        protected EditForm? editForm;
        protected string? strText = "";
        protected bool IsActive = false;
        private bool _isPageLoading = false;
        private async Task CloseTab()
        {
            await OnClose.InvokeAsync(false);
        }


        protected override async Task OnInitializedAsync()
        {

            await GetEditEmployee();

            await base.OnInitializedAsync();
        }


        async Task GetEditEmployee()
        {

            _isPageLoading = true;

            var query = new GetEditEmployeeQuery(EmployeeId);

            var rsp = await mediator.Send(query);

            if (rsp.IsSuccess())
            {
                model = rsp.Value;

                model.Photo ??= "/assets/profiles/profile.png";
            }

            _isPageLoading = false;

            StateHasChanged();
        }

        protected async Task FormValidSubmit()
        {
            var command = new EditEmployeeCommand(model);

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {
                _toastService.ShowSuccess("Employee was updated successfully.", "Success");

                await UpdateChanged.InvokeAsync(rsp.Value);

            }
            else
            {
                _toastService.ShowError(rsp.GetMessage(), "Error");
            }
        }
    }
}
