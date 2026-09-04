using Applications.Features.Addresses.Dto;
using Applications.Features.OtherNames.Dto;
using Domains.Models.Addresses;

namespace Web.UI.Pages.Lists.Customers;

public partial class CustomerAddress
{
    [Parameter] public BaseEntityNamesCreate model { get; set; }
    string city = "";
    private IEnumerable<ProvinceDto> Provinces = [];
    private IEnumerable<DistrictDto> Districts = [];
    private IEnumerable<CommuneDto> Communes = [];
    private IEnumerable<VillageDto> Villages = [];
    private IEnumerable<string> Cities { get; set; } = [];
    private IEnumerable<Country> Countries { get; set; } = [];

    private async Task GetCountries()
    {
        var rsp = await _addressService.GetCountries(105);

        if (!rsp.IsError)
        {
            Countries = rsp.Value;
        }
    }
    async void SelectedProvinceChanged(object arg)
    {
        if (arg is not null)
        {
            model.ProInKhmer = ((ProvinceDto)arg).ProInKhmer;
            Districts = await _addressService!.GetDistrictsByProvinceId(((ProvinceDto)arg).ProvinceId);
            Communes = [];
            Villages = [];
            StateHasChanged();
        }

    }
    async void SelectedDistrictChanged(object arg)
    {
        if (arg is not null)
        {
            model.DisInKhmer = ((DistrictDto)arg).DisInKhmer;
            Communes = await _addressService!.GetCommunesByDistrictId(((DistrictDto)arg).DistrictId);
            Villages = [];
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
        await GetCities();

        await GetProvinces();

        await GetCountries();


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

    private async Task GetCities()
    {
        Cities = await _addressService.GetCities();
    }

    private async Task GetProvinces()
    {
        Provinces = await _addressService.GetsProvinces();
    }

    protected override async Task OnInitializedAsync()
    {
        await GetAdditionalInformation();

        await base.OnInitializedAsync();
    }
}