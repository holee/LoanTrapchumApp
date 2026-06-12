using Applications.Commons.Messages.Incomes;
using Applications.Extensions;
using Applications.Features.Incomes.Command.DeleteIncome;
using Applications.Features.Incomes.Queries.GetIncomeByCustomerId;
using Applications.Responses.ViewModels.Customers;

namespace TrapchumApp.Pages.Lists.Customers.Incomes
{
    public partial class CustomerInc
    {
        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public bool ShowAdd { get; set; } = true;
        [Parameter] public bool ShowReload { get; set; } = false;
        [Parameter] public bool ShowCommand { get; set; } = false;
        [Parameter] public bool ShowAction { get; set; } = true;
        [Parameter] public IEnumerable<VCustomerIncomes> Items { get; set; } = [];
        [Parameter] public EventCallback<IEnumerable<VCustomerIncomes>> GetIncomes { get; set; }

        bool isLoading = false;
        protected override async Task OnInitializedAsync()
        {
            await GetCustomerIncomes(CustomerId);

            await base.OnInitializedAsync();
        }


        async Task GetEditIncome(VCustomerIncomes c)
        {
            var parameters = new Dictionary<string, object?>()
        {
            { "Id", c.IncomeId },
            { "CustomerId", CustomerId }
        };

            var dia = await DialogService.OpenAsync<IncomeEdit>("Edit Income", parameters);

            if (dia is bool && dia == true)
            {
                await GetCustomerIncomes(CustomerId);
            }
        }

        async Task DeleteIncome(VCustomerIncomes c)
        {
            var parameters = new Dictionary<string, object?>
            {
                { "Id", c.IncomeId },
                { "CustomerId", CustomerId }
            };

            var options = new ConfirmOptions
            {
                OkButtonText = "Ok",
                CancelButtonText = "Cancel"
            };

            var dia=await MsgBox.ConfirmAsync(IncomeMessage.confirmDelete, "Confirm Delete");

            if (dia == MsgBoxResult.No) return;

            var command = new DeleteIncomeCommand(c.IncomeId, CustomerId);

            var rsp = await mediator.Send(command);

            if (rsp.IsError)
            {
                _toastService.ShowError(rsp.GetMessage(), "Error");

                return;
            }


            await GetCustomerIncomes(CustomerId);

            await GetIncomes.InvokeAsync(Items);

        }


        async Task GetCustomerIncomes(string? customerId)
        {

            isLoading = true;

            if (string.IsNullOrEmpty(customerId))
            {
                isLoading = false;
                return;
            }

            if (customerId.IsNotNullOrEmpty() || !Items.Any())
            {
                var resp = await mediator.Send(new GetIncomeByCustomerQuery(customerId));

                if (!resp.IsError)
                {
                    Items = resp.Value;
                    isLoading = false;
                }
                ;
            }

            isLoading = false;

            StateHasChanged();
        }

        async Task RefreshIncome()
        {
            await GetCustomerIncomes(CustomerId);

        }

        protected async Task AddInComeModal()
        {

            if (CustomerId.IsNullOrEmpty())
            {
                _toastService.ShowError("Please select cusotmer..", "Error");

                return;
            }

            var options = new DialogOptions
            {
                Width = "50vw",
                Resizable = false,
                Draggable = true,
                AutoFocusFirstElement = false
            };

            var param = new Dictionary<string, object?>
            {
                { "CustomerId",CustomerId }
            };

            bool? result = await DialogService.OpenAsync<IncomeCreate>("Add Income", param, options);

            if (result == true)
            {
                await GetCustomerIncomes(CustomerId);

                await GetIncomes.InvokeAsync(Items);
            }

        }


    }
}