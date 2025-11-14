using Applications.Features.Employees.Dto;
using Domains.Models.Bases;

namespace TrapchumApp.Pages.Lists.Employees
{
    public partial class CreateEmployee
    {
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public EventCallback Reloaded { get; set; }
        async void Reload()
        {
            await Reloaded.InvokeAsync();
        }
        private async Task CloseTab()
        {
            await OnClose.InvokeAsync(false);
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

                var createResponse = await _employeeService.CreateAsync(model);

                if (createResponse.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, createResponse.Data);
                    model = new();
                    await CloseTab();
                    Reload();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, createResponse.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }

        }

    }
}
