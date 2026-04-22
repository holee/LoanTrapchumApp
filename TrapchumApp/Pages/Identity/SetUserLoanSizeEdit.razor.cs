using Applications.Features.Settings.Commands.UserLoanSizes.UpdateUserLoanSize;
using Applications.Features.Settings.Dto;
using Applications.Responses.ViewModels.Settings;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Identity
{
    public partial class SetUserLoanSizeEdit
    {
        [Parameter] public VUserLoanSizes Item { get; set; } = default!;

        private UserLoanSizeDtoUpdate model = default!;

        protected override void OnParametersSet()
        {
            model = new UserLoanSizeDtoUpdate
            {
                Id             = Item.Id!.Value,
                UserRefId      = Item.UserRefId,
                MinUsdLoanSize = Item.MinUsdLoanSize,
                MaxUsdLoanSize = Item.MaxUsdLoanSize,
                MinKhrLoanSize = Item.MinKhrLoanSize,
                MaxKhrLoanSize = Item.MaxKhrLoanSize,
                StartDate      = Item.StartDate,
                EndDate        = Item.EndDate,
                Description    = Item.Description,
                IsActive       = Item.IsActive,
                EditSeq        = Item.EditSeq
            };
        }

        private void StartDateChanged(DateTime? date)
        {
            if (date.HasValue && (model.EndDate is null || model.EndDate < date))
                model.EndDate = date.Value.AddYears(1);
        }

        private async Task HandleFormSubmit()
        {
            var rsp = await mediator.Send(new UpdateUserLoanSizeCommand(model));

            if (rsp.IsError)
            {
                await DialogService.Alert(rsp.FirstError.Description, "Error",
                    new ConfirmOptions { OkButtonText = "OK" });
                return;
            }

            NotificationService.Notify(NotificationSeverity.Success, "Updated", "Loan size updated.");
            DialogService.Close(true);
        }
    }
}
