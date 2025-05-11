using Applications.Responses.Lists.Holidays;

namespace TrapchumApp.Components.Pages.Lists.Holidays
{
    public partial class UpdateHolidayModal
    {
        IReadOnlyList<DateTime?> selectedDates = [];
        private IEnumerable<HolidayDto> listvm = new List<HolidayDto>();

        private async void SelectedDateChanged()
        {
            if (selectedDates.Any())
            {
                // SelectedDate = selectedDates.OrderByDescending(x => x).FirstOrDefault();
            }
            else
            {
                // SelectedDate = null;
            }

            // await GetHolidays(SelectedDate);

            if (listvm.Any())
            {
                selectedDates = listvm.Select(x => x.HolidayName).ToList();
            }
            else
            {
                selectedDates.ToList().Clear();
            }

            await Task.CompletedTask;

        }
    }
}