using Applications.Extensions.DateTimes;
using Applications.Features.Holidays;
using Applications.Features.Holidays;
using Applications.Responses.ViewModels.Holidays;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Pages.Lists.Holidays
{
    public partial class CreateHolidayModal
    {

        private readonly IHolidayServiceImpl _holidayService;

        public CreateHolidayModal(IHolidayServiceImpl holidayService)
        {
            _holidayService = holidayService;
        }

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
            catch(Exception ex)
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

                var rsp = await _holidayService.CreateOrUpdateHoliday(modelDTO);

                if (rsp.IsSuccess)
                {

                    await GetHolidays(modelDTO.HolidayDt);

                    NotificationService.Notify(NotificationSeverity.Success, "Holiday were added.");

                    modelDTO = new();

                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);

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
                var rsp = await _holidayService.GetHolidays(dt);

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
                    var deleteResponse = await _holidayService!.DeleteHoliday(id);

                    if (deleteResponse.IsSuccess)
                    {
                        await GetHolidays(SelectedDate);
                        NotificationService.Notify(NotificationSeverity.Success, deleteResponse.Data);
                    }
                    else
                    {
                        NotificationService.Notify(NotificationSeverity.Error, "Can not delete");
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