using Applications.Features.Addresses.Dto;
using Applications.Features.OtherNames.Dto;
using Domains.Models.Addresses;
namespace Web.UI.Pages.Lists
{
    public partial class AddressCreate 
    {
        [Parameter] public BaseEntityNamesCreate model { get; set; }
        [Parameter] public IEnumerable<string> Cities { get; set; } = [];
        [Parameter] public IEnumerable<Country> Countries { get; set; } = [];
        string city = "";
        IEnumerable<ProvinceDto> Provinces = [];
        IEnumerable<DistrictDto> Districts = [];
        IEnumerable<CommuneDto> Communes = [];
        IEnumerable<VillageDto> Villages = [];
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
            var city = new CityDtoCreate { CityName = model.City! };

            var message = await _addressService!.AddCity(city);

            if (!message.IsError)
            {
                await GetCities();
                StateHasChanged();
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
                model.ProInKhmer = ((ProvinceDto)arg).ProInKhmer;
                Districts = await _addressService!.GetDistrictsByProvinceId(((ProvinceDto)arg).ProvinceId);
                model.DistrictRefId = null;
                model.CommuneRefId = null;
                model.VillageRefId = null;
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
                model.CommuneRefId = null;
                model.VillageRefId = null;
                StateHasChanged();
            }
        }
        async void SelectedCommuneChanged(object arg)
        {
            if (arg is not null)
            {
                model.ComInKhmer = ((CommuneDto)arg).ComInKhmer;
                Villages = await _addressService!.GetVillagesByCommuneId(((CommuneDto)arg).CommuneId);
                model.VillageRefId = null;
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

        async Task GetAdditionalInformation()
        {
            Provinces = await _addressService.GetsProvinces();

            await GetCities(searchText);

            if (!Countries.Any())
            {
                var rsp = await _addressService.GetCountries(105);

                if (!rsp.IsError)
                {
                    Countries = rsp.Value;
                }
            }

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