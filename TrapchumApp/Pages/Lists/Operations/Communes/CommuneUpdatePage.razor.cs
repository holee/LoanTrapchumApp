using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;
using Applications.Responses.Message;
using Azure;
using Domains.Models.Addresses;
using Mapster;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Lists.Operations.Communes
{
    public partial class CommuneUpdatePage
    {
        [Parameter] public int Id { get; set; }
        private readonly IAddressService districtService;
        private readonly IAddressService communeService;

        public CommuneUpdatePage(IAddressService communeService)
        {
            this.districtService = communeService;
            this.communeService = communeService;
        }
        protected List<District> districts = [];
        protected CommuneDtoUpdate model = new CommuneDtoUpdate();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await districtService.GetAllDistrictsAsync(null);
                if (!results.IsError)
                {

                    districts = results.Value.ToList();
                }
                var response = await communeService.GetCommuneByIdAsync(Id);

                if (!response.IsError)
                {
                    model = response.Value.Adapt<CommuneDtoUpdate>();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Commune", response.FirstError.Description);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
            
            await base.OnInitializedAsync();
        }
        protected async Task Update()
        {
            try
            {

                //Console.WriteLine(req);
                var response = await communeService.UpdateCommuneAsync(model);
                if (!response.IsError)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Commune Updated", Messages.DataUpdated);

                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, response.FirstError.Description);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }
    }
}