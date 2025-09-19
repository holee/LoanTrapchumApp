using Applications.Commons.Mappers;
using Applications.Features.Addresses.Responses;
using Applications.Interfaces.Addresses;
using Applications.Interfaces.Lists;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Mapster;

namespace TrapchumApp.Pages.Lists.Operations.Provinces
{
    public partial class ProvinceCreatePage
    {
        private readonly IProvinceService proService;
        private readonly IAddressService addressService;
        private readonly ILogger<ProvinceCreatePage> logger;
        public ProvinceCreatePage(IProvinceService ser, IAddressService addressService, ILogger<ProvinceCreatePage> logger)
        {
            proService = ser;
            this.addressService = addressService;
            this.logger = logger;
        }
        protected IEnumerable<Country> Countries = [];
        private ProvinceDtoCreate Req = new ProvinceDtoCreate();
        protected override async Task OnInitializedAsync()
        {
            var results = await addressService.GetCountries(105);
            Countries = results.Data;
            Req.CountryId = 105;
        }
        protected async Task Create()
        {
            try
            {
                var model = Req.MapTo();

                var response = await proService.CreateAsync(model);

                if (response!.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Province new record was created successful.");
                  
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Warning, response.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error creating province : {ex.InnerException!.Message}");
            } 
           
        }
    }
}