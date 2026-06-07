using Applications.Features.Expenses.Queries.GetExpenseByCustomerId;
using Applications.Features.Incomes.Queries.GetIncomeByCustomerId;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerProfit
    {
        [Parameter] public string CustomerId { get; set; } = string.Empty;
        [Parameter] public bool ShowAction { get; set; } = true;
        [Parameter] public bool ShowCommand { get; set; } = true;
        [Parameter] public EventCallback<(decimal?, decimal?)> GetIncomeAndExpense { get; set; } = default;
        private decimal? _profits = 0.0M;
        private decimal? _income = 0.00M;
        private decimal? _expense = 0.00M;
        private IEnumerable<VCustomerIncomes> _incomes = [];
        private IEnumerable<VCustomerExpenses> _expenses = [];
        protected override async Task OnInitializedAsync()
        {
            await GetCustomerIncomeAndExpense();

            await base.OnInitializedAsync();

        }

        private async Task GetCustomerIncomeAndExpense()
        {
            var income = await mediator.Send(new GetIncomeByCustomerQuery(CustomerId));

            var expense = await mediator.Send(new GetExpenseByCustomerQuery(CustomerId));

            if (!income.IsError)
            {
                _incomes = income.Value;

                _income = _incomes.Sum(x => x.IncomeAmount) ?? 0;

            }

            if (!expense.IsError)
            {
                _expenses = expense.Value;

                _expense = _expenses.Sum(x => x.ExpendAmount) ?? 0;

            }

            _profits = _income - _expense;
        }

        async Task GetIncomes(IEnumerable<VCustomerIncomes> args)
        {

            await GetCustomerIncomeAndExpense();

            await GetIncomeAndExpense.InvokeAsync((_income, _expense));

            StateHasChanged();
        }

        async Task GetExpenses(IEnumerable<VCustomerExpenses> args)
        {

            await GetCustomerIncomeAndExpense();

            await GetIncomeAndExpense.InvokeAsync((_income, _expense));

            StateHasChanged();
        }

    }
}