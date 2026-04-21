using Applications.Features.Employees.Commands.CreateEmployee;
using Applications.Features.Employees.Dto;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Bases;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Lists.Employees
{
    public partial class CreateEmployee
    {
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback<VEmployeeList> UpdateStateChanged { get; set; }
        private async Task CloseTab()
        {
            await OnClose.InvokeAsync();
        }
        protected EntityEmployeeCreate model = new EntityEmployeeCreate();

        protected override void OnInitialized()
        {

            model.Photo = "/assets/profiles/profile.png";

            base.OnInitialized();
        }
        protected async Task FormValidSubmit()
        {

            model.Id = $"{Guid.CreateVersion7()}";

            model.EntType = EntType.Employee.ToString();

            var command = new CreateEmployeeCommand(model);

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {
                _toastService.ShowSuccess("Employee","New Employee was Created");

                await OnClose.InvokeAsync();

                await UpdateStateChanged.InvokeAsync(rsp.Value);

            }
            else
            {
                _toastService.ShowError("Employee", rsp.GetMessage());
            }
        }

    }
}
