using Applications.Features.Expenses.Queries.GetExpenseByCustomerId;
using Applications.Features.Incomes.Queries.GetIncomeByCustomerId;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerProfit
    {
        [Parameter] public string CustomerId { get; set; } = string.Empty;

        private decimal? _profits = 0.0M;
        private readonly IMediator _med;
        private IEnumerable<VCustomerIncomes> _incomes = [];
        private IEnumerable<VCustomerExpenses> _expenses = [];
        public CustomerProfit(IMediator med)
        {
            _med = med;
        }

        protected override async Task OnInitializedAsync()
        {

            var income = await _med.Send(new GetIncomeByCustomerQuery(CustomerId));

            var expense = await _med.Send(new GetExpenseByCustomerQuery(CustomerId));

            if(!income.IsError)
            {
                _incomes = income.Value;
            }
            
            if(!expense.IsError)
            {
                _expenses = expense.Value;
            }

            _profits = (_incomes.Sum(x => x.IncomeAmount) ?? 0) - (_expenses.Sum(x => x.ExpendAmount) ?? 0);

            await base.OnInitializedAsync();

        }

        void GetIncomes(IEnumerable<VCustomerIncomes> args)
        {

            _incomes = args;

            _profits = (_incomes.Sum(x => x.IncomeAmount) ?? 0) - (_expenses.Sum(x => x.ExpendAmount) ?? 0);


            StateHasChanged();
        }

        void GetExpenses(IEnumerable<VCustomerExpenses> args)
        {
            _expenses = args;

            _profits = (_incomes.Sum(x => x.IncomeAmount) ?? 0) - (_expenses.Sum(x => x.ExpendAmount) ?? 0);

            StateHasChanged();
        }

    }
}