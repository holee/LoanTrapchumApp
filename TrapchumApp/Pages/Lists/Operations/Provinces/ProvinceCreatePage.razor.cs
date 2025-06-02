using Applications.Interfaces.Addresses;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Entities;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Mapster;

namespace TrapchumApp.Pages.Lists.Operations.Provinces
{
    public partial class ProvinceCreatePage
    {
        private readonly IProvinceService proService;
        private readonly IAddressService addressService;

        public ProvinceCreatePage(IProvinceService ser,IAddressService addressService)
        {
            proService = ser;
            this.addressService = addressService;
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
                var model = Req.Adapt<Province>();

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
                throw new Exception();
            } 
           
        }
    }
}