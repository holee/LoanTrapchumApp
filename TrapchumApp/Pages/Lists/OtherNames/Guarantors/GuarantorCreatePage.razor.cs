using Applications.Features.Guarantor.Command.CreateGuarantor;
using Applications.Features.Guarantor.Dtos;
using Domains.Models.Bases;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Lists.OtherNames.Guarantors
{
    public partial class GuarantorCreatePage
    {
        [Parameter] public EventCallback ClosedTab { get; set; }
        [Parameter] public EventCallback<VGuarantor> GetGuarantor { get; set; } 
        private EntityGuarantorCreate model { get; set; } = new();
        async Task TabClosing()
        {
            await ClosedTab.InvokeAsync();
        }

        protected async Task OnSaveChanges()
        {
            model.EntType = EntType.OtherName.ToString();

            model.Id = Guid.CreateVersion7().ToString();

            var command = new CreateGuarantorCommand(model);

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success, "New Guarantor was created successfully.");

                await GetGuarantor.InvokeAsync(rsp.Value);

                model = new();

                await TabClosing();
            }

            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
            }
        }
    }
}