using ApplicationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using TCLMS.Shared.Entities.Dtos.Lists.Customers;
using TCLMS.Shared.Entities.Models.Lists.Customers;
using TCLMS.Shared.Entities.ViewModels.Collaterals;
using TCLMS.Shared.Entities.ViewModels.Customers;
using TCLMS.Shared.Entities.ViewModels.Loans;

namespace TCLMS.Front.Pages.Lists.Customers
{
    public partial class CustomerDetails
    {
        [Parameter] public long CustomerId { get; set; }
        [Parameter] public EventCallback Close { get; set; }
        protected CustomerDto? EntCustomer { get; set; } = new();
        protected VCustomerList Customer = new();
        protected IEnumerable<VCollateralLines> CollateralLines = [];
        protected IEnumerable<VLoanRequests> VLoanRequests = [];
        protected IEnumerable<VCustomerIncomes> VCustomerIncomes = [];
        protected IEnumerable<VCustomerExpenses> VCustomerExpenses = [];
        protected IEnumerable<VLoanDisbursments> VLoanDisbursements = [];
        protected IEnumerable<VCustomerNotes> CustomerNotes = [];
        protected override async Task OnInitializedAsync()
        {
            try
            {

                IsLoading = true;

                var response = await _customerService.GetCustomerDetailsAsync(CustomerId);

                if (response.IsSuccess)
                {
                    Customer = response.Data!;

                    VLoanRequests = Customer.LoanRequests;
                }
                else
                {
                    var error = response.Errors!.FirstOrDefault();

                    NotificationService.Notify(Radzen.NotificationSeverity.Error, error);
                }

                IsLoading = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IsLoading = false;
            }

            await base.OnInitializedAsync();
        }
        protected void AddInCome()
        {
            AddInComeModal();
        }
        protected void AddExpense(long? id)
        {
            AddExpenseModal();
        }
        protected async void AddInComeModal()
        {
            await DialogService.OpenAsync<CustomerModalIncome>("Add Income", options: new Radzen.DialogOptions
            {
                Width = "50rem",
                Height = "25rem",
                Resizable = false,
                Draggable = true,
            }, parameters: new Dictionary<string, object>
            {
                { "CustomerId",CustomerId }
            });
        }
        protected async void AddExpenseModal()
        {
            await DialogService.OpenAsync<CustomerModalExpense>("Add Expense", options: new Radzen.DialogOptions
            {
                Width = "50rem",
                Height = "25rem",
                Resizable = false,
                Draggable = true,
            }, parameters: new Dictionary<string, object>
            {
                { "CustomerId",CustomerId }
            });
        }




        void ToList()
        {
            NavigationManager.NavigateTo("/Customers");
        }

        async Task GetCustomerNotes()
        {
            var noteResponse = await _customerService.GetCusomerNotesAsync(CustomerId, 0);

            if (noteResponse.IsSuccess)
            {
                CustomerNotes = noteResponse.Data!;
            }
            else
            {
                CustomerNotes = [];

                var error = noteResponse.Errors!.FirstOrDefault();

                NotificationService.Notify(Radzen.NotificationSeverity.Error, error);
            }
        }

        async Task GetCustomerLoanHistory()
        {
            var historyResponse = await _customerService.GetCustomerLoanHostoryAsync(CustomerId);

            if (historyResponse.IsSuccess)
            {
                VLoanDisbursements = historyResponse.Data!;
            }
            else
            {
                CollateralLines = [];
            }
        }

        async Task GetCustomerCollateralLines()
        {
            var collateralResponse = await _customerService.GetCustomerCollateralLineAsync(CustomerId);

            if (collateralResponse.IsSuccess)
            {
                CollateralLines = collateralResponse.Data!;
            }
            else
            {
                CollateralLines = [];
            }
        }


        async Task GetCustomerIncomeAndExpense()
        {
            var incomeResponse = await _customerService.GetCustomerIncomesAsync(CustomerId);

            var expenseResponse = await _customerService.GetCustomerExpensesAsync(CustomerId);

            if (incomeResponse.IsSuccess)
            {
                VCustomerIncomes = incomeResponse.Data!;
            }
            else
            {
                VCustomerIncomes = [];
            }

            if (expenseResponse.IsSuccess)
            {
                VCustomerExpenses = expenseResponse.Data!;
            }
            else
            {
                VCustomerExpenses = [];
            }
        }

        async void CloseTab()
        {
            await Close.InvokeAsync();
        }

    }
}
