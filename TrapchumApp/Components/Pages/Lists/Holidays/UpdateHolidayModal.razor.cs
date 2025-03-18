using Microsoft.AspNetCore.Components;
using TCLMS.Shared.Entities.Dtos.Lists.Holidays;

namespace TCLMS.Front.Pages.Lists.Holidays
{
    public partial class UpdateHolidayModal
    {
        [Parameter] public long id { get; set; }

        protected HolidayUpdatesDto model = new HolidayUpdatesDto();
        private void Close() => DialogService.Close(false);
        protected async Task FormValidSubmit()
        {
            //try
            //{
            //    var response = await _holidayService.ModifyHoliday(id, model);
            //    if (response.IsSuccess)
            //    {
            //        model = new();
            //        NotificationService.Notify(Radzen.NotificationSeverity.Success, response.Message);
            //        Close();
            //    }
            //    else
            //    {
            //        NotificationService.Notify(Radzen.NotificationSeverity.Error, response.Message);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    NotificationService.Notify(Radzen.NotificationSeverity.Error, $"{ex.Message}");
            //}
        }
        protected override async Task OnInitializedAsync()
        {
            model = await _holidayService.GetHoliday(id);
            await base.OnInitializedAsync();

        }


    }
}