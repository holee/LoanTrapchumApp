using Applications.Features.AdditionalInformations.Dto;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Services;

namespace Web.UI.Pages.Settings.Occupatiion
{
    public partial class OccupationDetail
    {
        [Inject] private ISettingApi SettingApi { get; set; } = default!;

        [Parameter] public int Id { get; set; }

        private OccupationDto? _occupation;

        private bool _isPageLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadOccupation();

            await base.OnInitializedAsync();
        }

        private async Task LoadOccupation()
        {
            _isPageLoading = true;

            try
            {
                _occupation = await SettingApi.GetOccupationByIdAsync(Id);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
            }
            finally
            {
                _isPageLoading = false;
            }
        }

        private async Task OnEditClick()
        {
            var result = await DialogService.OpenAsync<OccupationEdit>("Edit Occupation",
                new Dictionary<string, object?> { { nameof(OccupationEdit.Id), Id } },
                new DialogOptions { AutoFocusFirstElement = false });

            if (result is OccupationDto updated)
            {
                _occupation = updated;
            }
        }
    }
}
