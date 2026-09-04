using Applications.Features.Collaterals.Dtos;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.UI.Services;

namespace Web.UI.Pages.Settings
{
    public partial class CollateralSetting
    {
        [Inject] private ISettingApi SettingApi { get; set; } = default!;

        private bool _isLoading = false;

        private int pageIndex = 0;

        private int pageSize = 15;

        private int _counts = 0;

        private string sortField = "Id";

        private bool sortDescending = false;

        private RadzenDataGrid<CollateralSubTypeDto> refGrid = default!;

        private List<CollateralSubTypeDto> _collateralSubTypes = [];

        private List<CollateralTypeDto> _colleralTypes = [];

        protected override async Task OnInitializedAsync()
        {
            await GetCollateralTypes();

            await GetCollateralSubTypes();

            await base.OnInitializedAsync();
        }

        private async Task GetCollateralTypes()
        {
            try
            {
                var types = await SettingApi.GetCollateralTypesAsync();

                _colleralTypes = types ?? [];
            }
            catch (Exception ex)
            {
                _colleralTypes = [];

                NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
            }
        }

        private async Task GetCollateralSubTypes()
        {
            _isLoading = true;

            try
            {
                var pager = await SettingApi.GetCollateralSubTypesAsync(pageIndex, pageSize, sortField, sortDescending, true, null);

                _collateralSubTypes = pager.Items ?? [];

                _counts = pager.Count;
            }
            catch (Exception ex)
            {
                _collateralSubTypes = [];

                _counts = 0;

                NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task LoadData(LoadDataArgs args)
        {
            pageIndex = args.Skip ?? 0;

            pageSize = args.Top ?? pageSize;

            (sortField, sortDescending) = args.PageInfo(nameof(CollateralSubTypeDto.Id));

            await GetCollateralSubTypes();
        }

        private void Reset()
        {
        }

        private async Task EditRow(CollateralSubTypeDto item)
        {
            if (!refGrid.IsValid)
            {
                return;
            }

            await refGrid.EditRow(item);
        }

        private async Task SaveRow(CollateralSubTypeDto item)
        {
            await refGrid.UpdateRow(item);
        }

        private void CancelEdit(CollateralSubTypeDto item)
        {
            refGrid.CancelEditRow(item);
        }

        private async Task OnUpdateRow(CollateralSubTypeDto item)
        {
            try
            {
                var rsp = await SettingApi.UpdateCollateralSubTypeAsync(item.Id, item);

                NotificationService.Notify(NotificationSeverity.Success, "Success", rsp.Message);

                await GetCollateralSubTypes();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
            }
        }

        private async Task OnCreateRow(CollateralSubTypeDto item)
        {
            await OnUpdateRow(item);
        }

        private async Task ClickAdd()
        {
            var result = await DialogService.OpenAsync<CollateralSubTypeCreate>("Add Collateral Sub-Type",
                options: new DialogOptions { AutoFocusFirstElement = false });

            if (result is true)
            {
                await GetCollateralSubTypes();
            }
        }
    }
}
