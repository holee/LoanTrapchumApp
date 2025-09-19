using Applications.Features.OtherNames;
using Applications.Responses.Lists.Entities;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Lists;

namespace TrapchumApp.Pages.Lists.Vendors
{
    public partial class CreateComp
    {

        private readonly INationalityServiceImpl _nationalityService;
        private readonly IOccupationServiceImpl _occupationService;
        private readonly ISalutationServiceImpl _salutationService;
        [Parameter] public EntityVendorCreate model { get; set; } = new();

        IEnumerable<OcupationResponse> Occupations = [];
        IEnumerable<Nationality> Nationalities { get; set; } = [];
        IEnumerable<string> Statuses = ["Single", "Married", "Divorced", "Widow"];
        IEnumerable<string> Cities = [];
        IEnumerable<Country> Countries = [];
        IEnumerable<string> Salutations = [];

        public CreateComp(INationalityServiceImpl nationalityService, IOccupationServiceImpl occupationService, ISalutationServiceImpl salutationService)
        {
            _nationalityService = nationalityService;
            _occupationService = occupationService;
            _salutationService = salutationService;
        }

        protected override async Task OnInitializedAsync()
        {
            await GetAdditionalInformation();
            await base.OnInitializedAsync();
        }


        async Task GetAdditionalInformation()
        {
            Nationalities = await _nationalityService.GetNationalities();

            Occupations = await _employeeService!.GetOccupationsAsync("Position");

            var rsp = await _addressService.GetCountries(105);

            if (rsp.IsSuccess)
            {
                Countries = rsp.Data ?? [];
            }

            Cities = await _addressService.GetCities();

            var rsp1 = await _salutationService.GetSalutions();

            if (rsp1.IsSuccess)
            {
                Salutations = rsp1.Data!.Select(x => x.Title!).ToList();
            }
        }
    }
}