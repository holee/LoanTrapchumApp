using Applications.Extensions;
using Applications.Features.Collaterals.Commands.AddOrUpdateCollaterals;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Collaterals.Queries.GetCollateralDetails;
using Applications.Features.Collaterals.Queries.GetCollateralLineById;
using Applications.Features.Collaterals.Queries.GetCollateralSubTypesById;
using Applications.Features.Collaterals.Queries.GetCollateralTypes;
using Applications.Services.Lists;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Collaterals;

public partial class CustomerCollateralLineCreate
{
    private IEnumerable<CollateralTypeDto> collateralTypes = [];

    private IEnumerable<CollateralSubTypeDto> allSubTypes = [];

    private CollateralDtoCreate baseModel = new();

    private CollateralLineDtoCreate model = new();

    private bool success;

    private bool isSaving;

    private RadzenUpload radzenUpload = default!;

    private long maxFileSize = 1024 * 1024 * 50L; // 50 MB
    [Parameter] public string? CollateralId { get; set; }
    [Parameter] public long CollateralLineId { get; set; } = 0;
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback<VCollateralLines> OnCollateralAdd { get; set; }
    [Parameter] public PageType PageType { get; set; } = PageType.Tab;
    protected override async Task OnInitializedAsync()
    {
        model ??= new CollateralLineDtoCreate(CollateralId);

        await GetCollateralTypes();

        await GetCustomerCollaterals(CollateralId);

        await base.OnInitializedAsync();
    }

    private async Task GetCollateralTypes()
    {
        collateralTypes = await mediator.Send(new GetCollateralTypeQuery(null, null));
    }

    private async Task GetCollateralSubTypes(object? arg)
    {
        if (arg is null) return;

        var id = ((CollateralTypeDto)arg).CollateralTypeId;

        model.TypeName = ((CollateralTypeDto)arg).CollateralTypeCode;

        await GetCollateralSubTypes(id);
    }

    void OnSelectedItemChanged(object? args)
    {
        if(args is not null)
        {
            var subType = (CollateralSubTypeDto)args;

            model.SubTypeName = subType.CollateralSubTypeName;

        }
    }


    private async Task GetCollateralSubTypes(int? id)
    {
        allSubTypes = await mediator.Send(new GetCollateralSubTypesByIdQuery(id, null)); ///_service.GetCollateralSubTypes(id);
    }

    private async Task GetCustomerCollaterals(string? collateralId)
    {
        if (collateralId.IsNullOrEmpty()) return;

        var query = new GetCollateralDetailQuery(collateralId!);

        var rsp = await mediator.Send(query);

        if (rsp.IsError)
        {
            _toastService.ShowError(rsp.FirstError.Description);

            return;
        }

        var collateral = rsp.Value;

        CollateralId = collateralId;

        baseModel = collateral.ToCollateralDtoCreate();

        await GetCollaralLine();

        model.OwnerName = baseModel.CustomerName;
    }

    private async Task GetCollaralLine()
    {
        if (CollateralLineId == 0)
        {
            model = new CollateralLineDtoCreate(CollateralId);

            return;
        }


        var command = new GetCollateralLineByIdQuery(CollateralLineId, CollateralId);

        var lineRsp = await mediator.Send(command);

        if (lineRsp.IsError)
        {
            _toastService.ShowError(lineRsp.GetMessage());

            return;
        }

        model = lineRsp.Value.ToCollateralLineDtoCreate();

        await GetCollateralSubTypes(model.CategoryRefId);

    }

    private void OnMarketPriceChanged(decimal? arg, CollateralLineDtoCreate line)
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

    private async Task OnCloseTab()
    {
        if (PageType == PageType.Tab)
        {
            await OnClose.InvokeAsync();
        }
        else
        {
            DialogService.Close(success);
        }

    }

    private async Task HandleFormSubmit()
    {
        isSaving = true;

        try
        {
            baseModel.CollateralLines.Add(model);

            var command = new AddOrUpdateCollateralCommand(baseModel);

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {
                success = true;

                _toastService.ShowSuccess(rsp.Value);

                await OnCloseTab();

            }
            else
            {
                success = false;

                _toastService.ShowError(rsp.GetMessage());
            }
        }
        finally
        {
            isSaving = false;
        }
    }
}
