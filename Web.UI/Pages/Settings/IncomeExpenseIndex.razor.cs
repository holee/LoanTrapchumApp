using Applications.Features.Incomes.Dto;
using Domains.Enumerations;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.UI.Helpers.Extensions;
using Web.UI.Services;

namespace Web.UI.Pages.Settings
{
    public partial class IncomeExpenseIndex
    {
        [Inject] private ISettingApi SettingApi { get; set; } = default!;

        private List<IncomeExpenseDto> _incomeExpense = [];

        private RadzenDataGrid<IncomeExpenseDto> _gridRef = default!;

        private int _counts = 0;

        private int _skip = 0;

        private int _take = 15;

        private string? _filter = string.Empty;

        private bool isLoading = false;

        private string _sortField = nameof(IncomeExpenseDto.Types);

        private bool _sortDescending = false;

        protected override async Task OnInitializedAsync()
        {
            await GetIncomeExpenses();

            await base.OnInitializedAsync();
        }

        private async Task GetIncomeExpenses()
        {
            isLoading = true;

            try
            {
                var pager = await SettingApi.GetIncomeExpensesAsync(null, _skip, _take, _sortField, _sortDescending,
                    _filter, IncomeExpenseTypes.All);

                _incomeExpense = pager.Items ?? [];

                _counts = pager.Count;
            }
            catch (Exception ex)
            {
                _incomeExpense = [];

                _counts = 0;

                NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task OnLoadData(LoadDataArgs args)
        {
            _skip = args.Skip ?? 0;

            _take = args.Top ?? _take;

            if (!string.IsNullOrWhiteSpace(args.Filter))
            {
                _filter = args.Filter;
            }

            (_sortField, _sortDescending) = args.PageInfo(nameof(IncomeExpenseDto.Types));

            await GetIncomeExpenses();
        }

        private void OnRowRender(RowRenderEventArgs<IncomeExpenseDto> args)
        {
            if (args.Data is null) return;

            args.Data.RowIndex = args.Index + 1;
        }

        private async Task PageSizeChanged(int size)
        {
            _take = size;

            _skip = 0;

            await GetIncomeExpenses();
        }

        private async Task ClickAdd()
        {
            var result = await DialogService.OpenAsync<IncomeExpenseCreate>("Add Income / Expense",
                options: new DialogOptions { AutoFocusFirstElement = false });

            if (result is true)
            {
                await GetIncomeExpenses();
            }
        }

        private async Task ClickEdit(int? arg)
        {
            var result = await DialogService.OpenAsync<IncomeExpenseCreate>("Edit Income / Expense",
                new Dictionary<string, object?> { { nameof(IncomeExpenseCreate.Id), arg } },
                new DialogOptions { AutoFocusFirstElement = false });

            if (result is true)
            {
                await GetIncomeExpenses();
            }
        }

        private async Task ClickDelete(int? arg)
        {
            if (arg is null or 0)
            {
                return;
            }

            var confirmed = await DialogService.Confirm("Do you want to delete this record?", "Confirm Delete",
                new ConfirmOptions { OkButtonText = "Delete", CancelButtonText = "Cancel" });

            if (confirmed != true)
            {
                return;
            }

            try
            {
                var rsp = await SettingApi.DeleteIncomeExpenseAsync(arg.Value);

                NotificationService.Notify(NotificationSeverity.Success, "Deleted", rsp.Message);

                await GetIncomeExpenses();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
            }
        }

        private async Task ClickDetails(int? arg)
        {
            await DialogService.OpenAsync<IncomeExpenseDetails>("Income / Expense Details",
                new Dictionary<string, object?> { { nameof(IncomeExpenseDetails.Id), arg } },
                new DialogOptions { AutoFocusFirstElement = false });
        }
    }
}
