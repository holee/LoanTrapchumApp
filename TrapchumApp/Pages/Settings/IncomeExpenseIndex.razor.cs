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
        
            if(dialogResult is bool  success && success)
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


        private Task ClickEdit(int? arg)
        {
            throw new NotImplementedException();
        }

        private Task ClickDelete(int? arg)
        {
            throw new NotImplementedException();
        }

        private Task ClickDetails(int? arg)
        {
            throw new NotImplementedException();
        }
    }
}