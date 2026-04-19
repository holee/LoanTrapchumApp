using Applications.Commons.Mappers;
using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;

namespace TrapchumApp.Pages.Lists.Operations.Districts
{
    public partial class DistrictCreatePage
    {
        private readonly IAddressService districtService;

        private readonly IAddressService addressService;

        [Parameter] public int? ProvinceCode { get; set; } = 0;

        public DistrictCreatePage(IAddressService districtService)
        {
            this.districtService = districtService;

            this.addressService = districtService;
        }
        protected List<ProvinceDto> provinces = [];

        protected DistrictDtoCreate model = new DistrictDtoCreate();
        protected override async Task OnInitializedAsync()
        {
            if (ProvinceCode > 0)
            {
                await GetProvinces(ProvinceCode, string.Empty);

                model.ProvinceRefId = ProvinceCode;
            }
            else
            {
                await GetProvinces(0, string.Empty);
            }


            await base.OnInitializedAsync();
        }

        private async Task GetProvinces(int? provinceCode, string? filter = null)
        {
            var results = await addressService.GetsProvinces(provinceCode,filter);

            if (results.Count() > 0)
            {
                provinces = results.ToList();
            }
        }

        protected async Task Create()
        {
            try
            {
                var data = model.MapCreate();

                var response = await districtService.CreateDistrictAsync(data);

                if (!response.IsError)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "District new record was created successful.");
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Warning, response.FirstError.Description);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }
    }
}