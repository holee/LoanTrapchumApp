using Applications.Features.Periods.Dto;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Services;

namespace Web.UI.Pages.Settings.Periods
{
    public partial class PeriodEdit
    {
        [Inject] private ISettingApi SettingApi { get; set; } = default!;

        [Parameter] public int Id { get; set; }

        private PeriodDtoUpdate _model = new();

        private bool _isBusy;

        protected override async Task OnInitializedAsync()
        {
            await LoadPeriod();

            await base.OnInitializedAsync();
        }

        private async Task LoadPeriod()
        {
            try
            {
                var pager = await SettingApi.GetPeriodsAsync(0, 1000, nameof(PeriodDto.Id), false, null);

                var item = pager.Items?.FirstOrDefault(x => x.Id == Id);

                if (item is not null)
                {
                    _model = new PeriodDtoUpdate
                    {
                        Id = item.Id,
                        Period = item.Period
                    };
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
            }
        }

        private async Task OnSubmit()
        {
            _isBusy = true;

            try
            {
                var updated = await SettingApi.UpdatePeriodAsync(_model.Id, _model);

                NotificationService.Notify(NotificationSeverity.Success, "Success", "Period was updated successfully.");

                DialogService.Close(updated);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
            }
            finally
            {
                _isBusy = false;
            }
        }
    }
}
