using Applications.Features.Collaterals.Dtos;
using Radzen.Blazor;

namespace Web.UI.Pages.Lists.Collaterals
{
    public partial class CollateralsProperties
    {
        RadzenDataGrid<VCollateralLines> propertiesGrid = default!;
        IEnumerable<CollateralSubTypeDto> CollateralProperties = [];
        List<VCollateralLines> _collateralLines = []; 
        void SelectedPeopleChanged()
        {

        }

    }
}