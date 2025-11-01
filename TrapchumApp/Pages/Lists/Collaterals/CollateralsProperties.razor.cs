using Applications.Features.Collaterals.Dtos;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Collaterals
{
    public partial class CollateralsProperties
    {
        RadzenDataGrid<VCollateralLines> propertiesGrid = default!;
        IEnumerable<CollateralSubTypeResponse> CollateralProperties = [];
        List<VCollateralLines> _collateralLines = []; 
        void SelectedPeopleChanged()
        {

        }

    }
}