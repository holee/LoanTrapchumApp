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
        private readonly IAddressService districtService;
        private readonly IAddressService addressService;

        public DistrictUpdatePage(IAddressService districtService)
        {
            this.districtService = districtService;
            this.addressService = districtService;
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

                var response = await districtService.GetDistrictByIdAsync(Id);

                if (!response.IsError)
                {
                    model = response.Value.Adapt<DistrictDtoUpdate>();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "District", response.FirstError.Description);
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
                var response = await districtService.UpdateDistrictAsync(req);
                if (!response.IsError)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "District Updated", Messages.DataCreated);

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