using Applications.Features.Collaterals.Dtos;
using Applications.Services.Lists;
using Radzen.Blazor;

namespace Web.UI.Pages.Lists.Collaterals
{
    public partial class CollateralsPerson
    {

        private readonly ICollateralsServiceImpl _collateralsService;

        public CollateralsPerson(ICollateralsServiceImpl collateralsService)
        {
            _collateralsService = collateralsService;
        }

        [Parameter] public List<VCollateralLines> Personals { get; set; } = [];
        [Parameter] public EventCallback<VCollateralLines> ClickedSave { get; set; }

        RadzenDataGrid<VCollateralLines> personalGrid = default!;
        DataGridEditMode editMode = DataGridEditMode.Multiple;
        IEnumerable<CollateralSubTypeDto> CollateralPersonals { get; set; } = [];
        protected override async Task OnInitializedAsync()
        {
            CollateralPersonals = await _collateralsService.GetCollateralSubTypes(3);

            await base.OnInitializedAsync();
        }

        void Reset()
        {
            Personals.Clear();
        }

        void Reset(VCollateralLines order)
        {
            Personals.Remove(order);
        }

        void OnCreateRow(VCollateralLines order)
        {
            Personals.Remove(order);
        }
        async Task InsertRow()
        {
            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }
            var order = new VCollateralLines();
            Personals.Add(order);
            await personalGrid.InsertRow(order);
        }


    }
}