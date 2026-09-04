using Applications.Extensions;
using Applications.Features.Holidays.Command.CreateHolidays;
using Applications.Features.Holidays.Dtos;

namespace Web.UI.Pages.Lists.Holidays
{
    public partial class CreateMultipleModal
    {

        [Parameter] public EventCallback Closed { get; set; }


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

                List<HolidayCreateDto> model = [];

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

                var command = new CreateHolidaysCommand(model);

                var rsp = await mediator.Send(command);

                if (!rsp.IsError)
                {

                    NotificationService.Notify(NotificationSeverity.Success, "Holiday were added.");

                    visibled = false;

                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());

                    visibled = false;

                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);

                visibled = false;

            }

        }

    }
}
