using Applications.Extensions;
using Applications.Features.Customers.Services;
using Applications.Responses.ViewModels.Customers;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerExp
    {

        private readonly IIncomeExpendServiceImpl _incomeExpendServiceImpl;

        public CustomerExp(IIncomeExpendServiceImpl incomeExpendServiceImpl)
        {
            _incomeExpendServiceImpl = incomeExpendServiceImpl;
        }

        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public bool ShowAdd { get; set; } = false;
        [Parameter] public bool ShowReload { get; set; } = false;
        [Parameter] public bool ShowCommand { get; set; } = false;
        [Parameter] public IEnumerable<VCustomerExpenses> Items { get; set; } = [];
        [Parameter] public EventCallback<IEnumerable<VCustomerExpenses>> GetExpenses { get; set; } 
        bool isLoading = false;
        RadzenDataGrid<VCustomerExpenses>? grid = default!;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await GetCustomerExpense(CustomerId);
        }

        async Task GetCustomerExpense(string? customerId)
        {

            isLoading = true;

            if (CustomerId.IsNotNullOrEmpty())
            {
                var resp = await _incomeExpendServiceImpl.GetCustomerCustomerExpenses(customerId);

                if (resp.IsSuccess)
                {
                    Items = resp.Data!;

                    isLoading = false;
                }
            }

            isLoading = false;

            StateHasChanged();
        }



        async void RefreshIncome()
        {
            await GetCustomerExpense(CustomerId);

        }

        protected async Task AddExpenseModal()
        {

            if (!CustomerId.IsNotNullOrEmpty() || !Items.Any())
            {
                NotificationService.Notify(NotificationSeverity.Info, "Please Select cusotmer..");
                return;
            }

            var options = new DialogOptions
            {
                Width = "50vw",
                Resizable = false,
                Draggable = true,
            };
            var parameters = new Dictionary<string, object?>
            {
                { "CustomerId",CustomerId}
            };
            bool result = await DialogService.OpenAsync<CustomerModalExpense>("Add Expense", parameters, options);

            if (bool.TryParse(result.ToString(), out bool success) && success)
            {
                await GetCustomerExpense(CustomerId);
                await GetExpenses.InvokeAsync(Items);
            }

        }









    }
}