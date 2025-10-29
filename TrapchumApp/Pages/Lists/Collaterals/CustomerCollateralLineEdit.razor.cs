using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Mappers;
using Applications.Features.Customers.UseCases;
using Applications.Responses.Lists.Collaterals;
using Applications.Services.Lists;

namespace TrapchumApp.Pages.Lists.Collaterals;
public partial class CustomerCollateralLineEdit
{
    private IEnumerable<CollateralTypeResponse> collateralTypes = [];

    private IEnumerable<CollateralSubTypeResponse> allSubTypes = [];

    CollateralLineDtoCreate model = new();

    private readonly ICollateralsServiceImpl _service;

    private readonly CustomerCollateralServices collateralServices;

    private bool success = false;
    [Parameter] public long? Id { get; set; }
    [Parameter] public string? CollateralId { get; set; }

    public CustomerCollateralLineEdit(ICollateralsServiceImpl service, CustomerCollateralServices collateralServices)
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

        var id = ((CollateralTypeResponse)arg).CollateralTypeId;

        await GetCollateralSubTypes(id);
    }
    protected override async Task OnInitializedAsync()
    {

        collateralTypes = await _service.GetCollateralTypes();

        var rsp = await collateralServices.GetCustomerCollateralLine(Id, CollateralId);

        if (rsp.IsSuccess && rsp.Data is not null)
        {
            model = rsp.Data.MapCollateralLineEdit();

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

        var rsp = await collateralServices.UpsertCollateralLine(model);

        if (rsp.IsSuccess)
        {
            success = true;

            NotificationService.Notify(NotificationSeverity.Success, rsp.Data);

            DialogService.CloseSide(success);
        }
        else
        {
            success = false;

            NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
        }
    }
}
