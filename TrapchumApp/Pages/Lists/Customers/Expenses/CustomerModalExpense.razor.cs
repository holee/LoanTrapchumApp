using Applications.Features.Customers.Services;
using Applications.Features.Expenses.CreateExpensesCommands;
using Applications.Features.Expenses.Dto;
using Applications.Features.Expenses.Queries.GetExpenseByCustomerId;
using Applications.Responses.ViewModels.Customers;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Customers.Expenses
{
    public partial class CustomerModalExpense
    {
        List<ExpenseDtoCreate> CustomerExpenses = new List<ExpenseDtoCreate>();

        RadzenDataGrid<VCustomerExpenses> incomeGrid = default!;

        List<VCustomerExpenses> Expenses = [];

        [Parameter] public string CustomerId { get; set; } = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await GetCustomerIncome(CustomerId);
        }

        async Task GetCustomerIncome(string? customerId)
        {

            if (string.IsNullOrEmpty(customerId)) return;


            var resp = await mediator.Send(new GetExpenseByCustomerQuery(customerId));

            if (!resp.IsError)
            {
                Expenses = resp.Value.ToList();
            }
        }

        private void Close()
        {
            DialogService.Close(false);
        }

        private bool isValid = true;

        async Task SaveChanged()
        {
            try
            {
                var model = Expenses.Where(x => x.ExpendAmount > 0)
                                  .Select(x => new ExpenseDtoCreate(x.ExpendId, CustomerId, x.ExpenseTitle, x.ExpendAmount, x.ExDescription))
                                  .ToList();

                if (!model.Any()) return;

                var rsp = await mediator.Send(new AddExpensesCommand(CustomerId, model));

                if (!rsp.IsError)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Added");

                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }


        void Reset()
        {

        }

        void Reset(VCustomerExpenses income)
        {
            Expenses.Remove(income);
        }

        async void InsertedRow()
        {
            var income = new VCustomerExpenses();

            var row = Expenses.Any() ? Expenses.Last() : null;

            Expenses.Add(income);

            if (row is null)
            {
                await incomeGrid.InsertRow(income);
            }
            else
            {
                await incomeGrid.InsertAfterRow(income, row);
            }


        }

        async Task DeleteRow(VCustomerExpenses income)
        {
            Reset(income);
            if (Expenses.Contains(income))
            {
                await incomeGrid.Reload();
            }
            else
            {
                incomeGrid.CancelEditRow(income);
                await incomeGrid.Reload();
            }
        }


        void CancelEdit(VCustomerExpenses income)
        {
            Reset(income);
            incomeGrid.CancelEditRow(income);
        }

        async Task SaveRow(VCustomerExpenses income)
        {
            await incomeGrid.UpdateRow(income);
        }

        async Task EditRow(VCustomerExpenses income)
        {
            await incomeGrid.EditRow(income);
        }






    }
}