using Applications.Features.Collaterals.Dtos;
using Applications.Responses.Lists.Collaterals;
using Applications.Services.Lists;

namespace TrapchumApp.Pages.Lists.Collaterals;

public partial class SubTypeCollateralCreate 
{
    private readonly ICollateralsServiceImpl _collateralService;
    private CollateralSubTypeDtoCreate model = new();
    private List<CollateralTypeResponse> _collateralTypes = [];
    public SubTypeCollateralCreate(ICollateralsServiceImpl collateralService)
    {
        _collateralService = collateralService;
    }

    async Task GetCollateralTypes()
    {
        var rsp=await _collateralService.GetCollateralTypes();

        _collateralTypes = rsp.ToList();
    }

    protected override async Task OnInitializedAsync()
    {
        await GetCollateralTypes();

        await base.OnInitializedAsync();
    }

    private async Task SaveForm()
    {
       var rsp=  await _collateralService.UpsertAsync(model);
       
       if (rsp.IsSuccess)
       {
            NotificationService.Notify(NotificationSeverity.Error, rsp.Data);

            DialogService.Close(true);

        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error,rsp.ErrMessage);
        }
    }

    private void CancelForm()
    {
        DialogService.Close(false); 
    }
}