using Applications.Commons.Messages;
using Applications.Features.Addresses.Dto;
using Applications.Features.Branches.Command.CreateBranch;
using Applications.Features.Branches.Dtos;
using Applications.Features.Currencies.Dto;
using Applications.Interfaces.Banks;
using Mapster;
using Web.UI.Helpers.Validations;
using Applications.Features.Banks.Queries.GetBanks;
using Applications.Features.Addresses.Queries.GetDistricts;
using Applications.Features.Addresses.Queries.GetCommunes;
using Applications.Features.Addresses.Queries.GetVillages;
using Applications.Features.Addresses.Queries.GetProvinces;
namespace Web.UI.Pages.Lists.Branches;

public partial class BranchCreateModal(ILogger<BranchCreateModal> logger)
{
    [Parameter] public PageType PageType { get; set; } = PageType.Tab;
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback<VBranch> OnCreated { get; set; }

    private BranchDtoCreate model = new();
    [Inject] IBankService bankService { get; set; } = default!;
    protected IEnumerable<ProvinceDto> provinceList = [];
    protected IEnumerable<CommuneDto> communeList = [];
    protected IEnumerable<DistrictDto> districtList = [];
    protected IEnumerable<VillageDto> villageList = [];
    protected IEnumerable<BankResponse> bankList = [];
    protected IEnumerable<CountryDto> Countries = [];
    protected IEnumerable<string> strCountries = [];

    protected IEnumerable<string> Cities = [];

    protected IEnumerable<string> filteredItems = [];

    EditContext editContext = default!;

    protected override async Task OnInitializedAsync()
    {

        editContext = new EditContext(model);

        model.ImagePath ??= tempPath;

        var result = await mediator.Send(new GetProvincesQuery(null, new QueryParam(0, 25)));

        provinceList = result.Items;

        var results = await mediator.Send(new GetBanksQuery(null, new QueryParam()));

        if (results.Items.Any())
        {
            bankList = results.Items.Adapt<IEnumerable<BankResponse>>();
        }

        await base.OnInitializedAsync();
    }

    async Task OnSubmit()
    {

        var bank = bankList?.FirstOrDefault();

        model.BankRefId = bank?.BankId;

        var command = new CreateBranchCommand(model);

        var response = await mediator.Send(command);

        if (response.IsSuccess())
        {

            _toastService.ShowSuccess(BranchMessage.BranchCreated);

            if (PageType == PageType.Tab)
            {
                await OnCreated.InvokeAsync(response.Value);
            }
            else
            {
                DialogService.Close(true);
            }


        }
        else
        {
            editContext.ApplyValidationSummary<BranchDtoCreate>(response.GetMessage());
        }
    }

    async void SelectProvince(object e)
    {
        if (e is not null)
        {
            var id = ((ProvinceDto)e).ProvinceId;

            var result = await mediator.Send(new GetDistrictsQuery(id, null, new QueryParam(0, 30)));

            districtList = result.Items;

            model.ProDescription = ((ProvinceDto)e).ProDescription;

            model.ProInKhmer = ((ProvinceDto)e).ProInKhmer;
        }
        StateHasChanged();

    }
    async Task SelectDistrict(object e)
    {
        if (e is not null)
        {
            var id = ((DistrictDto)e).DistrictId;

            var result = await mediator.Send(new GetCommunesQuery(id, null, new QueryParam(0, 30)));

            communeList = result.Items;

            model.DisDescription = ((DistrictDto)e).DisDescription;

            model.DisInKhmer = ((DistrictDto)e).DisInKhmer;
        }
        else
        {
            communeList = [];
        }
        StateHasChanged();
    }
    async Task SelectCommune(object e)
    {
        if (e is not null)
        {
            var id = ((CommuneDto)e).CommuneId;

            var result = await mediator.Send(new GetVillagesQuery(id, null, new QueryParam(0, 30)));

            villageList = result.Items;

            model.ComDescription = ((CommuneDto)e).ComDescription;

            model.ComInKhmer = ((CommuneDto)e).ComInKhmer;
        }
    }

    void SelectVillage(object e)
    {
        if (e is not null)
        {
            model.ComDescription = ((VillageDto)e)?.VillDescription;

            model.ComInKhmer = ((VillageDto)e!)?.VilInKhmer;
        }
        StateHasChanged();
    }

    async Task Close()
    {
        if (PageType == PageType.Tab)
        {
            await OnClose.InvokeAsync();
        }
        else if (PageType == PageType.Dialog)
        {
            DialogService.Close(false);
        }
        else
        {
            NavigationManager.NavigateTo("/App/Branches");
        }
    }
}


