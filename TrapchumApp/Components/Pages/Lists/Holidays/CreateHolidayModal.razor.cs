using Applications.Extensions.mappers;
using Applications.Requests.Lists.Holidays;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;
namespace TrapchumApp.Components.Pages.Lists.Holidays
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
        private IEnumerable<HolidayDto> listvm = new List<HolidayDto>();

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
        protected override async Task OnParametersSetAsync()
        {

            await base.OnParametersSetAsync();
        }
        void MultiYear()
        {

            if (IsMultiYear == true)
            {
                IsDisible = false;
            }
            else
            {
                IsDisible = true;
                return;
            }

        }
        private void SelectedDateChanged()
        {
            modelDTO.Status = true;

            if (listvm.Any())
            {

                var hol = listvm!.Select(x => new HolidayCreateDto
                {
                    HolidayName = x.HolidayName,
                    Status = x.Status,
                    Description = x.Description,
                    Id = x.Id
                }).FirstOrDefault(x => x.HolidayName!.Value.Date == SelectedDate!.Value.Date);

                if (hol == null)
                {
                    modelDTO = new();
                }
                else
                {
                    modelDTO = hol;
                }
                StateHasChanged();
            }
            else
            {
                listvm = [];
                modelDTO.Description = "";
            }

        }
        protected async Task FormValidSubmit()
        {
            try
            {
                visibled = true;
                if (IsMultiYear == true)
                {
                    if (endYear > 20)
                    {
                        NotificationService.Notify(Radzen.NotificationSeverity.Warning, "Over Year", "Create Multi-Year support between 1 to 20 years");
                        return;
                    }
                    Year = SelectedDate!.Value;
                    for (int i = 1; i <= endYear; i++)
                    {
                        Year = Year.AddYears(1);
                        Years!.Add(Year);
                        modelDTO!.HolidayName = Year;
                        var ex = await _holidayService.CreateHoliday(modelDTO);

                    }
                    IsMultiYear = false;
                    endYear = null;
                    IsDisible = true;

                }
                modelDTO!.HolidayName = SelectedDate;
                var rsp = await _holidayService.CreateHoliday(modelDTO);
                if (rsp.IsSuccess == true)
                {

                    await GetHolidays(modelDTO.HolidayName);
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, rsp.Data);
                    modelDTO = new();
                    visibled = false;
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, rsp.ErrMessage);
                    modelDTO = new();
                    visibled = false;

                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);
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
                    listvm = rsp.Maps<HolidayDto>();
                }

            }
            catch (Exception e)
            {
                listvm = new List<HolidayDto>();
                Console.WriteLine(e.Message);
            }
        }
        async Task DeleteHoliday(long id)
        {
            var confitm = await DialogService!.Confirm("Holiday deleted will not be able to recover, would you like to continue?",
                "Delete Holiday",
                new Radzen.ConfirmOptions
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
                        NotificationService.Notify(Radzen.NotificationSeverity.Success, deleteResponse.Data);
                    }
                    else
                    {
                        NotificationService.Notify(Radzen.NotificationSeverity.Error, "Can not delete");
                    }
                    StateHasChanged();

                }
                catch (Exception ex)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.InnerException!.Message);
                }

            }
        }


        [Parameter] public EventCallback Closed { get; set; }
        void DateChangedEvt(DateTime? d)
        {
            Console.WriteLine(d!.Value);
        }
        private async Task CloseTab()
        {
            await Closed.InvokeAsync();
        }

        void OnSelectRow(HolidayDto holiday)
        {
            if (holiday != null)
            {
                SelectedDate = holiday.HolidayName;
                modelDTO.Id = holiday.Id;
                modelDTO.HolidayName = holiday.HolidayName;
                modelDTO.Description = holiday.Description;
                modelDTO.Status = holiday.Status;
            }
        }
        async void ChangedDate(DateTime dt)
        {
            Console.WriteLine("Change date...:" + dt.ToLongDateString());
            if (dt.Month != currentdt.Month)
            {
                await GetHolidays(dt);
                modelDTO = new();
                currentdt = dt;
            }
            StateHasChanged();

        }
    }
}