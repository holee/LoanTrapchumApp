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
        [Parameter] public EventCallback<VEmployeeList> Reloaded { get; set; }

        [Parameter] public EventCallback<VEmployeeList> UpdateStateChanged { get; set; }
        async void Reload()
        {
            await Reloaded.InvokeAsync();
        }
        private async Task CloseTab()
        {
            await OnClose.InvokeAsync();
        }
        protected EntityEmployeeCreate model = new EntityEmployeeCreate();

        protected override void OnInitialized()
        {

            model.Photo = "/Profiles/Profile.png";

            base.OnInitialized();
        }
        protected async Task FormValidSubmit()
        {
            try
            {
                model.Id = $"{Guid.CreateVersion7()}";

                model.EntType = EntType.Employee.ToString();

                var command = new CreateEmployeeCommand(model);

                var rsp = await mediator.Send(command);

                if (rsp.IsSuccess())
                {
                    NotificationService.Notify(NotificationSeverity.Success, "New Employee was Created");
                    await OnClose.InvokeAsync();
                    await UpdateStateChanged.InvokeAsync(rsp.Value);

                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }

        }

    }
}
