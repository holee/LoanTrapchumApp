using Applications.Enumerations;
using Applications.Interfaces.Vendors;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.OtherNames;
using Applications.Services.LIsts;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.Vendors
{
    public partial class VendorCreate
    {

        private readonly IVendorServiceImpl _vendorService;

        public VendorCreate(IVendorServiceImpl vendorService)
        {
            _vendorService = vendorService;
        }

        [Parameter] public EventCallback<int> SelectIndex { get; set; }
        [Parameter] public IEnumerable<ProvinceDto>? ProvinceList { get; set; }
        EntNameDtoCreate model = new EntNameDtoCreate();
        IEnumerable<CommuneDto>? CommuneList = [];
        IEnumerable<DistrictDto>? DistrictList = [];
        IEnumerable<VillageDto>? VillageList = [];
        ProvinceDto? SelectedProvince { get; set; }
        DistrictDto? SelectedDistrict { get; set; }
        CommuneDto? SelectedCommune { get; set; }
        VillageDto? SelectedVillage { get; set; }
        IEnumerable<string> Salutations = [];
        private bool isValid = true;
        //protected List<string> salutations = ["Mr.", "Mrs.", "Miss.", "Others"];

        protected override async Task OnInitializedAsync()
        {
            try
            {
                model.Sex = "Mr";
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.Message);


            }

            await base.OnInitializedAsync();
        }
        protected async void SelectedIndex()
        {
            await SelectIndex.InvokeAsync(0);
        }

        void SetFirstName()
        {
            model.FirstName = model.Name;
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

        }

        protected async Task HandleFormSubmit()
        {
            try
            {
                model.EntType = EntityTypes.Vendor.ToString();
                model.CountryName = "Cambodia";
                model.City = SelectedProvince?.ProDescription;
                model.ProvinceRefId = SelectedProvince?.ProvinceId;
                model.ProDescription = SelectedProvince?.ProDescription;
                model.ProInKhmer = SelectedProvince?.ProInKhmer;
                model.DistrictRefId = SelectedDistrict?.DistrictId;
                model.DisDescription = SelectedDistrict?.DisDescription;
                model.DisInKhmer = SelectedDistrict?.DisInKhmer;
                model.CommuneRefId = SelectedCommune?.CommuneId;
                model.ComDescription = SelectedCommune?.ComDescription;
                model.ComInKhmer = SelectedCommune?.ComInKhmer;
                model.VillageRefId = SelectedVillage?.VillageId;
                model.VillDescription = SelectedVillage?.VillDescription;
                model.VilInKhmer = SelectedVillage?.VilInKhmer;
                var message = await _vendorService!.CreateAsync(model);
                if (message.IsSuccess)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, "Other Name", "Other Name created successfully!");
                    model = new();
                    return;
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, "Other Name", "Other Name create failed");

                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.Message);
            }

        }

        async void SelectProvince(ProvinceDto e)
        {
            SelectedProvince = e;
            if (SelectedProvince is not null)
            {
                DistrictList = await _addressService!.GetDistrictsByProvinceId(SelectedProvince.ProvinceId);
            }
            else
            {
                SelectedProvince = null;
                DistrictList = null;
            }
            StateHasChanged();

        }
        async Task SelectDistrict(DistrictDto e)
        {
            if (e is not null)
            {
                SelectedDistrict = e;
                CommuneList = await _addressService!.GetCommunesByDistrictId(SelectedDistrict.DistrictId);
            }
            else
            {
                SelectedDistrict = null;
                CommuneList = null;
            }
            StateHasChanged();
        }
        async Task SelectCommune(CommuneDto e)
        {
            if (e is not null)
            {
                SelectedCommune = e;
                VillageList = await _addressService!.GetVillagesByCommuneId(SelectedCommune.CommuneId);
            }
            else
            {
                SelectedCommune = null;
                VillageList = null;
            }
            StateHasChanged();
        }
        void SelectVillage(VillageDto e)
        {
            if (e is not null)
            {
                SelectedVillage = e;

            }
            else
            {
                SelectedVillage = null;
            }
            StateHasChanged();
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


    }
}