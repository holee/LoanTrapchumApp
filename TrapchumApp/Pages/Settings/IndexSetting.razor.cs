using Applications.Features.Settings.Commands.AppSettings.DeleteAppSetting;
using Applications.Features.Settings.Dto;
using Applications.Features.Settings.Queries.AppSettings.GetAppSettings;
using Radzen.Blazor;
using TrapchumApp.Pages.Settings.AppSettings;

namespace TrapchumApp.Pages.Settings
{
    public partial class IndexSetting
    {
        private List<AppSettingDto> _settings = [];

        private RadzenDataGrid<AppSettingDto> _settingsGrid = default!;

        private string? _filter;

        private bool _isLoading = false;
        protected override async Task OnInitializedAsync()
        {
            await LoadSettings();

            await base.OnInitializedAsync();
        }

        private async Task LoadSettings(string? filter = null)
        {
            Tracker.Show();

            _isLoading = true;

            var rsp = await mediator.Send(new GetAppSettingsQuery(filter));

            _settings = rsp.IsError ? [] : rsp.Value.ToList();

            Tracker.Hide();

            _isLoading = false;

            StateHasChanged();
        }

        private async Task TextChanged(string? filter)
        {
            _filter = filter;

            await LoadSettings(filter);
        }

        private async Task AddAppSetting()
        {
            var result = await DialogService.OpenAsync<AppSettingCreate>("Add Setting",
                options: new DialogOptions { AutoFocusFirstElement = false });

            if (result is AppSettingDto)
                await LoadSettings(_filter);
        }

        private async Task EditAppSetting(AppSettingDto item)
        {
            var result = await DialogService.OpenAsync<AppSettingEdit>("Edit Setting",
                new Dictionary<string, object?> { { nameof(AppSettingEdit.Item), item } },
                options: new DialogOptions { AutoFocusFirstElement = false });

            if (result is AppSettingDto)
                await LoadSettings(_filter);
        }

        private async Task DeleteAppSetting(AppSettingDto item)
        {
            var confirm = await DialogService.Confirm($"Delete setting '{item.Code}'?", "Confirm Delete",
                new ConfirmOptions { OkButtonText = "Delete", CancelButtonText = "Cancel" });

            if (confirm != true) return;

            var rsp = await mediator.Send(new DeleteAppSettingCommand(item.Id));

            if (rsp.IsError)
                await DialogService.Alert(rsp.FirstError.Description, "Error");
            else
            {
                NotificationService.Notify(NotificationSeverity.Success, "Deleted", $"Setting '{item.Code}' removed.");
                await LoadSettings(_filter);
            }
        }
    }
}
