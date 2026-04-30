using Applications.Features.Settings.Queries.CollateralSubTypes.GetCollateralSubTypes;
using Applications.Features.Collaterals.Dtos;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Domains.Models.Collaterals;
using Radzen.Blazor;
using TrapchumApp.Pages.Lists.Collaterals;

namespace TrapchumApp.Pages.Settings
{
    public partial class CollateralSetting
    {

        private bool _isLoading = false;
        private int pageIndex = 0;
        private int pageSize = 15;
        private int _counts = 0;
        private string sortField = "Id";
        private bool sortDescending = false;
        private RadzenDataGrid<CollateralSubType> refGrid = default!;
        private List<CollateralSubType> _collateralSubTypes = [];
        private List<CollateralTypeDto> _colleralTypes = [];


        protected override async Task OnInitializedAsync()
        {
            _colleralTypes = (await _collateralService.GetCollateralTypes(null)).ToList();
            await GetCollateralSubTypes();
            await base.OnInitializedAsync();
        }

        private async Task GetCollateralSubTypes()
        {
            _isLoading = true;
            var param = new QueryParam(pageIndex, pageSize, sortField, true, sortDescending, null);
            var rsp = await mediator.Send(new GetCollateralSubTypesQuery(param));
            _collateralSubTypes = rsp.Items?.Select(x => new CollateralSubType
            {
                Id = x.Id,
                CollateralSubTypeName = x.CollateralSubTypeName,
                CollateralSubTypeNameInKhmer = x.CollateralSubTypeNameInKhmer,
                CollateralTypeRefId = x.CollateralTypeRefId,
                ValueRate = x.ValueRate,
                LoanSizeAmuntMin = x.LoanSizeAmuntMin,
                LoanSizeAmuntMax = x.LoanSizeAmuntMax,
                IsActive = x.IsActive
            }).ToList() ?? [];
            _counts = rsp.Count;
            _isLoading = false;
            StateHasChanged();
        }

        private async Task LoadData(LoadDataArgs args)
        {
            pageIndex = args.Skip ?? 0;
            pageSize = args.Top ?? 15;
            if (args.Sorts?.Any() == true)
            {
                sortField = args.Sorts.First().Property ?? "Id";
                sortDescending = args.Sorts.First().SortOrder == SortOrder.Descending;
            }
            await GetCollateralSubTypes();
        }

        void Reset() { }

        async Task EditRow(CollateralSubType order)
        {
            if (!refGrid.IsValid) return;
            await refGrid.EditRow(order);
        }

        async Task SaveRow(CollateralSubType order) => await refGrid.UpdateRow(order);

        void CancelEdit(CollateralSubType order) => refGrid.CancelEditRow(order);

        async Task OnUpdateRow(CollateralSubType item)
        {
            var rsp = await _collateralService.Upsert(item);
            if (rsp.IsSuccess)
                NotificationService.Notify(NotificationSeverity.Success, "Saved", "Collateral sub-type saved.");
            else
                await DialogService.Alert(rsp.Errors.ErrMessage, "Error");
        }

        void OnCreateRow(CollateralSubType item) { }

        async Task ClickAdd()
        {
            var result = await DialogService.OpenAsync<SubTypeCollateralCreate>("Add",
                options: new DialogOptions { AutoFocusFirstElement = false });

            if (result is bool b && b)
                await GetCollateralSubTypes();
        }
    }
}