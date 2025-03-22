using Applications.Extensions;
using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.Customers
{
    public partial class CustomerExp
    {

        private readonly ICustomerIncomeExpendService _customerService;

        public CustomerExp(ICustomerIncomeExpendService customerService)
        {
            _customerService = customerService;
        }

        [Parameter] public Guid? CustomerId { get; set; } = 0;
        [Parameter] public bool ShowAdd { get; set; } = false;
        [Parameter] public bool ShowReload { get; set; } = false;
        [Parameter] public IEnumerable<VCustomerExpenses> Items { get; set; } = [];
        bool isLoading = false;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await GetCustomerExpense(CustomerId);
        }

        async Task GetCustomerExpense(Guid? customerId)
        {

            isLoading = true;

            if (CustomerId.NotEmpty())
            {
                var resp = await _customerService.GetCustomerCustomerExpenses(customerId);

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

        protected async void AddExpenseModal()
        {

            if (!CustomerId.NotEmpty())
            {
                NotificationService.Notify(NotificationSeverity.Info, "Please select cusotmer..");
                return;
            }

            var options = new Radzen.DialogOptions
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
            }

        }









    }
}