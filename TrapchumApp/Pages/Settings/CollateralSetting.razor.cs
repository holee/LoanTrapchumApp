using Applications.Features.Settings.UseCases;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Domains.Models.Collaterals;
using Microsoft.EntityFrameworkCore;
using Radzen.Blazor;
using System.Threading.Tasks;
using Applications.Services.Lists;

namespace TrapchumApp.Pages.Settings
{
    public partial class CollateralSetting
    {
        private readonly ISettingServiceImpl _settingService;
        private readonly ICollateralsServiceImpl _collateralsService;
        private int pageIndex = 0;
        private int pageSize = 15;
        private int _counts = 0;
        private string sortField = "Id";
        private bool sortDescending = false;
        private bool includeInactive = false;
        private string filterText = string.Empty;
        private RadzenDataGrid<CollateralSubType> refGrid = default!;
        private List<CollateralSubType> _collateralSubTypes = [];
        private List<CollateralTypeResponse> _colleralTypes = [];
        List<CollateralSubType> subTypeToUpdate = new List<CollateralSubType>();
        public CollateralSetting(ISettingServiceImpl settingServiceImpl, ICollateralsServiceImpl collateralsServiceImpl)
        {
            _settingService = settingServiceImpl;
            _collateralsService = collateralsServiceImpl;
        }


        protected override async Task OnInitializedAsync()
        {

            await GetCollateralTypes();

            await GetCollateralSubTypes();

            await base.OnInitializedAsync();
        }

        private async Task GetCollateralSubTypes()
        {
            _isLoading = true;
            var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filterText);

            var rsp = await _settingService.GetColleralSubTypes(param);

            _collateralSubTypes = rsp.Items.ToList() ?? [];

            _counts = rsp.Count;

            _isLoading = false;
            
            StateHasChanged();

        }

        private async Task GetCollateralTypes()
        {
            var rsp = await _collateralsService.GetCollateralTypes();

            _colleralTypes = rsp.ToList() ?? [];

        }
        private async Task LoadData(LoadDataArgs args)
        {
            pageIndex = args.Skip ?? 0;
            pageSize = args.Top ?? 15;
            await GetCollateralSubTypes();
        }


        void Reset()
        {
            subTypeToUpdate.Clear();
        }

        void Reset(CollateralSubType order)
        {
            subTypeToUpdate.Remove(order);
        }


        async Task EditRow(CollateralSubType order)
        {
            if (!refGrid.IsValid) return;

            subTypeToUpdate.Add(order);

            await refGrid.EditRow(order);
        }

        async Task OnUpdateRow(CollateralSubType type)
        {
            Reset(type);

            var rsp = await _collateralsService.UpdateAsync(type);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            }
        }

        async Task SaveRow(CollateralSubType order)
        {
            await refGrid.UpdateRow(order);
        }

        void CancelEdit(CollateralSubType order)
        {
            Reset(order);

            refGrid.CancelEditRow(order);

            //var orderEntry = dbContext.Entry(order);

            //if (orderEntry.State == EntityState.Modified)
            //{
            //    orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
            //    orderEntry.State = EntityState.Unchanged;
            //}

        }


        async Task DeleteRow(CollateralSubType order)
        {
            Reset(order);

            //if (orders.Contains(order))
            //{
            //    dbContext.Remove<Order>(order);

            //    dbContext.SaveChanges();

            //    await ordersGrid.Reload();
            //}
            //else
            //{
            //    ordersGrid.CancelEditRow(order);
            //    await ordersGrid.Reload();
            //}

            await Task.CompletedTask; // Placeholder for actual delete logic
        }

        async Task InsertRow()
        {
            //if (!ordersGrid.IsValid) return;

            //if (editMode == DataGridEditMode.Single)
            //{
            //    Reset();
            //}

            //var order = new Order();
            //ordersToInsert.Add(order);
            //await ordersGrid.InsertRow(order);

            await Task.CompletedTask; // Placeholder for actual insert logic
        }

        async Task InsertAfterRow(CollateralSubType row)
        {
            //if (!ordersGrid.IsValid) return;

            //if (editMode == DataGridEditMode.Single)
            //{
            //    Reset();
            //}

            //var order = new Order();
            //ordersToInsert.Add(order);
            //await ordersGrid.InsertAfterRow(order, row);


            await Task.CompletedTask; // Placeholder for actual insert logic
        }

        void OnCreateRow(CollateralSubType order)
        {
            //dbContext.Add(order);

            //dbContext.SaveChanges();

            //ordersToInsert.Remove(order);
        }

    }
}