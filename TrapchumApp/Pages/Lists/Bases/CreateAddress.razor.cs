using Applications.Requests.Lists.Addreses;
using Domains.Models.Addresses;

namespace TrapchumApp.Pages.Lists.Bases;

public partial class CreateAddress : ComponentBase
{

    private string? _selectedProvinceText = string.Empty;
    private string? _selectedDistricText = string.Empty;
    private string? _selectedCommuneText = string.Empty;
    private string? _selectedVillageText = string.Empty;

    [Parameter] public int? ProvinceRefId { get; set; }
    [Parameter]
    public string? ProInKhmer
    {
        get => _selectedProvinceText;
        set
        {
            _selectedProvinceText = value ?? string.Empty;
        }
    }
    [Parameter] public int? DistrictRefId { get; set; }
    [Parameter]
    public string? DisInKhmer
    {
        get => _selectedDistricText;
        set
        {
            _selectedDistricText = value ?? string.Empty;
        }
    }
    [Parameter] public int? CommuneRefId { get; set; }
    [Parameter]
    public string? ComInKhmer
    {
        get => _selectedCommuneText;
        set
        {
            _selectedCommuneText = value ?? string.Empty;
        }
    }
    [Parameter] public int? VillageRefId { get; set; }
    [Parameter]
    public string? VilInKhmer
    {
        get => _selectedVillageText;
        set
        {
            _selectedVillageText = value ?? string.Empty;
        }
    }
    [Parameter] public string? CountryName { get; set; }
    [Parameter] public string? CityName { get; set; }
    IEnumerable<ProvinceDto> Provinces = [];
    IEnumerable<DistrictDto> Districts = [];
    IEnumerable<CommuneDto> Communes = [];
    IEnumerable<VillageDto> Villages = [];
    IEnumerable<string> Cities { get; set; } = [];
    IEnumerable<Country> Countries { get; set; } = [];
    string? searchText = string.Empty;
    protected IEnumerable<string> filteredItems = [];
    void OnSearchTextChanged(ChangeEventArgs e)
    {
        searchText = e.Value?.ToString() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            filteredItems = Cities; // Show all items if search is empty
        }
        else
        {
            filteredItems = Cities
                .Where(i => i.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
    async Task GetCities(string? filter = null)
    {
        Cities = await _addressService.GetCities(filter);

        filteredItems = Cities;
    }
    protected async Task AddCity()
    {
        var city = new CityDtoCreate { CityName = "" };

        var message = await _addressService.AddCity(city);

        if (message!.IsSuccess)
        {
            await GetCities();
            //StateHasChanged();
        }
    }
    async Task OnKeyPress(KeyboardEventArgs args)
    {
        if (args.Key == "Enter" && !string.IsNullOrWhiteSpace(searchText))
        {
            if (!Cities.Any(i => i.Equals(searchText, StringComparison.OrdinalIgnoreCase)))
            {
                await AddCity();

                searchText = ""; //Clear the textbox
            }
        }
    }

    async void SelectedProvinceChanged(object arg)
    {
        if (arg is not null)
        {
            ProInKhmer = ((ProvinceDto)arg).ProInKhmer;
            Districts = await _addressService!.GetDistrictsByProvinceId(((ProvinceDto)arg).ProvinceId);
            DistrictRefId = null;
            CommuneRefId = null;
            VillageRefId = null;
            Communes = [];
            Villages = [];
            // StateHasChanged();
        }

    }
    async void SelectedDistrictChanged(object arg)
    {
        if (arg is not null)
        {
            DisInKhmer = ((DistrictDto)arg).DisInKhmer;
            Communes = await _addressService!.GetCommunesByDistrictId(((DistrictDto)arg).DistrictId);
            Villages = [];
            CommuneRefId = null;
            VillageRefId = null;
            //StateHasChanged();
        }
    }
    async void SelectedCommuneChanged(object arg)
    {
        if (arg is not null)
        {
            ComInKhmer = ((CommuneDto)arg).ComInKhmer;
            Villages = await _addressService!.GetVillagesByCommuneId(((CommuneDto)arg).CommuneId);
            VillageRefId = null;
            //StateHasChanged();
        }

    }
    void SelectedVillageChanged(object arg)
    {
        if (arg is not null)
        {
            VilInKhmer = ((VillageDto)arg).VilInKhmer;
            //StateHasChanged();
        }

    }

    async Task GetAdditionalInformation()
    {
        Provinces = await _addressService.GetsProvinces();

        await GetCities(searchText);

        if (!Countries.Any())
        {
            var rsp = await _addressService.GetCountries(105);

            if (rsp.IsSuccess)
            {
                Countries = rsp.Data ?? [];
            }
        }


        if (ProvinceRefId.HasValue)
        {
            Districts = await _addressService.GetDistrictsByProvinceId(ProvinceRefId);
        }

        if (DistrictRefId.HasValue)
        {
            Communes = await _addressService.GetCommunesByDistrictId(DistrictRefId);
        }

        if (CommuneRefId.HasValue)
        {
            Villages = await _addressService.GetVillagesByCommuneId(CommuneRefId);
        }

    }

    protected override async Task OnInitializedAsync()
    {
        await GetAdditionalInformation();
        await base.OnInitializedAsync();
    }
}