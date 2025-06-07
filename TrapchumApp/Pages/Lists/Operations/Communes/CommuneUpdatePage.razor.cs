using Applications.Interfaces.Addresses;
using Applications.Requests.Lists.Addreses;
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
        private readonly IDistrictService districtService;
        private readonly ICommuneService communeService;

        public CommuneUpdatePage(IDistrictService districtService, ICommuneService communeService)
        {
            this.districtService = districtService;
            this.communeService = communeService;
        }
        protected List<District> districts = [];
        protected CommuneDtoUpdate model = new CommuneDtoUpdate();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await districtService.GetAllAsync(null);
                if (results.IsSuccess)
                {

                    districts = results.Data!.ToList();
                }
                var response = await communeService.GetByIdAsync(Id);

                if (response.IsSuccess)
                {
                    model = response.Data!.Adapt<CommuneDtoUpdate>();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Commune", response.ErrMessage);
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
                var response = await communeService.UpdateAsync(model);
                if (response!.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Commune Updated", Messages.DataUpdated);

                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }
    }
}