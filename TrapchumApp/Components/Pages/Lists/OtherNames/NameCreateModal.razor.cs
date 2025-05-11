using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.OtherNames;
using Applications.Services.Lists;
using Domains.Models.Bases;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TrapchumApp.Components.Pages.Lists.OtherNames
{
    public partial class NameCreateModal
    {
        private readonly IOtherNameServiceImpl _otherNameService;
        private readonly ISalutationServiceImpl _salutationService;
        public NameCreateModal(IOtherNameServiceImpl otherNameService, ISalutationServiceImpl salutationService)
        {
            _otherNameService = otherNameService;
            _salutationService = salutationService;
        }

        private EntityNamesCreate model = new EntityNamesCreate();
        [Parameter] public IEnumerable<ProvinceDto>? ProvinceList { get; set; }
        [Parameter] public IEnumerable<CommuneDto>? CommuneList { get; set; }
        [Parameter] public IEnumerable<DistrictDto>? DistrictList { get; set; }
        [Parameter] public IEnumerable<VillageDto>? VillageList { get; set; }
        private bool isValid = true;
        protected List<string> salutations = ["Mr.", "Mrs.", "Miss.", "Others"];
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ProvinceList = await _addressService!.GetsProvinces();

                model.Gender = "Mr";
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.Message);


            }

            await base.OnInitializedAsync();
        }
        protected async Task OnSaveChanges()
        {
            try
            {
                model.Id = $"{Guid.CreateVersion7()}";
                model.EntType = EntType.OtherName.ToString();
                model.CountryName = "Cambodia";
                var rsp = await _otherNameService!.CreateAsync(model);
                if (rsp.IsSuccess)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, rsp.Data);
                    model = new();
                    return;
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, rsp.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.Message);
            }

        }

        async void SelectProvinceChanged(object e)
        {

            var province = e as ProvinceDto;

            if (province is not null)
            {
                model.ProInKhmer = province.ProInKhmer;
                DistrictList = await _addressService!.GetDistrictsByProvinceId(province.ProvinceId);
            }
            else
            {
                DistrictList = [];
            }
            StateHasChanged();

        }
        async Task SelectDistrictChanged(object e)
        {
            var district = e as DistrictDto;

            if (district is not null)
            {
                model.DisInKhmer = district.DisInKhmer;
                CommuneList = await _addressService!.GetCommunesByDistrictId(district.DistrictId);
            }
            else
            {
                CommuneList = [];
            }
            StateHasChanged();
        }
        async Task SelectCommuneChanged(object e)
        {

            var commune = e as CommuneDto;

            if (commune is not null)
            {
                model.ComInKhmer = commune.ComInKhmer;
                VillageList = await _addressService!.GetVillagesByCommuneId(commune.CommuneId);
            }
            else
            {
                VillageList = [];
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
    }
}