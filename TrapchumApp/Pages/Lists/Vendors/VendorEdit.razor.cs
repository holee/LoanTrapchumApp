using Applications.Features.Addresses.Requests;
using Applications.Features.Addresses.Responses;
using Applications.Features.OtherNames;
using Applications.Responses.Lists.Entities;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Pages.Lists.Vendors
{
    public partial class VendorEdit
    {
        private readonly IVendorServiceImpl _vendorService;
        private readonly INationalityServiceImpl _nationalityService;
        private readonly IOccupationServiceImpl _occupationService;
        private readonly ISalutationServiceImpl _salutationService;
        public VendorEdit(IVendorServiceImpl vendorService, INationalityServiceImpl nationalityService, IOccupationServiceImpl occupationService, ISalutationServiceImpl salutationService)
        {
            _vendorService = vendorService;
            _nationalityService = nationalityService;
            _occupationService = occupationService;
            _salutationService = salutationService;
        }

        [Parameter] public EventCallback<bool> Closed { get; set; }
        [Parameter] public EventCallback<int> SelectIndex { get; set; }
        [Parameter] public string? Id { get; set; }
        [Parameter] public IEnumerable<ProvinceDto> ProvinceList { get; set; } = [];
        EntityVendorCreate model = new();
        IEnumerable<Nationality> Nationalities = [];
        IEnumerable<string> Salutations = [];
        IEnumerable<Country> Countries = [];
        IEnumerable<string> Cities = [];
        IEnumerable<ProvinceDto> Provinces = [];
        IEnumerable<DistrictDto> Districts = [];
        IEnumerable<CommuneDto> Communes = [];
        IEnumerable<VillageDto> Villages = [];
        IEnumerable<string> Statuses = ["Single", "Married", "Divorced", "Widow"];
        IEnumerable<OcupationResponse> Occupations = [];
        List<string> salutations = ["Mr.", "Mrs.", "Miss.", "Others"];
        string city = "";


        protected override async Task OnInitializedAsync()
        {
            await GetAdditionalInformation();
            await GetSalutations();
            await ShowEditVendor(Id);
            await base.OnInitializedAsync();
        }
        protected async Task SelectedIndex()
        {
            await SelectIndex.InvokeAsync(0);
        }

        protected async Task HandleFormSubmit()
        {
            var rsp = await _vendorService.UpdateAsync(model);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Vendor", rsp.Data);
                await Close();
                return;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, "Vendor", rsp.ErrMessage);
            }

        }

        async Task GetAdditionalInformation()
        {
            Nationalities = await _nationalityService.GetNationalities();

            Provinces = await _addressService.GetsProvinces();

            Occupations = await _employeeService!.GetOccupationsAsync(string.Empty);

            var rsp = await _addressService.GetCountries(105);

            if (rsp.IsSuccess)
            {
                Countries = rsp.Data ?? [];
            }

            Cities = await _addressService.GetCities();
        }

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



        protected string? newSalution = "";

        protected async Task AddNewSalutation(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                await _salutationService.AddSalution(new Salutation
                {
                    Title = newSalution
                });
                await GetSalutations();
            }
        }
        protected void OnTextChangedSalution(ChangeEventArgs e)
        {
            newSalution = e?.Value?.ToString();
        }
        private async Task GetSalutations()
        {
            var salutations = await _salutationService.GetSalutions();

            if (salutations.IsSuccess)
            {
                Salutations = salutations!.Data!.Select(x => x.Title!).ToList();
            }
            else
            {
                Salutations = [];
            }
            ProvinceList = await _addressService!.GetsProvinces();
        }

        void SetFirstName()
        {
            model!.FirstName = model.Name;
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
        protected async Task ShowEditVendor(string? id)
        {
            try
            {
                var resp = await _vendorService!.GetEditVendorAsync(id);

                if (!resp.IsSuccess)
                {
                    model = new();
                    return;
                }
                model = resp.Data!;
                if (model is not null)
                {
                    if (model.DistrictRefId.HasValue)
                    {
                        Districts = await _addressService!.GetDistrictsByProvinceId(model?.ProvinceRefId);
                    }
                    if (model!.CommuneRefId.HasValue)
                    {
                        Communes = await _addressService!.GetCommunesByDistrictId(model?.DistrictRefId);
                    }
                    if (model!.VillageRefId.HasValue)
                    {
                        Villages = await _addressService!.GetVillagesByCommuneId(model?.CommuneRefId);
                    }

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }


        }

        protected async Task Close()
        {
            await Closed.InvokeAsync(false);
        }
    }
}