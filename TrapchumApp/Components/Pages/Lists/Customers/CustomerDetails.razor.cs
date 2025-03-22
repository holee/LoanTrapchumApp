using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.Lists.Customers;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Customers;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Lists.Customers
{
    public partial class CustomerDetails
    {

        private readonly ICustomerServiceImpl _customerService;
        private readonly ICustomerIncomeExpendService _customerIncomeService;
        public CustomerDetails(ICustomerServiceImpl customerService, ICustomerIncomeExpendService customerIncomeService)
        {
            _customerService = customerService;
            _customerIncomeService = customerIncomeService;
        }

        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter] public EventCallback Close { get; set; }
        protected CustomerReponse? EntCustomer { get; set; } = new();
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

                var response = await _customerService.GetCustomerDetailsByIdAsync(CustomerId);

                if (response.IsSuccess)
                {
                    Customer = response.Data!;

                    VLoanRequests = Customer.LoanRequests;
                }
                else
                {

                    NotificationService.Notify(Radzen.NotificationSeverity.Error, response.ErrMessage);
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
            }, parameters: new Dictionary<string, object?>
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
            }, parameters: new Dictionary<string, object?>
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
            var noteResponse = await _customerService.GetCusomerNotesAsyn(CustomerId, 0);

            if (noteResponse.IsSuccess)
            {
                //CustomerNotes = noteResponse.Data!;
            }
            else
            {
                CustomerNotes = [];

                NotificationService.Notify(Radzen.NotificationSeverity.Error, noteResponse.ErrMessage);
            }
        }

        async Task GetCustomerLoanHistory()
        {
            var historyResponse = await _customerService.GetCustomerLoanHistoryAsync(CustomerId);

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
            var collateralResponse = await _customerService.GetCustomerCollateralLines(CustomerId);

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
            var incomeResponse = await _customerIncomeService.GetCustomerCustomerIncomes(CustomerId);

            var expenseResponse = await _customerIncomeService.GetCustomerCustomerExpenses(CustomerId);

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
