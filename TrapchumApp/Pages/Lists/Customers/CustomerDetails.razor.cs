using Applications.Extensions;
using Applications.Extensions.Web;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.LIsts;
using Radzen.Blazor;
using TrapchumApp.Pages.Lists.Collaterals;
using Domains.Models.Identity;
using Applications.Features.Customers.Services;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerDetails
    {
        private readonly IIncomeExpendServiceImpl _customerIncomeService;

        private readonly ICollateralsServiceImpl _collateralService;

        private readonly ILogger<CustomerDetails> _logger;
        public CustomerDetails(IIncomeExpendServiceImpl customerIncomeService, ICollateralsServiceImpl collateralService, ILogger<CustomerDetails> logger)
        {
            _customerIncomeService = customerIncomeService;

            _collateralService = collateralService;

            _logger = logger;
        }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter, SupplyParameterFromQuery] public string? UserId { get; set; }
        [Parameter, EditorRequired] public string? CustomerId { get; set; }
        [Parameter] public EventCallback<int> Close { get; set; }
        [Parameter] public EventCallback<int> EditClick { get; set; }

        protected VCustomerList Customer = new();

        protected IEnumerable<VCollateralLines> CollateralLines = [];

        bool isCollateralTab = false;

        RadzenTabs tabs = default!;

        int selectedIndex = 0;

        bool collapse = false;

        bool IsLoading = false;

        string collapseIcon = "expand_less";

        bool ShowEditTab = false;

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

        async Task ShowEditCustomer()
        {
            await EditClick.InvokeAsync(2);

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

                var response = await _customerService.GetCustomerDetailByIdAsync(CustomerId, o => o.LoanRequests);

                if (response.IsSuccess && response.Data is not null)
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

                _logger.LogError(ex, ex.Message);

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

        async Task CloseTab()
        {
            await Close.InvokeAsync(2);
        }
    }
}
