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
        private readonly IProvinceService provinceService;

        public DistrictCreatePage(IDistrictService districtService,IProvinceService provinceService)
        {
            this.districtService = districtService;
            this.provinceService = provinceService;
        }
        protected List<Province> provinces = [];
        protected DistrictDtoCreate model= new DistrictDtoCreate();
        protected override async Task OnInitializedAsync()
        {
            var results= await provinceService.GetAllAsync(0,0,null);
            if (results.IsSuccess)
            {

                provinces = results.Data.ToList();
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