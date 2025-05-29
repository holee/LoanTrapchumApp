using Applications.Requests.Settings;
using Applications.Responses.ViewModels.Settings;
using Applications.Services.Settings;
using Mapster;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Settings
{
    public partial class IndexSetting
    {

        IEnumerable<AppSettingVms> Settings = [];
        List<AppSettingDtoCreate> SysSettingsToUpdate = [];
        RadzenDataGrid<AppSettingVms> settingsGrid = default!;
        DataGridEditMode editMode = DataGridEditMode.Single;
        private readonly ISettingServiceImpl _settingServiceImpl;
        public IndexSetting(ISettingServiceImpl settingServiceImpl)
        {
            _settingServiceImpl = settingServiceImpl;
        }

        protected override async Task OnInitializedAsync()
        {
            await GetSettings();
            await base.OnInitializedAsync();
        }


        void Reset()
        {
            SysSettingsToUpdate.Clear();
        }

        void Reset(AppSettingDtoCreate order)
        {
            SysSettingsToUpdate.Remove(order);
        }


        async Task GetSettings(string? filterValue = null)
        {
            var response = await _settingServiceImpl.GetSysSettings(filterValue);

            if (response.IsSuccess)
            {
                Settings = response.Data!.Adapt<IEnumerable<AppSettingVms>>();
            }
        }

        async void EditRow(AppSettingVms e)
        {
            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }
            //SysSettingsToUpdate.Add(e);
            //await settingsGrid.EditRow(e);
        }
        void OnUpdateRow(AppSettingVms e)
        {
            //Reset(e);

            // dbContext.Update(order);

            // dbContext.SaveChanges();
        }
        void CancelEdit(AppSettingVms e)
        {
            
            
            
            
            
            //Reset(e);

            //settingsGrid.CancelEditRow(e);

            // var orderEntry = dbContext.Entry(order);

            // if (orderEntry.State == EntityState.Modified)
            // {
            //     orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
            //     orderEntry.State = EntityState.Unchanged;
            // }
        }

    }
}