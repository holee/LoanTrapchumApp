using Applications.Features.Customers.Services;
using Applications.Features.Entities;
using Applications.Responses.ViewModels.Customers;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerModalExpense
    {

        private readonly IIncomeExpendServiceImpl _cusExspenseAndIncome;

        public CustomerModalExpense(IIncomeExpendServiceImpl cusExspenseAndIncome)
        {
            _cusExspenseAndIncome = cusExspenseAndIncome;
        }

        List<CustomerExpenseDtoCreate> CustomerExpenses = new List<CustomerExpenseDtoCreate>();
        RadzenDataGrid<VCustomerExpenses> incomeGrid = default!;
        List<VCustomerExpenses> Expenses = [];
        [Parameter] public string? CustomerId { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetCustomerIncome(CustomerId);
        }

        async Task GetCustomerIncome(string? customerId)
        {
            var resp = await _cusExspenseAndIncome.GetCustomerCustomerExpenses(customerId);
            if (resp.IsSuccess)
            {
                Expenses = resp.Data!.ToList();
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
                                  .Select(x => new CustomerExpenseDtoCreate(x.ExpendId, CustomerId, x.ExpenseTitle, x.ExpendAmount, x.ExDescription))
                                  .ToList();

                if (!model.Any()) return;

                var response = await _cusExspenseAndIncome.AddOrUpdateExpensesAsync(CustomerId ,model);

                if (response.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Added");

                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);
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
            Expenses.Add(income);
            await incomeGrid.InsertRow(income);
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