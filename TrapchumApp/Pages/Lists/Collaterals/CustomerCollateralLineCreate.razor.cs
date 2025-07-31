using Applications.Extensions;
using Applications.Features.Customers.Services;
using Applications.Requests.Lists.Collaterals;
using Applications.Responses.Lists.Collaterals;
using Applications.Services.LIsts;

namespace TrapchumApp.Pages.Lists.Collaterals;
public partial class CustomerCollateralLineCreate
{
    private IEnumerable<CollateralTypeResponse> collateralTypes = [];

    private IEnumerable<CollateralSubTypeResponse> allSubTypes = [];

    CollateralDtoCreate model = new();

    CollateralLineDtoCreate collateralLineDto=new();

    private readonly ICollateralsServiceImpl _service;

    private readonly CustomerCollateralServices _cusCollateralServices;

    private bool success = false;
    [Parameter] public string? CollateralId { get; set; }


    protected override void OnParametersSet()
    {

        collateralLineDto ??= new CollateralLineDtoCreate();

        base.OnParametersSet();
    }


    public CustomerCollateralLineCreate(ICollateralsServiceImpl service, CustomerCollateralServices collateralServices)
    {
        _service = service;

        _cusCollateralServices = collateralServices;
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
        await GetCollateralTypes();

        await GetCustomerCollaterals(CollateralId);

        await base.OnInitializedAsync();
    }

    private async Task GetCollateralTypes()
    {
        collateralTypes = await _service.GetCollateralTypes();
    }

    private async Task GetCollateralSubTypes(int? id)
    {
        allSubTypes = await _service.GetCollateralSubTypes(id);
    }

    private async Task GetCustomerCollaterals(string? collateralId)
    {
        if (collateralId.IsNullOrEmpty())
        {
            return;
        }

        var rsp = await _cusCollateralServices.GetCustomerCollateralAsync(collateralId, true);

        if (rsp.IsSuccess && rsp.Data is not null)
        {
            var collateral = rsp.Data!;

            CollateralId = collateralId;

            model = collateral.ToCollateralDtoCreate();

            model.CollateralLines.Add(collateralLineDto);
        }
    }




    async Task HandleFormSubmit()
    {

 

        var rsp = await _service.CreateAsync(model);

        if (rsp.IsSuccess)
        {
            success = true;

            NotificationService.Notify(NotificationSeverity.Success, rsp.Data);

            DialogService.Close(success);
        }
        else
        {
            success = false;

            NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
        }
    }
}
