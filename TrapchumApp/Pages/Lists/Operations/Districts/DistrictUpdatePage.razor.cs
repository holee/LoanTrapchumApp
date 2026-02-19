using Applications.Interfaces.Addresses;
using Applications.Responses.Message;
using Domains.Models.Addresses;
using Mapster;
using Microsoft.AspNetCore.Components;
using System.Net.Sockets;
using Applications.Features.Addresses.Dto;

namespace TrapchumApp.Pages.Lists.Operations.Districts
{
    public partial class DistrictUpdatePage
    {
        private readonly IDistrictService districtService;
        private readonly IAddressService addressService;

        public DistrictUpdatePage(IDistrictService districtService, IAddressService addressService)
        {
            this.districtService = districtService;
            this.addressService = addressService;
        }
        protected List<ProvinceDto> provinces = [];
        protected DistrictDtoUpdate model = new DistrictDtoUpdate();

        [Parameter] public int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await addressService.GetsProvinces(null);
                if (results.Count() > 0)
                {

                    provinces = results.ToList();
                }

                var response = await districtService.GetByIdAsync(Id);

                if (response.IsSuccess)
                {
                    model = response.Data!.Adapt<DistrictDtoUpdate>();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "District", response.ErrMessage);
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "District", ex.Message);

            }
            await base.OnInitializedAsync();
        }
        protected async Task Update()
        {
            try
            {
                var req = model.Adapt<District>();
                //Console.WriteLine(req);
                var response = await districtService.UpdateAsync(req);
                if (response!.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "District Updated", Messages.DataCreated);

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