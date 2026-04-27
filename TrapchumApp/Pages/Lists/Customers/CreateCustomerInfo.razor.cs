using Applications.Extensions;
using Applications.Features.Customers.Dto;
using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.AdditionalInformations.Queries.GetNationalities;
using Applications.Features.AdditionalInformations.Queries.GetSalutaions;

namespace TrapchumApp.Pages.Lists.Customers;

public partial class CreateCustomerInfo(ILogger<CreateCustomerInfo> logger)
{
    [Parameter] public EntityCustomerCreate Model { get; set; } = new();

    const long maxFileSize = 30_000_000;

    private string? tempUrl = "/assets/profiles/profile.png";

    IEnumerable<OccupationDto> _occupations = [];

    IEnumerable<OccupationDto> OriginalOccupations = [];
    IEnumerable<NationalityDto> _nationalities { get; set; } = [];

    IEnumerable<string> Salutations = [];

    IEnumerable<string> OriginalSalutations = [];

    string occupationFilter = string.Empty;

    string _salFilter = string.Empty;


    protected override async Task OnInitializedAsync()
    {
        await GetAdditionalInformation();

        if (Model.Photo.IsNotNullOrEmpty())
        {
            tempUrl = Model.Photo;
        }
        else
        {
            Model.Photo = tempUrl;
        }

        await base.OnInitializedAsync();
    }

    async Task GetAdditionalInformation()
    {
        await GetNationalities();

        await GetSalutations();
    }
    private async Task GetNationalities()
    {
        var command = new GetNationalitiesQuery(0, new QueryParam(0, 0));

        var rsp = await mediator.Send(command);

        _nationalities = rsp.Items ?? [];
    }

    private async Task GetSalutations(string? filter = null)
    {
        var comamnd = new GetSalutationsQuery(0, new QueryParam(0, 0));

        var rsp = await mediator.Send(comamnd);

        Salutations = rsp.Items.Select(x => x.Title!).ToList();

    }

}




