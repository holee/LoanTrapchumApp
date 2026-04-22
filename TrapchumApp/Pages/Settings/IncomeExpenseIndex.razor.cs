using Applications.Features.Incomes.Command.DeleteIncomeExpense;
using Applications.Features.Incomes.Dto;
using Applications.Features.Incomes.Queries.GetIncomeExpense;
using Applications.Responses.Paginations;
using Domains.Enumerations;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Settings
{
    public partial class IncomeExpenseIndex
    {

        private IEnumerable<IncomeExpenseDto> _incomeExpense = [];
        private int _counts = 0;
        private int _skip = 0;
        private int _take = 15;
        private string? _filter = "";
        private bool isLoading = false;
        private string _sortField = nameof(IncomeExpenseDto.Types);
        private bool _sortDescending = false;
        protected override async Task OnInitializedAsync()
        {

            await GetIncomeExpenses(_filter);

            await base.OnInitializedAsync();
        }

        async Task OnLoadData(LoadDataArgs args)
        {
            _skip = args.Skip ?? 0;

            _filter = args.Filter ?? string.Empty;

            (_sortField, _sortDescending) = args.PageInfo();

            await GetIncomeExpenses(_filter);
        }


        async Task OnAddClick()
        {

            var dialogResult = await DialogService.OpenAsync<IncomeExpenseCreate>("Add Income/Expense");

            if (dialogResult is bool success && success)
            {
                await GetIncomeExpenses(_filter);
            }
        }


        void OnRowRender(RowRenderEventArgs<IncomeExpenseDto> args)
        {
            args.Data.RowIndex = args.Index + 1;
        }

        private async Task GetIncomeExpenses(string? filter = null)
        {
            isLoading = true;
            try
            {
                var param = new QueryParam(_skip, _take, _sortField, false, _sortDescending, filter);

                var query = new GetIncomeExpenseQuery(null, IncomeExpenseTypes.All, param);

                var handler = await mediator.Send(query);

                _incomeExpense = handler.Items ?? [];

                _counts = handler.Count;

                isLoading = false;
            }
            catch
            {

                isLoading = false;
            }

        }


        private async Task ClickEdit(int? arg)
        {
            Console.WriteLine("ClickEdit called with arg: " + arg);
            if (arg == null) return;
            var parameter = new Dictionary<string, object>
            {
                { "Id", arg! }
            };
            var dialogResult = await DialogService.OpenAsync<IncomeExpenseCreate>("Edit Income/Expense", parameter!);

            if (dialogResult is bool success && success)
            {
                await GetIncomeExpenses(_filter);
            }
        }

        private async Task ClickDelete(int? arg)
        {
            if (arg == null) return;
            bool? confirmed = await DialogService.Confirm($"Are you sure you want to delete this record?", "Delete Confirmation");
            if (confirmed == true)
            {
                var result = await mediator.Send(new DeleteIncomeExpenseCommand(arg.Value));
                if (result.IsError)
                {
                    await DialogService.Alert(result.FirstError.Description, "Error");
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Deleted successfully");
                    await GetIncomeExpenses(_filter);
                }
            }
        }

        private async Task ClickDetails(int? arg)
        {
            if (arg == null) return;
            var parameters = new Dictionary<string, object> { { "Id", arg } };
            await DialogService.OpenAsync<IncomeExpenseDetails>("Income/Expense Details", parameters!);
        }
    }
}