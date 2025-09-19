using Applications.Features.Addresses.Responses;
using Applications.Interfaces.Addresses;
using Azure;
using Domains.Models.Addresses;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Lists.Operations.Communes
{
    public partial class CommuneCreatePage
    {

        private readonly IDistrictService districtService;
        private readonly ICommuneService communeService;

        public CommuneCreatePage(IDistrictService districtService, ICommuneService communeService)
        {
            this.districtService = districtService;
            this.communeService = communeService;
        }
        [Parameter] public int? Id { get; set; } 
        protected List<District> districts = [];
        protected CommuneDtoCreate model = new CommuneDtoCreate();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await districtService.GetAllAsync(null);
                if (results.IsSuccess)
                {

                    districts = results.Data!.ToList();
                    model.DistrictRefId = Id ?? 0;
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
            
            await base.OnInitializedAsync();
        }
        protected async Task Create()
        {
            try
            {

                var response = await communeService.CreateAsync(model);

                if (response!.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Commune new record was created successful.");
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Warning, response.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error,ex.Message);
            }
        }
    }
}