using Applications.Commons.Messages.Expenses;
using Applications.Extensions;
using Applications.Features.Customers.Services;
using Applications.Features.Expenses.DeleteExpenseCommand;
using Applications.Features.Expenses.Queries.GetExpenseByCustomerId;
using Applications.Features.Expenses.Queries.GetExpenseById;
using Applications.Responses.ViewModels.Customers;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Customers.Expenses
{
    public partial class CustomerExp
    {
        [Parameter] public string CustomerId { get; set; }=string.Empty;    
        [Parameter] public bool ShowAdd { get; set; } = false;
        [Parameter] public bool ShowReload { get; set; } = false;
        [Parameter] public bool ShowCommand { get; set; } = false;
        [Parameter] public bool ShowAction { get; set; } = false;
        [Parameter] public IEnumerable<VCustomerExpenses> Items { get; set; } = [];
        [Parameter] public EventCallback<IEnumerable<VCustomerExpenses>> GetExpenses { get; set; } 
        bool isLoading = false;
        RadzenDataGrid<VCustomerExpenses>? grid = default!;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await GetCustomerExpense(CustomerId);
        }


        async Task GetEditExpense(VCustomerExpenses c)
        {
            var query = new GetExpenseByIdQuery(c.ExpendId, CustomerId);

            var rsp = await mediator.Send(query);

            if (rsp.IsError)
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
                return;
            }

            var parameters = new Dictionary<string, object?>
        {
            { "Id", c.ExpendId },
            { "CustomerId", CustomerId }
        };

            var dia = await DialogService.OpenAsync<CustomerModalExpenseEdit>("Expenses", parameters);

            if (dia is bool and true)
            {
                await GetCustomerExpense(CustomerId);
            }
        }

        async Task DeleteExpense(VCustomerExpenses c)
        {
            var confirm = await DialogService.Confirm(ExpenseMessage.ConfirmDelete, "Delete");

            if (confirm == false) return;

            var command = new DeleteExpenseCommand(c.ExpendId, CustomerId);

            var rsp = await mediator.Send(command);

            if (rsp.IsError)
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
                return;
            }

            NotificationService.Notify(NotificationSeverity.Success, rsp.Value);

            await GetCustomerExpense(CustomerId);

        }


        async Task GetCustomerExpense(string? customerId)
        {

            isLoading = true;

            if(string.IsNullOrEmpty(customerId))
            {
                isLoading = false;

                return;
            }


            if (CustomerId.IsNotNullOrEmpty())
            {
                var resp = await mediator.Send(new GetExpenseByCustomerQuery(customerId));

                if (!resp.IsError)
                {
                    Items = resp.Value;

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

            if (!CustomerId.IsNotNullOrEmpty())
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
            bool result = await DialogService.OpenAsync<CreateExpense>("Add Expense", parameters, options);

            if (bool.TryParse(result.ToString(), out bool success) && success)
            {
                await GetCustomerExpense(CustomerId);

                await GetExpenses.InvokeAsync(Items);
            }

        }









    }
}