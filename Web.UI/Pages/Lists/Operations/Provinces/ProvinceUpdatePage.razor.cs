
using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;
using Applications.Responses.Message;
using Domains.Models.Addresses;
using Mapster;

namespace Web.UI.Pages.Lists.Operations.Provinces
{
    public partial class ProvinceUpdatePage
    {
        private readonly IAddressService proService;
        private readonly IAddressService addressService;

        public ProvinceUpdatePage(IAddressService ser)
        {
            proService = ser;
            this.addressService = ser;
        }
        protected IEnumerable<Country> Countries = [];
        private ProvinceDtoUpdate Req = new ProvinceDtoUpdate();
        [Parameter] public int Id { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await addressService.GetCountries(105);
                if (!results.IsError) Countries = results.Value;

                var response = await proService.GetProvinceByIdAsync(Id);

                if (!response.IsError)
                {
                    Req = response.Value.Adapt<ProvinceDtoUpdate>();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Province", response.FirstError.Description);
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Province", ex.Message);

            }
            await base.OnInitializedAsync();
        }
        protected async Task Update()
        {
            try
            {
                var req = Req.Adapt<Province>();
                //Console.WriteLine(req);
                var response = await proService.UpdateProvinceAsync(req);
                if (!response.IsError)
                {
                    NotificationService.Notify(NotificationSeverity.Success,"Provice Updated", Messages.DataCreated);

                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, response.FirstError.Description);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }

        }
    }
}