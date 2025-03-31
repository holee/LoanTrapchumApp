using Applications.Requests.Lists.Holidays;
using Applications.Services.Lists;
using Mapster;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Lists.Holidays
{
    public partial class UpdateHolidayModal
    {

        private readonly IHolidayServiceImpl _holidayService;

        public UpdateHolidayModal(IHolidayServiceImpl holidayService)
        {
            _holidayService = holidayService;
        }

        [Parameter] public long id { get; set; }

        protected HolidayUpdatesDto model = new HolidayUpdatesDto();
        private void Close() => DialogService.Close(false);
        protected async Task FormValidSubmit()
        {
            try
            {
                var response = await _holidayService.ModifyHolidate(id, model);

                if (response.IsSuccess)
                {
                    model = new();
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, response.Data);
                    Close();
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, response.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, $"{ex.Message}");
            }
        }
        protected override async Task OnInitializedAsync()
        {

            var rsp = await _holidayService.GetHoliday(id);

            if (rsp is not null)
            {
                model = rsp.Adapt<HolidayUpdatesDto>();
            }

            await base.OnInitializedAsync();

        }


    }
}