using Applications.Requests.Lists.OtherNames;
using Applications.Services.Lists;
using Domains.Models.Bases;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Lists.OtherNames
{
    public partial class GaurantorCreateComp
    {
        private readonly IGuarantorServiceImpl _gService;

        public GaurantorCreateComp(IGuarantorServiceImpl gService)
        {
            _gService = gService;
        }

        [Parameter] public EntityGuarantorCreate model { get; set; } = new();
        [Parameter] public EventCallback<int> Closed { get; set; } 
        async Task TabClosing()
        {
            await Closed.InvokeAsync(1);
        }

        void SetFirstName(string e)
        {
            var value = e ?? string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                var names = value.Split(" ");
                if (names.Length == 1)
                {
                    model.FirstName = names[0];
                    model.LastName = string.Empty;
                }
                if (names.Length == 2)
                {
                    model.FirstName = names[0];
                    model.LastName = names[1];
                }
            }
            else
            {
                model.FirstName = string.Empty;
                model.LastName = string.Empty;
            }

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