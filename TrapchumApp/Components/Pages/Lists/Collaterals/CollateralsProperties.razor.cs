using Applications.Requests.Collaterals;
using Applications.Responses.Collaterals;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Lists.Collaterals
{
    public partial class CollateralsProperties
    {
        RadzenDataGrid<CollateralLineResponse> propertiesGrid = default!;
        IEnumerable<CollateralSubTypeResponse> CollateralProperties = [];
        List<CollateralLineResponse> Properties = [];
        void SelectedPeopleChanged()
        {

        }

    }
}