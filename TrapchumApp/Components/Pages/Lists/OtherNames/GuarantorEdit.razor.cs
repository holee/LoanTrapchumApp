using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Entities;
using Applications.Services.Lists;
using Domains.Models.Lists;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.OtherNames
{
    public partial class GuarantorEdit
    {


        private readonly IGuarantorServiceImpl _gService;

        public GuarantorEdit(IGuarantorServiceImpl gService)
        {
            this._gService = gService;
        }

        [Parameter] public Guid? NameId { get; set; }
        IEnumerable<ProvinceDto>? ProvinceList = [];
        IEnumerable<CommuneDto>? CommuneList = [];
        IEnumerable<DistrictDto>? DistrictList = [];
        IEnumerable<VillageDto>? VillageList = [];
        ProvinceDto? SelectedProvince { get; set; }
        DistrictDto? SelectedDistrict { get; set; }
        CommuneDto? SelectedCommune { get; set; }
        VillageDto? SelectedVillage { get; set; }
        EntityNameDtoUpdate model { get; set; } = new();

        protected List<string> salutations = ["Mr.", "Mrs.", "Miss.", "Others"];

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var resp = await _gService.GetGuarantorEditAsync(NameId);

            if (resp.IsSuccess)
            {
                model = resp.Data!;

                ProvinceList = await _addressService.GetsProvinces();

                if (model.ProvinceRefId > 0)
                {
                    DistrictList = await _addressService.GetDistrictsByProvinceId(model.ProvinceRefId);
                }
                if (model.DistrictRefId > 0)
                {
                    CommuneList = await _addressService.GetCommunesByDistrictId(model.DistrictRefId);
                }
                if (model.CommuneRefId > 0)
                {
                    VillageList = await _addressService.GetVillagesByCommuneId(model.CommuneRefId);
                }
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
            }

        }

        protected async Task OnSaveChanges()
        {
            try
            {
                model.EntType = "Other Name";
                var resultResponse = await _gService.UpdateAsync(model);
                if (resultResponse.IsSuccess)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, resultResponse.Data);
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, resultResponse.ErrMessage);
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
        //Salutation implements
        [Parameter] public IEnumerable<string> Salutations { get; set; } = [];
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

        }


        async void GetDistrictByProvince(object e)
        {
            if (e is not null)
            {
                model.ProInKhmer = ((ProvinceDto)e).ProInKhmer;
                var id = ((ProvinceDto)e).ProvinceId;
                DistrictList = await _addressService!.GetDistrictsByProvinceId(id);
            }
            StateHasChanged();
        }
        async void GeCommunesByDistrict(object e)
        {
            if (e is not null)
            {
                model.DisInKhmer = ((DistrictDto)e).DisInKhmer;
                var id = ((DistrictDto)e).DistrictId;
                CommuneList = await _addressService!.GetCommunesByDistrictId(id);
            }
            StateHasChanged();
        }

        async void GetVillagesByCommune(object e)
        {
            if (e is not null)
            {
                model.ComInKhmer = ((CommuneDto)e).ComInKhmer;
                var id = ((CommuneDto)e).CommuneId;
                VillageList = await _addressService!.GetVillagesByCommuneId(id);
            }
            StateHasChanged();
        }
        void GetVillagesName(object e)
        {
            if (e is not null)
            {
                model.VilInKhmer = ((VillageDto)e).VilInKhmer;
            }
            StateHasChanged();
        }

        void SetFirstName()
        {
            model.FirstName = model.Name;
        }
    }
}