using Applications.Features.Addresses.Dto;
using Applications.Features.OtherNames;
using Applications.Services.Lists;
using Domains.Models.Bases;
using Domains.Models.Lists;

namespace TrapchumApp.Pages.Lists.OtherNames
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
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);


            }

            await base.OnInitializedAsync();
        }
        protected async Task OnSaveChanges()
        {
            model.Id = $"{Guid.CreateVersion7()}";
            model.EntType = EntType.OtherName.ToString();
            model.CountryName = "Cambodia";
            var rsp = await _otherNameService!.CreateAsync(model);
            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
                model = new();
                await CloseTab();
                return;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }

        }
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