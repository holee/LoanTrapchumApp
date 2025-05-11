using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.OtherNames;
using Domains.Models.Addresses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TrapchumApp.Components.Pages.Lists.Customers
{
    public partial class CustomerAddress
    {
        [Parameter] public EntityNamesCreate model { get; set; } = new();
        [Parameter] public IEnumerable<string> Cities { get; set; } = [];
        [Parameter] public IEnumerable<Country> Countries { get; set; } = [];
        string city = "";
        IEnumerable<ProvinceDto> Provinces = [];
        IEnumerable<DistrictDto> Districts = [];
        IEnumerable<CommuneDto> Communes = [];
        IEnumerable<VillageDto> Villages = [];

        async void SelectedProvinceChanged(object arg)
        {
            if (arg is not null)
            {
                model.ProInKhmer = ((ProvinceDto)arg).ProInKhmer;
                Districts = await _addressService!.GetDistrictsByProvinceId(((ProvinceDto)arg).ProvinceId);
                StateHasChanged();
            }

        }
        async void SelectedDistrictChanged(object arg)
        {
            if (arg is not null)
            {
                model.DisInKhmer = ((DistrictDto)arg).DisInKhmer;
                Communes = await _addressService!.GetCommunesByDistrictId(((DistrictDto)arg).DistrictId);
                StateHasChanged();
            }
        }
        async void SelectedCommuneChanged(object arg)
        {
            if (arg is not null)
            {
                model.ComInKhmer = ((CommuneDto)arg).ComInKhmer;
                Villages = await _addressService!.GetVillagesByCommuneId(((CommuneDto)arg).CommuneId);
                StateHasChanged();
            }

        }
        void SelectedVillageChanged(object arg)
        {
            if (arg is not null)
            {
                model.VilInKhmer = ((VillageDto)arg).VilInKhmer;
                StateHasChanged();
            }

        }


        async Task OnKeyPress(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                await _addressService.AddCity(new CityDtoCreate
                {
                    CityName = city
                });
                Cities = await _addressService.GetCities();
            }

        }

        async Task GetAdditionalInformation()
        {
            Provinces = await _addressService.GetsProvinces();
            if (model is not null)
            {
                if (model!.ProvinceRefId!.HasValue)
                {
                    Districts = await _addressService.GetDistrictsByProvinceId(model?.ProvinceRefId);
                }

                if (model!.DistrictRefId.HasValue)
                {
                    Communes = await _addressService.GetCommunesByDistrictId(model?.DistrictRefId);
                }

                if (model!.CommuneRefId.HasValue)
                {
                    Villages = await _addressService.GetVillagesByCommuneId(model?.CommuneRefId);
                }
            }
        }



        protected override async Task OnInitializedAsync()
        {
            await GetAdditionalInformation();

            await base.OnInitializedAsync();
        }
    }
}