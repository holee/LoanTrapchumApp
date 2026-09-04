using Applications.Features.Settings.Dto;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.UI.Pages.Settings.AppSettings;
using Web.UI.Services;

namespace Web.UI.Pages.Settings
{
    public partial class IndexSetting
    {
        [Inject] private ISettingApi SettingApi { get; set; } = default!;

        private List<AppSettingDto> _settings = [];

        private RadzenDataGrid<AppSettingDto> _settingsGrid = default!;

        private string? _filter = string.Empty;

        private bool _isLoading;

        protected override async Task OnInitializedAsync()
        {
            await LoadSettings(_filter);

            await base.OnInitializedAsync();
        }

        private async Task LoadSettings(string? filter)
        {
            _isLoading = true;

            try
            {
                var items = await SettingApi.GetAppSettingsAsync(filter);

                _settings = items ?? [];
            }
            catch (Exception ex)
            {
                _settings = [];

                NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task TextChanged(string? value)
        {
            _filter = value;

            await LoadSettings(_filter);
        }

        private async Task AddAppSetting()
        {
            var result = await DialogService.OpenAsync<AppSettingCreate>("Add Setting",
                options: new DialogOptions { AutoFocusFirstElement = false });

            if (result is AppSettingDto)
            {
                await LoadSettings(_filter);
            }
        }

        private async Task EditAppSetting(AppSettingDto item)
        {
            var result = await DialogService.OpenAsync<AppSettingEdit>("Edit Setting",
                new Dictionary<string, object?> { { nameof(AppSettingEdit.Item), item } },
                new DialogOptions { AutoFocusFirstElement = false });

            if (result is AppSettingDto)
            {
                await LoadSettings(_filter);
            }
        }

        private async Task DeleteAppSetting(AppSettingDto item)
        {
            var confirmed = await DialogService.Confirm($"Delete setting '{item.Code}'?", "Confirm Delete",
                new ConfirmOptions { OkButtonText = "Delete", CancelButtonText = "Cancel" });

            if (confirmed != true)
            {
                return;
            }

            try
            {
                await SettingApi.DeleteAppSettingAsync(item.Id);

                NotificationService.Notify(NotificationSeverity.Success, "Deleted", $"Setting '{item.Code}' was deleted.");

                await LoadSettings(_filter);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
            }
        }
    }
}
