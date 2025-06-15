using Applications.Requests.Lists.OtherNames;
using Applications.Services.Lists;
using Domains.Models.Bases;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Lists.OtherNames
{
    public partial class GuarantorCreate
    {
        private readonly IGuarantorServiceImpl _gService;

        public GuarantorCreate(IGuarantorServiceImpl gService) 
        {
            _gService = gService;
        }

        private EntityGuarantorCreate model { get; set; } = new();
        [Parameter] public EventCallback<int> Closed { get; set; } 
        async Task TabClosing()
        {
            await Closed.InvokeAsync(1);
        }

      
        protected async Task OnSaveChanges()
        {
            model.EntType = EntType.OtherName.ToString();

            model.Id = Guid.CreateVersion7().ToString();

            var rsp = await _gService.CreateAsync(model);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);

                model = new();

                await TabClosing();
            }

            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }
    }
}