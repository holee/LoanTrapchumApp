using Applications.Commons.Messages.Expenses;
using Applications.Features.Expenses.CreateExpenseCommands;
using Applications.Features.Expenses.Dto;
using Applications.Features.Incomes.Dto;
using Applications.Features.Incomes.Queries.GetIncomeExpense;
using Applications.Responses.Paginations;
using Domains.Enumerations;
namespace TrapchumApp.Pages.Lists.Customers.Expenses;

public partial class CreateExpense
{
    [Parameter] public string CustomerId { get; set; } = string.Empty;

    private ExpenseDtoCreate model = new();

    private List<IncomeExpenseDto> _incomeExpenses = new();

    private string _filter = string.Empty;

    async Task LoadData(LoadDataArgs arg)
    {
        _filter = arg.Filter ?? String.Empty;

        await GetIncomeExpenses(_filter);
    }
    private async Task GetIncomeExpenses(string? filter = null)
    {
        var query = new GetIncomeExpenseQuery(0, IncomeExpenseTypes.Expense, new QueryParam
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

        model.CustomerRefId = CustomerId;

        await GetIncomeExpenses(_filter);

        await base.OnInitializedAsync();
    }



    async Task HandleFormSubmit()
    {
        var comamnd = new AddExpenseCommand(model);

        var rsp = await mediator.Send(comamnd);

        if (rsp.IsError)
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, ExpenseMessage.Created);

        DialogService.Close(true);

    }

    private void HandleClose(MouseEventArgs args)
    {
        DialogService.Close(false);

    }
}