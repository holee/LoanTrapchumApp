using Applications.Features.Addresses.Dto;
using Applications.Features.OtherNames.Dto;
using Applications.Responses.Lists.Entities;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Lists;

namespace TrapchumApp.Pages.Lists.Vendors
{
    public partial class VendorEditPage(IVendorServiceImpl vendorService,
        INationalityServiceImpl nationalityService,
        ISalutationServiceImpl salutationService)
    {
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
        bool _isLoading = false;
        IEnumerable<OcupationResponse> Occupations = [];
        List<string> salutations = ["Mr.", "Mrs.", "Miss.", "Others"];
        string city = "";
        protected override async Task OnInitializedAsync()
        {

            await ShowEditVendor(Id);

            await base.OnInitializedAsync();
        }
        protected async Task SelectedIndex()
        {
            await SelectIndex.InvokeAsync(0);
        }

        protected async Task HandleFormSubmit()
        {
            var rsp = await vendorService.UpdateAsync(model);

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
            Nationalities = await nationalityService.GetNationalities();

            Provinces = await _addressService.GetsProvinces();

            //Occupations = await _employeeService!.GetOccupationsAsync(string.Empty);

            var rsp = await _addressService.GetCountries(105);

            if (!rsp.IsError)
            {
                Countries = rsp.Value;
            }

            Cities = await _addressService.GetCities();
        }




        protected string? newSalution = "";

        protected async Task AddNewSalutation(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                await salutationService.AddSalution(new Salutation
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
            var salutations = await salutationService.GetSalutions();

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
                _isLoading = true;

                var resp = await vendorService!.GetEditVendorAsync(id);

                if (!resp.IsSuccess)
                {
                    model = new();
                    return;
                }
                model = resp.Data!;

                _isLoading = false;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);

                _isLoading = false;
            }
            finally
            {
                _isLoading = false;
            }

            StateHasChanged();
        }

        protected async Task Close()
        {
            await Closed.InvokeAsync(false);
        }
    }
}



//if (model is not null)
//{
//    if (model.DistrictRefId.HasValue)
//    {
//        Districts = await _addressService!.GetDistrictsByProvinceId(model?.ProvinceRefId);
//    }
//    if (model!.CommuneRefId.HasValue)
//    {
//        Communes = await _addressService!.GetCommunesByDistrictId(model?.DistrictRefId);
//    }
//    if (model!.VillageRefId.HasValue)
//    {
//        Villages = await _addressService!.GetVillagesByCommuneId(model?.CommuneRefId);
//    }

//}

//async void SelectedProvinceChanged(object arg)
//{
//    if (arg is not null)
//    {
//        model.ProInKhmer = ((ProvinceDto)arg).ProInKhmer;

//        Districts = await _addressService!.GetDistrictsByProvinceId(((ProvinceDto)arg).ProvinceId);

//        StateHasChanged();
//    }

//}
//async void SelectedDistrictChanged(object arg)
//{
//    if (arg is not null)
//    {
//        model.DisInKhmer = ((DistrictDto)arg).DisInKhmer;

//        Communes = await _addressService!.GetCommunesByDistrictId(((DistrictDto)arg).DistrictId);

//        StateHasChanged();
//    }

//}
//async void SelectedCommuneChanged(object arg)
//{
//    if (arg is not null)
//    {
//        model.ComInKhmer = ((CommuneDto)arg).ComInKhmer;

//        Villages = await _addressService!.GetVillagesByCommuneId(((CommuneDto)arg).CommuneId);

//        StateHasChanged();
//    }

//}
//void SelectedVillageChanged(object arg)
//{
//    if (arg is not null)
//    {
//        model.VilInKhmer = ((VillageDto)arg).VilInKhmer;
//        StateHasChanged();
//    }

//}
