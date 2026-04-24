using Applications.Features.Expenses.Dto;
using Applications.Features.Expenses.EditExpenseCommands;
using Applications.Features.Expenses.Queries.GetExpenseById;
using Applications.Features.Incomes.Dto;
using Applications.Features.Incomes.Queries.GetIncomeExpense;
using Applications.Responses.Paginations;
using Domains.Enumerations;
using Domains.Models.Lists.Customers;
namespace TrapchumApp.Pages.Lists.Customers.Expenses;

public partial class CustomerModalExpenseEdit
{

    [Parameter] public long Id { get; set; } = 0;
    [Parameter] public string CustomerId { get; set; } = string.Empty;

    private ExpenseDtoUpdate model = new();

    private string _filter = string.Empty;

    private List<IncomeExpenseDto> _incomeExpenses = [];
    async Task LoadData(LoadDataArgs arg)
    {
        _filter = arg.Filter ?? String.Empty;

        await GetIncomeExpenses(_filter);
    }
    private async Task GetIncomeExpenses(string? filter = null)
    {
        var query = new GetIncomeExpenseQuery(0, IncomeExpenseTypes.Expense, new QueryParam(0, 0, string.Empty, _filter));

        var result = await mediator.Send(query);

        _incomeExpenses = result.Items;
    }


    protected override async Task OnInitializedAsync()
    {

        await GetIncomeExpenses(_filter);

        if (Id > 0 && !string.IsNullOrEmpty(CustomerId))
        {
            var query = new GetExpenseByIdUpdateQuery(Id, CustomerId);

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
        }

        await base.OnInitializedAsync();
    }



    async Task HandleFormSubmit()
    {

        var comamnd = new EditExpenseCommand(model);

        var rsp = await mediator.Send(comamnd);

        if (rsp.IsError)
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);

            return;
        }

        DialogService.Close(true);

    }

    private void HandleClose(MouseEventArgs args)
    {
        DialogService.Close(false);

    }
}