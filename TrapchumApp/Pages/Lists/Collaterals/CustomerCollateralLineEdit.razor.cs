using Applications.Features.Customers.Mappers;
using Applications.Features.Customers.Services;
using Applications.Requests.Lists.Collaterals;
using Applications.Responses.Lists.Collaterals;
using Applications.Services.LIsts;

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
