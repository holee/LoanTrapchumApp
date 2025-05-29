using Applications.Responses.Lists.Collaterals;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Collaterals
{
    public partial class CollateralsProperties
    {
        RadzenDataGrid<VCollateralLines> propertiesGrid = default!;
        IEnumerable<CollateralSubTypeResponse> CollateralProperties = [];
        List<VCollateralLines> Properties = [];
        void SelectedPeopleChanged()
        {

        }

    }
}