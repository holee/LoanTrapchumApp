
using Applications.Interfaces.Addresses;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Message;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Mapster;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Lists.Operations.Provinces
{
    public partial class ProvinceUpdatePage
    {
        private readonly IProvinceService proService;
        private readonly IAddressService addressService;

        public ProvinceUpdatePage(IProvinceService ser, IAddressService addressService)
        {
            proService = ser;
            this.addressService = addressService;
        }
        protected IEnumerable<Country> Countries = [];
        private ProvinceDtoUpdate Req = new ProvinceDtoUpdate();
        [Parameter] public int Id { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await addressService.GetCountries(105);
                Countries = results.Data;

                var response = await proService.GetById(Id);

                if (response.IsSuccess)
                {
                    Req = response.Data!.Adapt<ProvinceDtoUpdate>();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Province", response.ErrMessage);
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
                var response = await proService.UpdateAsync(req);
                if (response!.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success,"Provice Updated", Messages.DataCreated);

                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }

        }
    }
}