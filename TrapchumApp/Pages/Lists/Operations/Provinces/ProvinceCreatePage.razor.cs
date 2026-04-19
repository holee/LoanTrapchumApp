using Applications.Commons.Mappers;
using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;
using Applications.Interfaces.Lists;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Mapster;

namespace TrapchumApp.Pages.Lists.Operations.Provinces
{
    public partial class ProvinceCreatePage
    {
        private readonly IAddressService proService;
        private readonly IAddressService addressService;
        private readonly ILogger<ProvinceCreatePage> logger;
        public ProvinceCreatePage(IAddressService ser, ILogger<ProvinceCreatePage> logger)
        {
            proService = ser;
            this.addressService = ser;
            this.logger = logger;
        }
        protected IEnumerable<Country> Countries = [];
        private ProvinceDtoCreate Req = new ProvinceDtoCreate();
        protected override async Task OnInitializedAsync()
        {
            var results = await addressService.GetCountries(105);
            if (!results.IsError) Countries = results.Value;
            Req.CountryId = 105;
        }
        protected async Task Create()
        {
            try
            {
                var model = Req.MapTo();

                var response = await proService.CreateProvinceAsync(model);

                if (!response.IsError)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Province new record was created successful.");

                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Warning, response.FirstError.Description);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error creating province : {ex.InnerException!.Message}");
            } 
           
        }
    }
}