using Applications.Extensions;
using Domains.Shared.Enums;
using Applications.Features.Customers.Dto;
using Applications.Features.AdditionalInformations.Queries.GetOccupations;
using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.AdditionalInformations.Command.CreateOccupation;
using TrapchumApp.Helpers.Extensions;
using Applications.Features.AdditionalInformations.Queries.GetNationalities;
using Applications.Responses.Paginations;

namespace TrapchumApp.Pages.Lists.Customers;

public partial class CreateCustomerInfo(ILogger<CreateCustomerInfo> logger)
{
    [Parameter] public EntityCustomerCreate Model { get; set; } = new();

    const long maxFileSize = 30_000_000;

    private string? tempUrl = "/assets/profiles/profile.png";

    IEnumerable<OccupationDto> Occupations = [];

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

        await GetOccupations();
    }


    async Task AddOccupation()
    {
        if (!string.IsNullOrEmpty(Model.Occupation))
        {
            var occupation = new OccupationDtoCreate
            {
                OccupationTitle = Model.Occupation,
                Khmer = string.Empty,
                TypeOf = TypeOfEnum.Position.ToString()
            };


            var command = new CreateOccupationCommand(occupation);

            var rsp = await mediator.Send(command);  //occupationService.CreateAsync(occupation);

            if (!rsp.IsSuccess())
            {
                Model.Occupation = string.Empty;
            }
            else
            {
                await GetOccupations();
            }
        }
    }

    private async Task GetOccupations(string? filter = "Job")
    {
        var command = new GetOccupationsQuery(0, null);

        var rsp = await mediator.Send(command);//_employeeService.GetOccupationsAsync(filter);

        Occupations = rsp.Items;

    }


    private void OnSalutaionSearched(string value)
    {
        _salFilter = value;

        if (!string.IsNullOrEmpty(value))
        {
            Salutations = Salutations.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        else
        {
            Salutations = OriginalSalutations;
        }

        StateHasChanged();
    }

    private void OnOccupationSearched(string value)
    {
        //occupationFilter = value;

        if (!string.IsNullOrEmpty(value))
        {
            Occupations = OriginalOccupations.Where(x => x.OccupationTitle!.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                                             .ToList();
        }
        else
        {
            Occupations = OriginalOccupations.ToList();
        }

        StateHasChanged();

    }

    async Task LoadOccupations(LoadDataArgs args)
    {

        occupationFilter = args.Filter?.ToString() ?? string.Empty;

        await GetOccupations(occupationFilter);
    }

    private async Task GetNationalities()
    {
        var command = new GetNationalitiesQuery(0, new QueryParam(0,0));

        var rsp = await mediator.Send(command);

        _nationalities = rsp.Items ?? [];
    }

    private async Task GetSalutations(string? filter = null)
    {

        await Task.CompletedTask;
        //var rsp = await salutationService.GetSalutions(filter);

        //Salutations = rsp!.Data!.Select(x => x.Title!) ?? [];

        //OriginalSalutations = Salutations.Select(x => x) ?? [];

    }

}




