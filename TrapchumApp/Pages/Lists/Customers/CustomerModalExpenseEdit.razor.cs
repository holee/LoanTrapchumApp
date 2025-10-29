using Applications.Features.Customers.Commands.ExpenseCommands.EditExpensCommands;
using Applications.Features.Customers.Commands.ExpenseCommands.Queries.GetExpenseByCustomerId;
using Applications.Features.Customers.Commands.Expenses.EditExpenseCommands;
using Applications.Features.Customers.Commands.Expenses.Mapper;
namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerModalExpenseEdit
    {

        [Parameter] public long Id { get; set; } = 0;
        [Parameter] public string CustomerId { get; set; } = string.Empty;

        private ExpenseDtoUpdate model = new();

        protected override async Task OnInitializedAsync()
        {
            if(Id > 0 && !string.IsNullOrEmpty(CustomerId))
            {
                var query = new GetExpenseByIdQuery(Id, CustomerId);

                var resp = await mediator.Send(query);

                if (resp.IsError)
                {
                    model = new();

                    NotificationService.Notify(NotificationSeverity.Error, "Error", resp.FirstError.Description);
                }
                else
                {
                   model = resp.Value.ToExpenseEdit();
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
                NotificationService.Notify(NotificationSeverity.Error,rsp.FirstError.Description);
                return;
            }
            
            DialogService.Close(true);

        }

        private void HandleClose(MouseEventArgs args)
        {
            DialogService.Close(false);
            
        }
    }
}