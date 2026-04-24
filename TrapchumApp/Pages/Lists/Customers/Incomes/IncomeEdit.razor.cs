using Applications.Commons.Mappers.Incomes;
using Applications.Commons.Messages.Incomes;
using Applications.Features.Incomes.Command.EditIncome;
using Applications.Features.Incomes.Dto;
using Applications.Features.Incomes.Queries.GetEditIncome;
using Applications.Features.Incomes.Queries.GetIncomeExpense;
using Applications.Responses.Paginations;
using Domains.Enumerations;

namespace TrapchumApp.Pages.Lists.Customers.Incomes
{
    public partial class IncomeEdit
    {

        [Parameter] public long Id { get; set; } = 0;
        [Parameter] public string CustomerId { get; set; } = string.Empty;
        [Parameter] public EventCallback<bool> OnClose { get; set; }

        private EditIncomeDto model = new EditIncomeDto();
        private List<IncomeExpenseDto> _incomeExpenses = [];
        private string _filter = String.Empty;
        async Task LoadData(LoadDataArgs arg)
        {
            _filter = arg.Filter ?? String.Empty;

            await GetIncomeExpenses(_filter);
        }
        private async Task GetIncomeExpenses(string? filter = null)
        {
            var query = new GetIncomeExpenseQuery(0, IncomeExpenseTypes.Income, new QueryParam
            {
                Page = 0,
                PageSize = 10,
                Filter = filter
            });

            var result = await mediator.Send(query);

            _incomeExpenses = result.Items;
        }
        protected override async Task OnInitializedAsync()
        {

            await GetIncomeExpenses(_filter);

            var query = new GetEditIncomeCommand(Id, CustomerId);

            var resp = await mediator.Send(query);

            if (resp.IsError)
            {
                model = new();

                NotificationService.Notify(NotificationSeverity.Error, "Error", resp.FirstError.Description);
            }
            else
            {
                model = resp.Value;
            }

            await base.OnInitializedAsync();
        }

        async Task HandleFormSubmit()
        {

            await AddIfNotExists(model.IncomeSource);

            var command = new EditIncomeCommand(model);

            var rsp = await mediator.Send(command);

            if (!rsp.IsError)
            {
                await OnClose.InvokeAsync(true);
                NotificationService.Notify(NotificationSeverity.Success,IncomeMessage.Created);
                DialogService.Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error,rsp.FirstError.Description);
            }

        }
        private async Task CloseModal(MouseEventArgs args)
        {
            await OnClose.InvokeAsync(false);
            DialogService.Close(false);
        }
    }
}