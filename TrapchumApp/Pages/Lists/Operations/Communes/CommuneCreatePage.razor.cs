using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;
using Domains.Models.Addresses;

namespace TrapchumApp.Pages.Lists.Operations.Communes
{
    public partial class CommuneCreatePage
    {

        private readonly IAddressService districtService;
        private readonly IAddressService communeService;

        public CommuneCreatePage(IAddressService communeService)
        {
            this.districtService = communeService;
            this.communeService = communeService;
        }
        [Parameter] public int? Id { get; set; } 
        protected List<District> districts = [];
        protected CommuneDtoCreate model = new CommuneDtoCreate();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await districtService.GetAllDistrictsAsync(null);
                if (!results.IsError)
                {

                    districts = results.Value.ToList();
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

                var response = await communeService.CreateCommuneAsync(model);

                if (!response.IsError)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Commune new record was created successful.");
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Warning, response.FirstError.Description);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error,ex.Message);
            }
        }
    }
}