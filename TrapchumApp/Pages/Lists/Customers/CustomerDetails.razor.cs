using Applications.Bases;
using Applications.Extensions;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Commands.DeleteCustomer;
using Applications.Features.Customers.Queries.GetCustomerDetailById;
using Applications.Features.Customers.Response;
using Applications.Features.Customers.Services;
using Applications.Services.Lists;
using Domains.Models.Identity;
using Radzen.Blazor;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Pages.Lists.Collaterals;
using TrapchumApp.Pages.Lists.Customers.Expenses;
using TrapchumApp.Pages.Lists.Customers.Incomes;

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
        [Parameter, EditorRequired] public string? CustomerId { get; set; }
        [Parameter] public EventCallback<int> Closed { get; set; }
        [Parameter] public EventCallback<string?> EditClick { get; set; }

        protected VCustomerDetails Customer = new();

        protected IEnumerable<VCollateralLines> CollateralLines = [];

        bool isCollateralTab = false;

        RadzenTabs tabs = default!;

        int selectedIndex = 0;

        bool collapse = false;

        bool IsLoading = false;

        string collapseIcon = "expand_less";

        private string? _userId { get; set; }

        bool ShowEditTab = false;

        async Task DeleteCustomer()
        {
            var rsp = await mediator.Send(new DeleteCustomerCommand(CustomerId));

            if (rsp.Status == DeleteStatus.Success)
            {

                NavigationManager.NavigateTo("/App/Customers");

                return;
            }

            if (rsp.Status == DeleteStatus.InUse)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Customer cannot be deleted because it has active loan or collateral");
                return;
            }


        }

        async Task ShowEditCustomer(string? customerId)
        {
            await EditClick.InvokeAsync(customerId);

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


            var result = await DialogService.OpenAsync<CollateralModalAdd>("Add Collateral", parameters!, options);

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
                    _userId ??= auth.User.GetId();
                }

                var response = await mediator.Send(new GetCustomerDetailByIdQuery(CustomerId, false)); //_customerService.GetCustomerDetailsByIdAsync(CustomerId,true);

                if (response.IsSuccess())
                {
                    Customer = response.Value;
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, response.GetMessage());
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
                { "CustomerId", CustomerId! }
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
                { "CustomerId", CustomerId! }
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
            await Closed.InvokeAsync(2);
        }
    }
}
