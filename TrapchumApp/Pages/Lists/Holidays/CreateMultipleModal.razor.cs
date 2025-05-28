using Applications.Extensions.DateTimes;
using Applications.Requests.Lists.Holidays;
using Applications.Services.Lists;

namespace TrapchumApp.Components.Pages.Lists.Holidays
{
    public partial class CreateMultipleModal
    {
        private readonly IHolidayServiceImpl _holidayService;

        public CreateMultipleModal(IHolidayServiceImpl holidayService)
        {
            _holidayService = holidayService;
        }

        IReadOnlyList<int> years = [1, 5, 10, 15, 20];

        int selectedYear = 5;

        DateTime selectedDate = DateTime.Now;

        bool visibled = false;

        string Description = string.Empty;
        protected async Task FormValidSubmit()
        {
            try
            {
                visibled = true;

                ICollection<HolidayCreateDto> model = [];


                for (int i = 1; i <= selectedYear; i++)
                {
                    var newYear = selectedDate.AddYears(i);

                    var item = new HolidayCreateDto
                    {
                        HolidayName = newYear.DayName(),
                        HolidayDt = newYear,
                        HolidayNum = newYear.DayNum(),
                        Status = true,
                        Description = Description,
                    };
                    model.Add(item);
                }

                var rsp = await _holidayService.CreateHolidays(model);

                if (rsp.IsSuccess)
                {

                    NotificationService.Notify(Radzen.NotificationSeverity.Success, "Holiday were added.");

                    visibled = false;

                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, rsp.ErrMessage);

                    visibled = false;

                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);

                visibled = false;

            }

        }

    }
}
