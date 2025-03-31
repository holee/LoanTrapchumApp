using Applications.Extensions;
using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.Customers;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.Customers
{
    public partial class CustomerInc
    {
        private readonly IIncomeExpendServiceImpl _incomeExpendServiceImpl;
        public CustomerInc(IIncomeExpendServiceImpl customerService)
        {
            _incomeExpendServiceImpl = customerService;
        }

        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter] public bool ShowAdd { get; set; } = true;
        [Parameter] public bool ShowReload { get; set; } = false;
        [Parameter] public IEnumerable<VCustomerIncomes> Items { get; set; } = [];
        bool isLoading = false;
        protected override async Task OnInitializedAsync()
        {
            await GetCustomerIncomes(CustomerId);

            await base.OnInitializedAsync();
        }

        async Task GetCustomerIncomes(Guid? customerId)
        {

            isLoading = true;

            if (customerId.NotEmpty())
            {
                var resp = await _incomeExpendServiceImpl.GetCustomerCustomerIncomes(customerId);

                if (resp.IsSuccess)
                {
                    Items = resp.Data!;
                    isLoading = false;
                }
                ;
            }

            isLoading = false;

            StateHasChanged();
        }

        async void RefreshIncome()
        {
            await GetCustomerIncomes(CustomerId);

        }

        protected async void AddInComeModal()
        {

            if (CustomerId.IsNullOrEmpty())
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

            var param = new Dictionary<string, object?>
            {
                { "CustomerId",CustomerId }
            };

            bool? result = await DialogService.OpenAsync<CustomerModalIncome>("Add Income", param, options);

            if (result==true)
            {
                await GetCustomerIncomes(CustomerId);
            }

        }


    }
}