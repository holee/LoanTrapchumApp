using Applications.Extensions;
using Applications.Extensions.Identity;
using Applications.Extensions.Web;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.Customers;
using Applications.Services.LIsts;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;
using TrapchumApp.Pages.Lists.Collaterals;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerDetails
    {
        private readonly IIncomeExpendServiceImpl _customerIncomeService;
        private readonly ICollateralsServiceImpl _collateralService;

        public CustomerDetails(IIncomeExpendServiceImpl customerIncomeService,
            ICollateralsServiceImpl collateralService)
        {
            _customerIncomeService = customerIncomeService;
            _collateralService = collateralService;
        }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [Parameter, SupplyParameterFromQuery] public int? BranchId { get; set; } = 0;
        [Parameter, SupplyParameterFromQuery] public string? UserId { get; set; }
        [Parameter, EditorRequired] public string? CustomerId { get; set; }
        [Parameter] public EventCallback Close { get; set; }
        protected VCustomerList Customer = new();
        protected IEnumerable<VCollateralLines> CollateralLines = [];
        bool isCollateralTab = false;
        RadzenTabs tabs = default!;
        int selectedIndex = 0;
        bool collapse = false;
        bool IsLoading = false;
        string collapseIcon = "expand_less";
        bool ShowEditTab = false;
        async void SelectedTabIndexChanged()
        {
            if (selectedIndex == 0)
            {
                //await GetCustomerLoanReuquests();
            }

            if (selectedIndex == 1)
            {
                await GetCustomerCollateralLines();
            }

            if (selectedIndex == 2)
            {
                // await GetCustomerIncomeAndExpense();
                // await GetCustomerIncomeAndExpense();
            }

            if (selectedIndex == 3)
            {
                // await GetCustomerLoanHistory();
            }

            if (selectedIndex == 5)
            {
                //await GetCustomerNotes();
            }

            StateHasChanged();
        }


        async Task DeleteCustomer()
        {
            var rsp = await _customerService.DeleteCustomerAsync(CustomerId);

            if (rsp.IsSuccess)
            {
                NavigationManager.NavigateTo("/App/Customers");

                return;
            }

            NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
        }

        void ShowEditCustomer()
        {
            if (ShowEditTab == false)
            {
                ShowEditTab = true;
                selectedIndex = 1;
            }
        }

        void CloseEditTab(int index)
        {
            if (index > 0)
            {
                selectedIndex = index - 1;
                ShowEditTab = false;
            }
        }
        protected async Task AddCollateral()
        {
            if (CustomerId.IsNullOrEmpty())
            {
                NotificationService.Notify(NotificationSeverity.Error, "Please selected customer first");
                return;
            }

            var parameters = new Dictionary<string, object?>
            {
                { "CustomerId", CustomerId }
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

            var branchId = 1; // auth.User.getBranchId;

            var userId = auth.User.GetId();

            var uri = new Dictionary<string, object?>
            {
                { "BranchId", branchId },
                { "UserId", userId }
            }.AddQueryString($"/Loan/Request/{CustomerId}/CreateNewUser");
            NavigationManager.NavigateTo(uri);
        }


        protected override async Task OnInitializedAsync()
        {
            try
            {

                IsLoading = true;


                var auth = await AuthenticationState;

                if (auth.User.Identity!.IsAuthenticated)
                {
                    UserId ??= auth.User.GetId();
                }

                var response = await _customerService.GetCustomerDetailsByIdAsync(CustomerId, false);

                if (response.IsSuccess)
                {
                    Customer = response.Data!;
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

        protected async void AddInComeModal()
        {
            await DialogService.OpenAsync<CustomerModalIncome>("Add Income", options: new DialogOptions
            {
                Width = "50rem",
                Height = "25rem",
                Resizable = false,
                Draggable = true,
            }, parameters: new Dictionary<string, object?>
            {
                { "CustomerId", CustomerId }
            });
        }

        protected async void AddExpenseModal()
        {
            await DialogService.OpenAsync<CustomerModalExpense>("Add Expense", options: new DialogOptions
            {
                Width = "50rem",
                Height = "25rem",
                Resizable = false,
                Draggable = true,
            }, parameters: new Dictionary<string, object?>
            {
                { "CustomerId", CustomerId }
            });
        }


        void ToList()
        {
            NavigationManager.NavigateTo("/Customers");
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

        async void CloseTab()
        {
            await Close.InvokeAsync();
        }
    }
}
