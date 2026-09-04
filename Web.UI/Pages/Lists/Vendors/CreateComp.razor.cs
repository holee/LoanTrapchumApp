using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.AdditionalInformations.Queries.GetOccupations;
using Applications.Features.AdditionalInformations.Queries.GetSalutaions;
using Applications.Features.OtherNames.Dto;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Lists;

namespace Web.UI.Pages.Lists.Vendors
{
    public partial class CreateComp(INationalityServiceImpl nationalityService)
    {
        [Parameter] public EntityVendorCreate model { get; set; } = new();

        IEnumerable<OccupationDto> Occupations = [];
        IEnumerable<Nationality> Nationalities { get; set; } = [];

        IEnumerable<string> Statuses = ["Single", "Married", "Divorced", "Widow"];

        IEnumerable<string> Cities = [];

        IEnumerable<Country> Countries = [];

        IEnumerable<SalutationDto> _salutations = [];

        protected override async Task OnInitializedAsync()
        {
            await GetAdditionalInformation();

            await base.OnInitializedAsync();
        }


        async Task GetAdditionalInformation()
        {
            Nationalities = await nationalityService.GetNationalities();

            var rsp1 = await mediator.Send(new GetOccupationsQuery(0, new QueryParam(0, 0, string.Empty, "Position")));// _employeeService!.GetOccupationsAsync("Position");

            Occupations = rsp1.Items;

            var rsp = await _addressService.GetCountries(105);

            if (!rsp.IsError)
            {
                Countries = rsp.Value;
            }

            var rsp2 = await mediator.Send(new GetSalutationsQuery(null, new QueryParam(0, 0)));

            _salutations = rsp2.Items;


        }
    }
}