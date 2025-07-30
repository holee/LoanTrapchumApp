using Applications.Features.Collaterals.Services;
using Applications.Interfaces.Lists;
using Applications.Responses.Lists.Collaterals;
using Applications.Services.LIsts;

namespace TrapchumApp.Pages.Lists.Collaterals;
public partial class CustomerCollateralLineEdit
{
    private IEnumerable<CollateralTypeResponse> collateralTypes = [];
    private VCollateralLines _collateralLine = new();
    private readonly ICollateralsServiceImpl _service;

    private readonly CollateralServices collateralServices;


    [Parameter] public long? Id { get; set; }
    [Parameter] public string? CollateralId { get; set; }

    public CustomerCollateralLineEdit(ICollateralsServiceImpl service, CollateralServices collateralServices)
    {
        _service = service;

        this.collateralServices = collateralServices;
    }

    protected override async Task OnInitializedAsync()
    {

        collateralTypes = await _service.GetCollateralTypes();

        var rsp = await collateralServices.GetCustomerCollateralLine(Id, CollateralId);

        if (rsp.IsSuccess && rsp.Data is not null)
        {
            _collateralLine = rsp.Data;
        }

        await base.OnInitializedAsync();
    }
}
