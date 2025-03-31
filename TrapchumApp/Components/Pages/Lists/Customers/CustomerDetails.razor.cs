using Applications.Extensions;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.Lists.Customers;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.LIsts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;
using Radzen;
using TrapchumApp.Components.Pages.Lists.Collaterals;
using TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests;
using Applications.Extensions.Identity;
using Applications.Extensions.Web;
using Applications.Services.Customers;
using System.Threading.Tasks;

namespace TrapchumApp.Components.Pages.Lists.Customers
{
    public partial class CustomerDetails
    {
        private readonly IIncomeExpendServiceImpl _customerIncomeService;
        private readonly ICollateralsServiceImpl _collateralService;
        public CustomerDetails(IIncomeExpendServiceImpl customerIncomeService, ICollateralsServiceImpl collateralService)
        {
            _customerIncomeService = customerIncomeService;
            _collateralService = collateralService;
        }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        bool isCollateralTab = false;
        RadzenTabs tabs = default!;
        int selectedIndex = 0;
        bool collapse = false;
        bool IsLoading = false;
        string collapseIcon = "expand_less";
        void CollapseButton()
        {
            collapse = !collapse;
            collapseIcon = collapse ? "expand_more" : "expand_less";
        }
        async void SelectedTabIndexChanged()
        {
            if (selectedIndex == 0)
            {

            }
            if (selectedIndex == 1)
            {
                await GetCustomerCollateralLines();
            }
            if (selectedIndex == 2)
            {
                await GetCustomerIncomeAndExpense();
                await GetCustomerIncomeAndExpense();
            }
            if (selectedIndex == 3)
            {
                await GetCustomerLoanHistory();
            }
            if (selectedIndex == 5)
            {
                await GetCustomerNotes();
            }

            StateHasChanged();
        }


        async Task DeleteCustomer()
        {
            var rsp=await _customerService.DeleteCustomerAsync(CustomerId);

            if(rsp.IsSuccess)
            {
                NavigationManager.NavigateTo("/App/Customers");

                return;
            }

            NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);

        }



        async Task AddNote()
        {
            var auth = await AuthenticationState;

            if (auth.User.Identity!.IsAuthenticated==false)
            {
                return;
            }
                var userId = auth.User!.GetId()!.ToGuid();

                await DialogService.OpenAsync<CustomerNoteAdd>("New Note", new Dictionary<string, object?>
                                    {
                                        {"CustomerId",CustomerId},
                                        { "UserId",userId}
                                    });
  
        }

        protected async Task AddCollateral()
        {
            if (CustomerId.IsNullOrEmpty())
            {
                NotificationService.Notify(NotificationSeverity.Error, "Please selected customer first");
                return;
            }

            var parameters = new Dictionary<string, object?> {
            {"CustomerId", CustomerId }
        };

            var options = new DialogOptions
            {
                Width = "90vw",
                Draggable = true,
                Resizable = false
            };


            var result = await DialogService.OpenAsync<CollateralModalAdd>("Add Collateral", parameters, options);

            if (bool.TryParse(result, out bool success) && success)
            {
                var collateral = await _collateralService.GetCollateralByIdAsync(CustomerId);
            }

        }

        protected async Task AddLoanRequest()
        {
            if (CustomerId.IsNullOrEmpty())
            {
                NotificationService.Notify(NotificationSeverity.Error, "Please selected customer first");
                return;
            }
            var auth = await AuthenticationState;

            var branchId = 1;// auth.User.getBranchId;

            var userId = auth.User.GetId();

            var uri = new Dictionary<string, object?>
        {
            {"BranchId",branchId},
            {"UserId",userId}
        }.AddQueryString($"/Loan/Request/{CustomerId}/Create");
            NavigationManager.NavigateTo(uri);
        }

        async void OpenLoadRequestDialogue()
        {
            var auth = await AuthenticationState;

            var branchId = 1;// auth.User.GetBranchId();

            var userId = auth.User.GetId();

            var param = new Dictionary<string, object?>
        {
            {"BranchId",branchId},
            {"UserId",userId},
            {"CustomerId",CustomerId}
        };
            var options = new DialogOptions
            {
                Width = "90vw",
                Resizable = true
            };
            await DialogService.OpenAsync<LoanRequestCreateModal>("Loan Request", param, options);
        }






        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter] public EventCallback Close { get; set; }
        protected CustomerResponse? EntCustomer { get; set; } = new();
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

                var response = await _customerService.GetCustomerDetailsByIdAsync(CustomerId,false);

                if (response.IsSuccess)
                {
                    Customer = response.Data!;

                    VLoanRequests = Customer.LoanRequests;
                }
                else
                {

                    NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);
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
            var noteResponse = await _customerService.GetCusomerNotesAsync(CustomerId);

            if (noteResponse.IsSuccess)
            {
                CustomerNotes = noteResponse.Data!;
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
