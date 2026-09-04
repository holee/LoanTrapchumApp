using Applications.Features.Collaterals.Commands.CreateCollateralLines;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Collaterals.Queries.GetCollateralDetails;
using Applications.Features.Collaterals.Queries.GetCollateralLineById;
using Applications.Services.Lists;

namespace Web.UI.Pages.Lists.Collaterals;

public partial class CustomerCollateralLineEdit
{
    private IEnumerable<CollateralTypeDto> collateralTypes = [];

    private IEnumerable<CollateralSubTypeDto> allSubTypes = [];

    CollateralLineDtoCreate model = new();

    private readonly ICollateralsServiceImpl _service;

    private readonly GetCollateralDetailQueryHandler collateralServices;

    private bool success = false;
    [Parameter] public long Id { get; set; } = 0;
    [Parameter] public string CollateralId { get; set; } = string.Empty;

    public CustomerCollateralLineEdit(ICollateralsServiceImpl service, GetCollateralDetailQueryHandler collateralServices)
    {
        _service = service;

        this.collateralServices = collateralServices;
    }


    void OnMarketPriceChanged(decimal? arg, CollateralLineDtoCreate line)
    {

        if (!line.SubCategoryRefId.HasValue || line.SubCategoryRefId == 0)
        {
            model.MarketPrice = 0.00M;
            model.HotPrice = 0.00M;
            model.PawnPrice = 0.00M;
            return;
        }

        var maxPrice = allSubTypes.Where(x => x.Id == line.SubCategoryRefId)
                                  .Select(x => x.LoanSizeAmuntMax)
                                  .FirstOrDefault();
        var valueRate = allSubTypes.Where(x => x.Id == line.SubCategoryRefId)
                                  .Select(x => x.ValueRate)
                                  .FirstOrDefault();
        if (maxPrice > 0 && arg > maxPrice)
        {
            NotificationService.Notify(NotificationSeverity.Warning, $"Market Price must be less than {maxPrice}");

            model.MarketPrice = maxPrice ?? 0;

            model.HotPrice = maxPrice * valueRate;

            model.PawnPrice = line.HotPrice * valueRate;

            return;
        }

        if (arg.HasValue && valueRate > 0)
        {
            model.HotPrice = arg.Value * valueRate;

            model.PawnPrice = model.HotPrice * valueRate;
        }
        else
        {
            model.PawnPrice = arg;

            model.HotPrice = arg;
        }

        StateHasChanged();
    }


    private async Task GetCollateralSubTypes(object? arg)
    {
        if (arg is null)
        {
            return;
        }

        var id = ((CollateralTypeDto)arg).CollateralTypeId;

        await GetCollateralSubTypes(id);
    }
    protected override async Task OnInitializedAsync()
    {

        collateralTypes = await _service.GetCollateralTypes();

        var query = new GetCollateralLineByIdEditQuery(Id, CollateralId);

        var rsp = await mediator.Send(query);

        if (!rsp.IsError)
        {
            model = rsp.Value;

            await GetCollateralSubTypes(model.CategoryRefId);
        }

        await base.OnInitializedAsync();
    }

    private async Task GetCollateralSubTypes(int? id)
    {
        allSubTypes = await _service.GetCollateralSubTypes(id);
    }

    async Task HandleFormSubmit()
    {


        var command = new AddOrUpdateCollateralLineCommand(model);


        var rsp = await mediator.Send(command);//collateralServices.UpsertCollateralLine(model);

        if (!rsp.IsError)
        {
            success = true;

            NotificationService.Notify(NotificationSeverity.Success, rsp.Value);

            DialogService.Close(success);
        }
        else
        {
            success = false;

            NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
        }
    }
}
