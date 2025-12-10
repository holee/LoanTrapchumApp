using Applications.Extensions.DateTimes;
using Applications.Features.Holidays;
using Applications.Features.Holidays.Command.CreateHoliday;
using Applications.Features.Holidays.Command.DeleteHoliday;
using Applications.Features.Holidays.Queries.GetHolidays;
using Applications.Responses.ViewModels.Holidays;
using Applications.Services.Lists;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Lists.Holidays
{
    public partial class CreateHolidayModal
    {
        [Parameter] public EventCallback OnClose { get; set; }
        private List<HolidayCreateDto> model = new List<HolidayCreateDto>();
        private HolidayCreateDto modelDTO = new();
        private IEnumerable<HolidayVm> listvm = [];
        DateTime currentdt = DateTime.Now;
        bool IsMultiYear = false;
        bool IsDisible = true;
        bool visibled = false;
        //DateTime? selectedDates=DateTime.Now;
        protected DateTime? SelectedDate;
        protected List<DateTime?>? Years = [];
        protected DateTime Year = DateTime.Now;
        protected int? endYear = null;


        protected override async Task OnInitializedAsync()
        {
            SelectedDate = DateTime.Now;
            currentdt = DateTime.Now;
            await GetHolidays(DateTime.Now);
            StateHasChanged();
            await base.OnInitializedAsync();
        }

        private void SelectedDateChanged(DateTime? arg)
        {
            try
            {
                var result = listvm.FirstOrDefault(x => x.HolidayDt == arg);
                Console.WriteLine($"Selected Date: {result?.HolidayDt}");
                if (result == null || result.Id <= 0)
                {
                    modelDTO.Id = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        protected async Task FormValidSubmit()
        {
            try
            {
                visibled = true;

                modelDTO!.HolidayDt = SelectedDate;

                modelDTO.HolidayName = SelectedDate.DayName();

                modelDTO.HolidayNum = SelectedDate.DayNum();

                var command = new CreateHolidayCommand(modelDTO);

                var rsp = await mediator.Send(command);

                if (rsp.IsSuccess())
                {

                    await GetHolidays(modelDTO.HolidayDt);

                    NotificationService.Notify(NotificationSeverity.Success, "Holiday were added.");

                    modelDTO = new();

                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());

                    modelDTO = new();

                    visibled = false;

                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
                visibled = false;

            }

        }
        private async Task GetHolidays(DateTime? dt)
        {
            try
            {

                var query = new GetHolidaysQuery(dt ?? DateTime.Now);

                var rsp = await mediator.Send(query);

                if (rsp.Any())
                {
                    listvm = rsp;
                }
                else
                {
                    listvm = [];
                }

                //StateHasChanged();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        async Task DeleteHoliday(long id)
        {
            var confitm = await DialogService!.Confirm("Holiday deleted will not be able to recover, would you like to continue?",
                "Delete Holiday",
                options: new ConfirmOptions
                {
                    CancelButtonText = "Cancel",
                    OkButtonText = "OK"
                });
            if (confitm == true)
            {
                try
                {

                    var command = new DeleteHolidayCommand(id);

                    var rsp = await mediator.Send(command); // _holidayService!.DeleteHoliday(id);

                    if (rsp.IsSuccess())
                    {
                        await GetHolidays(SelectedDate);

                        NotificationService.Notify(NotificationSeverity.Success, rsp.Value.Message);
                    }
                    else
                    {
                        NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
                    }
                    StateHasChanged();

                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, ex.InnerException!.Message);
                }

            }
        }


        [Parameter] public EventCallback Closed { get; set; }

        private async Task CloseTab()
        {
            await Closed.InvokeAsync();
        }

        async void ChangedDate(DateTime dt)
        {
            await GetHolidays(dt);
            StateHasChanged();
        }
        private void RowSelectedChanged(HolidayVm arg)
        {
            if (arg != null)
            {
                modelDTO.Id = arg.Id;
                SelectedDate = arg!.HolidayDt;
                modelDTO.HolidayName = arg!.HolidayName;
                modelDTO.Description = arg!.Description;
                modelDTO.Status = arg.Status;
            }
        }
        private async Task ShowMultipleModal(MouseEventArgs args)
        {
            var options = new DialogOptions()
            {
                Draggable = true
            };

            bool? result = await DialogService.OpenAsync<CreateMultipleModal>("Create Multiple Holidays", options: options);

            if (result is not null && result == true)
            {
                await GetHolidays(SelectedDate);
            }
        }
    }
}