using Applications.Commons.Mappers;
using Applications.Interfaces.Addresses;
using Applications.Requests.Lists.Addreses;
using BitMiracle.LibTiff.Classic;
using Domains.Models.Addresses;
using Mapster;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Lists.Operations.Districts
{
    public partial class DistrictCreatePage
    {
        private readonly IDistrictService districtService;

        private readonly IAddressService addressService;

        [Parameter] public int? ProvinceCode { get; set; } = 0;

        public DistrictCreatePage(IDistrictService districtService, IAddressService addressService)
        {
            this.districtService = districtService;

            this.addressService = addressService;
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