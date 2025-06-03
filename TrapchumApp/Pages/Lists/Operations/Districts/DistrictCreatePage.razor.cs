using Applications.Interfaces.Addresses;
using Applications.Requests.Lists.Addreses;
using BitMiracle.LibTiff.Classic;
using Domains.Models.Addresses;
using Mapster;

namespace TrapchumApp.Pages.Lists.Operations.Districts
{
    public partial class DistrictCreatePage
    {
        private readonly IDistrictService districtService;
        private readonly IAddressService addressService;

        public DistrictCreatePage(IDistrictService districtService,IAddressService addressService)
        {
            this.districtService = districtService;
            this.addressService = addressService;
        }
        protected List<ProvinceDto> provinces = [];
        protected DistrictDtoCreate model= new DistrictDtoCreate();
        protected override async Task OnInitializedAsync()
        {
            var results = await addressService.GetsProvinces(null);
            if (results.Count()>0)
            {

                provinces = results.ToList();
            }
            await base.OnInitializedAsync();
        }
        protected async Task Create()
        {
            try
            {
                var data = model.Adapt<District>();

                var response = await districtService.CreateAsync(data);

                if (response!.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "District new record was created successful.");
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