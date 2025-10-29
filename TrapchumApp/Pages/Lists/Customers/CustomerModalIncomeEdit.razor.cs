using Applications.Commons.Messages.Incomes;
using Applications.Features.Customers.Commands.Incomes.Dto;
using Applications.Features.Customers.Commands.Incomes.EditIncome;
using Applications.Features.Customers.Commands.Incomes.Mappers;
using Applications.Features.Customers.Commands.Incomes.Queries.GetEditIncome;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerModalIncomeEdit
    {

        [Parameter] public long Id { get; set; } = 0;
        [Parameter] public string CustomerId { get; set; } = string.Empty;
        [Parameter] public EventCallback<bool> OnClose { get; set; }

        private EditIncomeDto model = new EditIncomeDto();

        protected override async Task OnInitializedAsync()
        {
            var query = new GetEditIncomeCommand(Id, CustomerId);

            var resp = await mediator.Send(query);

            if (resp.IsError)
            {
                model = new();

                NotificationService.Notify(NotificationSeverity.Error, "Error", resp.FirstError.Description);
            }
            else
            {
                model = resp.Value.ToIncomeEdit();
            }

            await base.OnInitializedAsync();
        }

        async Task HandleFormSubmit()
        {

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