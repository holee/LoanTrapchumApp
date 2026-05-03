using Applications.Features.Periods.Commands.UpdatePeriod;
using Applications.Features.Periods.Dto;
using Applications.Features.Periods.Queries.GetPeriods;

namespace TrapchumApp.Pages.Settings.Periods
{
    public partial class PeriodEdit
    {
        [Parameter] public int Id { get; set; }

        private PeriodDtoUpdate _model = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadPeriod();
            await base.OnInitializedAsync();
        }

        private async Task LoadPeriod()
        {
            var param = new QueryParam(0, 1, nameof(PeriodDto.Id));
            var result = await mediator.Send(new GetPeriodsQuery(param));
            var item = result.Items?.FirstOrDefault(x => x.Id == Id);
            if (item is not null)
            {
                _model = new PeriodDtoUpdate { Id = item.Id, Period = item.Period };
            }
        }

        private async Task OnSubmit(PeriodDtoUpdate model)
        {
            var rsp = await mediator.Send(new UpdatePeriodCommand(_model));

            if (rsp.IsError)
            {
                await DialogService.Alert(rsp.FirstError.Description, "Error", new ConfirmOptions { OkButtonText = "OK" });
                return;
            }

            NotificationService.Notify(NotificationSeverity.Success, "Updated", $"Period {rsp.Value.Period} updated successfully.");
            DialogService.Close(rsp.Value);
        }
    }
}
