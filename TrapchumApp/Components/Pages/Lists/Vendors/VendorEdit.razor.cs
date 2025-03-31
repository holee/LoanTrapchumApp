using Applications.Enumerations;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.OtherNames;
using Applications.Services.LIsts;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.Vendors
{
    public partial class VendorEdit
    {
        private readonly IVendorServiceImpl _vendorService;

        public VendorEdit(IVendorServiceImpl vendorService)
        {
            _vendorService = vendorService;
        }

        [Parameter] public EventCallback<int> SelectIndex { get; set; }
        [Parameter] public Guid? Id { get; set; }
        [Parameter] public IEnumerable<ProvinceDto> ProvinceList { get; set; } = [];
        EntNameDtoUpdate model = new();
        IEnumerable<CommuneDto>? CommuneList = [];
        IEnumerable<DistrictDto>? DistrictList = [];
        IEnumerable<VillageDto>? VillageList = [];
        IEnumerable<string> Salutations = [];
        ProvinceDto? SelectedProvince;
        DistrictDto? SelectedDistrict;
        CommuneDto? SelectedCommune;
        VillageDto? SelectedVillage;
        protected List<string> salutations = ["Mr.", "Mrs.", "Miss.", "Others"];
        protected override async Task OnInitializedAsync()
        {
            await GetSalutations();
            await ShowEditVendor(Id);
        }

        protected override async Task OnParametersSetAsync()
        {

            await base.OnParametersSetAsync();
        }

        protected async Task SelectedIndex()
        {
            await SelectIndex.InvokeAsync(0);
        }

        protected async Task HandleFormSubmit()
        {
            model!.EntType = EntityTypes.Vendor.ToString();
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

            var rsp = await _vendorService!.UpdateAsync(model);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Success, "Vendor", rsp.Data);

                NavigationManager.NavigateTo($"/trapchum/vendors/");

                return;
            }
            else
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Vendor", rsp.ErrMessage);
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
        protected async Task ShowEditVendor(Guid? id)
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
                    if (model!.ProvinceRefId.HasValue)
                    {
                        SelectedProvince = ProvinceList!.FirstOrDefault(d => d.ProvinceId == model?.ProvinceRefId);
                    }
                    if (model.DistrictRefId.HasValue)
                    {
                        this.DistrictList = await _addressService!.GetDistrictsByProvinceId(model?.ProvinceRefId);
                        SelectedDistrict = DistrictList.FirstOrDefault(d => d.DistrictId == model!.DistrictRefId);

                    }
                    if (model!.CommuneRefId.HasValue)
                    {
                        this.CommuneList = await _addressService!.GetCommunesByDistrictId(model?.DistrictRefId);
                        SelectedCommune = this.CommuneList.FirstOrDefault(c => c.CommuneId == model!.CommuneRefId);
                    }
                    if (model!.VillageRefId.HasValue)
                    {
                        this.VillageList = await _addressService!.GetVillagesByCommuneId(model?.CommuneRefId);
                        SelectedVillage = this.VillageList.FirstOrDefault(x => x.VillageId == model!.VillageRefId);
                    }

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }


        }




        async void ShowEditVendor()
        {

            if (model is null)
            {
                return;
            }
            if (model!.ProvinceRefId.HasValue)
            {
                SelectedProvince = ProvinceList!.FirstOrDefault(d => d.ProvinceId == model?.ProvinceRefId);
            }
            else
            {
                SelectedProvince = new();
            }
            if (model.DistrictRefId.HasValue)
            {
                this.DistrictList = await _addressService!.GetDistrictsByProvinceId(model?.ProvinceRefId);
                SelectedDistrict = DistrictList.FirstOrDefault(d => d.DistrictId == model!.DistrictRefId);
            }
            else
            {
                SelectedDistrict = new();
            }
            if (model!.CommuneRefId.HasValue)
            {
                this.CommuneList = await _addressService!.GetCommunesByDistrictId(model?.DistrictRefId);
                SelectedCommune = this.CommuneList.FirstOrDefault(c => c.CommuneId == model!.CommuneRefId);
            }
            else
            {
                SelectedCommune = new();
            }
            if (model!.VillageRefId.HasValue)
            {
                this.VillageList = await _addressService!.GetVillagesByCommuneId(model?.CommuneRefId);
                SelectedVillage = this.VillageList.FirstOrDefault(x => x.VillageId == model!.VillageRefId);
            }
            else
            {
                SelectedVillage = new();
            }


        }



        // async Task GetAddresses()
        // {
        //     try
        //     {
        //         if (clicked)
        //         {
        //             ProvinceList = await AddressService!.GetAllProvinceAsync();
        //             Countries = await AddressService.GetCountries(105);
        //             strCountries = Countries?.Select(x => x.DesInKhmer).ToList()!;
        //             Cities = await AddressService.GetCities();
        //             if (model is not null)
        //             {

        //                 if (model.ProvinceRefId.HasValue)
        //                 {
        //                     SelectedProvince = ProvinceList!.FirstOrDefault(d => d.ProvinceID == model?.ProvinceRefId);
        //                 }
        //                 if (model.DistrictRefId.HasValue)
        //                 {
        //                     this.DistrictList = await AddressService.GetDistrictByProvinceAsync(model?.ProvinceRefId);
        //                     SelectedDistrict = DistrictList.FirstOrDefault(d => d.DistrictId == model?.DistrictRefId);

        //                 }
        //                 if (model!.DistrictRefId.HasValue)
        //                 {
        //                     this.CommuneList = await AddressService.GetCommuneByDistricteAsync(model?.DistrictRefId);
        //                     SelectedCommune = this.CommuneList.FirstOrDefault(c => c.CommuneId == model?.CommuneRefId);
        //                 }
        //                 if (model!.DistrictRefId.HasValue)
        //                 {
        //                     this.VillageList = await AddressService.GetVillageByCommuneAsync(model?.CommuneRefId);

        //                     SelectedVillage = this.VillageList.FirstOrDefault(x => x.VillageID == model?.VillageRefId);

        //                 }

        //             }
        //             clicked = false;
        //             StateHasChanged();

        //         }

        //     }
        //     catch (Exception ex)
        //     {
        //         throw;
        //     }


        // }



    }
}