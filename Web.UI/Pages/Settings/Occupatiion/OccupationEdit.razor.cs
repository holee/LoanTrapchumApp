using Applications.Features.AdditionalInformations.Dto;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Services;

namespace Web.UI.Pages.Settings.Occupatiion
{
    public partial class OccupationEdit
    {
        [Inject] private ISettingApi SettingApi { get; set; } = default!;

        [Parameter] public int Id { get; set; }

        private OccupationDtoUpate model = new();

        private bool _isBusy;

        protected override async Task OnInitializedAsync()
        {
            await LoadOccupation();

            await base.OnInitializedAsync();
        }

        private async Task LoadOccupation()
        {
            try
            {
                var item = await SettingApi.GetOccupationByIdAsync(Id);

                if (item is not null)
                {
                    model = new OccupationDtoUpate
                    {
                        OccupationId = item.OccupationId,
                        OccupationTitle = item.OccupationTitle,
                        Khmer = item.Khmer,
                        TypeOf = item.TypeOf
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
                var updated = await SettingApi.UpdateOccupationAsync(model.OccupationId, model);

                NotificationService.Notify(NotificationSeverity.Success, "Success", "Occupation was updated successfully.");

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
