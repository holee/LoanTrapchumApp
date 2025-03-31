using Applications.Responses.Collaterals;
using Applications.Services.LIsts;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Lists.Collaterals
{
    public partial class CollateralsPerson
    {

        private readonly ICollateralsServiceImpl _collateralsService;

        public CollateralsPerson(ICollateralsServiceImpl collateralsService)
        {
            _collateralsService = collateralsService;
        }

        [Parameter] public List<CollateralLineResponse> Personals { get; set; } = [];
        [Parameter] public EventCallback<CollateralLineResponse> ClickedSave { get; set; }

        RadzenDataGrid<CollateralLineResponse> personalGrid = default!;
        DataGridEditMode editMode = DataGridEditMode.Multiple;
        IEnumerable<CollateralSubTypeResponse> CollateralPersonals { get; set; } = [];
        protected override async Task OnInitializedAsync()
        {
            CollateralPersonals = await _collateralsService.GetCollateralSubTypes(3);

            await base.OnInitializedAsync();
        }

        void Reset()
        {
            Personals.Clear();
        }

        void Reset(CollateralLineResponse order)
        {
            Personals.Remove(order);
        }

        void OnCreateRow(CollateralLineResponse order)
        {
            Personals.Remove(order);
        }
        async Task InsertRow()
        {
            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }
            var order = new CollateralLineResponse();
            Personals.Add(order);
            await personalGrid.InsertRow(order);
        }


    }
}