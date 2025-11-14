using Applications.Commons.Mappers.Incomes;
using Applications.Commons.Messages.Incomes;
using Applications.Enumerations;
using Applications.Features.Incomes.Command.CreateIncomeExpenseSource;
using Applications.Features.Incomes.Command.EditIncomeExpenseSource;
using Applications.Features.Incomes.Command.GetEditIncomeExpenseSource;
using Applications.Features.Incomes.Dto;
using Domains.Enumerations;

namespace TrapchumApp.Pages.Settings
{
    public partial class IncomeExpenseCreate
    {
        [Parameter] public int? Id { get; set; } = 0;
        private CreateIncomeExpenseDto _model = new();
        private string[] _types = [];
        private bool _isEdit = false;
        private async Task OnSubmit()
        {
            await CreateOrUpdate(_isEdit);
        }
        
        protected override async Task OnInitializedAsync()
        {

            _types = default(IncomeExpenseTypes).GetNamesExcluding<IncomeExpenseTypes>(IncomeExpenseTypes.All);
            
            if (Id is > 0)
            {
                await GetEditIncomeExpenseSource(Id);
                
                _isEdit = true;
            }
            
            await base.OnInitializedAsync();
        }

        async Task GetEditIncomeExpenseSource(int? id)
        {
            var query = new GetEditIncomeExpenseQuery(id);

            var handler = await mediator.Send(query);

            if (!handler.IsError)
            {
                _model = handler.Value.ToIncomeExpenseEdit();
            }

        }
        
        private async Task CreateOrUpdate(bool isUpdated)
        {

            if (isUpdated)
            {
                var command = new CreateIncomeExpenseCommand(_model);

                var handler = await mediator.Send(command);

                if (handler.IsError)
                {
                    await DialogService.Alert(handler.FirstError.Description, "Error", new() { OkButtonText = "OK" });
                }

                NotificationService.Notify(NotificationSeverity.Success, IncomeExpenseMessages.Created);

                DialogService.Close(true);
            }
            else
            {
                var command = new EditIncomeExpenseCommand(_model);

                var handler = await mediator.Send(command);

                if (handler.IsError)
                {
                    await DialogService.Alert(handler.FirstError.Description, "Error", new() { OkButtonText = "OK" });
                }

                NotificationService.Notify(NotificationSeverity.Success, IncomeExpenseMessages.Created);

                DialogService.Close(true);
            }
        }

        private void OnCancel()
        {
            DialogService.Close(false);
        }
    }
}