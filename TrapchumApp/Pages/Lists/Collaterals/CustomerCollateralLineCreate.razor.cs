using Applications.Extensions;
using Applications.Features.Collaterals.Commands.AddOrUpdateCollaterals;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Collaterals.Queries.GetCollateralDetails;
using Applications.Services.Files;
using Applications.Services.Lists;
using Domains.Models.Files;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Collaterals;

public partial class CustomerCollateralLineCreate
{
    private IEnumerable<CollateralTypeResponse> collateralTypes = [];

    private IEnumerable<CollateralSubTypeResponse> allSubTypes = [];

    private CollateralDtoCreate baseModel = new();

    private CollateralLineDtoCreate model = new();

    private readonly ICollateralsServiceImpl _service;

    private bool success;

    private bool isSaving;

    private FileModel? uploadedFile;

    private RadzenUpload radzenUpload = default!;

    private const long MaxFileSize = 10L * 1024 * 1024 * 1024; // 10 GB

    [Parameter] public string? CollateralId { get; set; }

    public CustomerCollateralLineCreate(ICollateralsServiceImpl service)
    {
        _service = service;
        //_cusCollateralServices = collateralServices;
        //_fileUploadService = fileUploadService;
    }

    protected override async Task OnInitializedAsync()
    {
        model ??= new CollateralLineDtoCreate(CollateralId);

        await GetCollateralTypes();

        await GetCustomerCollaterals(CollateralId);

        await base.OnInitializedAsync();
    }

    private async Task GetCollateralTypes()
    {
        collateralTypes = await _service.GetCollateralTypes();
    }

    private async Task GetCollateralSubTypes(object? arg)
    {
        if (arg is null) return;

        var id = ((CollateralTypeResponse)arg).CollateralTypeId;

        await GetCollateralSubTypes(id);
    }

    private async Task GetCollateralSubTypes(int? id)
    {
        allSubTypes = await _service.GetCollateralSubTypes(id);
    }

    private async Task GetCustomerCollaterals(string? collateralId)
    {
        if (collateralId.IsNullOrEmpty()) return;

        var query = new GetCollateralDetailQuery(collateralId!);

        var rsp = await mediator.Send(query);

        if (!rsp.IsError)
        {
            var collateral = rsp.Value;

            CollateralId = collateralId;

            baseModel = collateral.ToCollateralDtoCreate();

            baseModel.CollateralLines.Add(model!);
        }
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
            NotificationService.Notify(NotificationSeverity.Warning,$"Market Price must be less than {maxPrice}");
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

    private async Task HandleFormSubmit()
    {
        isSaving = true;

        try
        {
            var command = new AddOrUpdateCollateralCommand(baseModel);

            var rsp = await mediator.Send(command);

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
        finally
        {
            isSaving = false;
        }
    }
}
