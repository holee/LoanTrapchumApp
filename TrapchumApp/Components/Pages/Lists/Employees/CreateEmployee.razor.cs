using Microsoft.AspNetCore.Components;
using Applications.Requests.Lists.OtherNames;
using Applications.Services.Lists;
using Radzen;
using Domains.Models.Bases;

namespace TrapchumApp.Components.Pages.Lists.Employees
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
        protected EntityNamesCreate model = new EntityNamesCreate();
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
