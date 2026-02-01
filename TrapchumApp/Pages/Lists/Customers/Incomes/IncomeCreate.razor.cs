using Applications.Commons.Messages.Incomes;
using Applications.Features.Incomes.Command.CreateIncome;
using Applications.Features.Incomes.Dto;
using Applications.Features.Incomes.Queries.GetIncomeExpense;
using Applications.Responses.Paginations;
using Domains.Enumerations;
using System.Threading.Tasks;

namespace TrapchumApp.Pages.Lists.Customers.Incomes
{
    public partial class IncomeCreate
    {
        [Parameter] public string CustomerId { get; set; } = string.Empty;
        [Parameter] public EventCallback<bool> OnClose { get; set; }

        private List<IncomeExpenseDto> _incomeExpenses = [];

        private CreateIncomeDto model = new CreateIncomeDto();

        private string _filter = string.Empty;

       

        protected override async Task OnInitializedAsync()
        {
            await GetIncomeExpenses(_filter);
            model.CustomerId = CustomerId;
            await base.OnInitializedAsync();
        }

        async Task LoadData(LoadDataArgs arg)
        {
            _filter = arg.Filter ?? String.Empty;
            await GetIncomeExpenses(_filter);
        }
        private async Task GetIncomeExpenses(string? filter = null)
        {
            var query = new GetIncomeExpenseQuery(0, IncomeExpenseTypes.Income, new QueryParam
            {
                Skip = 0,
                PageSize = 10,
                Filter = filter
            });

            var result = await mediator.Send(query);

            _incomeExpenses = result.Items;
        }

        async Task HandleFormSubmit()
        {
            var command = new CreateIncomeCommand(model);

            var rsp = await mediator.Send(command);

            if (!rsp.IsError)
            {
                await OnClose.InvokeAsync(true);

                NotificationService.Notify(NotificationSeverity.Success, IncomeMessage.Created);

                DialogService.Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
            }
        }

        private async Task CloseModal(MouseEventArgs args)
        {
            await OnClose.InvokeAsync(false);
            DialogService.Close(false);
        }
    }
}