using Applications.Features.Settings.Commands.UserLoanSizes.CreateUserLoanSize;
using Applications.Features.Settings.Dto;

namespace TrapchumApp.Pages.Identity
{
    public partial class SetUserLoanSize
    {
        [Parameter, EditorRequired] public string UserId { get; set; } = string.Empty;

        private UserLoanSizeDtoCreate model = new();

        protected override void OnParametersSet()
        {
            model.UserRefId = UserId;
            model.StartDate = DateTime.Now;
        }

        private void StartDateChanged(DateTime? date)
        {
            if (date.HasValue && (model.EndDate is null || model.EndDate < date))
                model.EndDate = date.Value.AddYears(1);
        }

        private async Task HandleFormSubmit()
        {
            var rsp = await mediator.Send(new CreateUserLoanSizeCommand(model));

            if (rsp.IsError)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", rsp.FirstError.Description);
                return;
            }

            NotificationService.Notify(NotificationSeverity.Success, "Created", "Loan size saved.");
            DialogService.Close(true);
        }
    }
}
