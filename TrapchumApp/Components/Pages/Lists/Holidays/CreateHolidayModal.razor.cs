using Microsoft.AspNetCore.Components;
using TCLMS.Shared.Entities.Dtos.Lists.Holidays;
using TCLMS.Shared.Entities.Models.Lists.Employees;

namespace TCLMS.Front.Pages.Lists.Holidays
{
    public partial class CreateHolidayModal
    {


        private List<HolidayCreateDto> model = new List<HolidayCreateDto>();
        private HolidayCreateDto? modelDTO = new();
        private IEnumerable<HolidayDto> listvm = new List<HolidayDto>();

        DateTime currentdt = DateTime.Now;
        bool IsMultiYear=false;
        bool IsDisible = true;
        bool visibled = false;
        //DateTime? selectedDates=DateTime.Now;
        protected DateTime? SelectedDate;
        protected List<DateTime?>? Years=[];
        protected DateTime Year=DateTime.Now;
        protected int? endYear=null;
 
        
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
        private async void SelectedDateChanged()
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
                        NotificationService.Notify(Radzen.NotificationSeverity.Warning, "Over Year","Create Multi-Year support between 1 to 20 years");
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
                var e = await _holidayService.CreateHoliday(modelDTO);
                if (e.IsSuccess == true)
                {

                    await GetHolidays(modelDTO.HolidayName);
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, e.Data!.Message);
                    modelDTO = new();
                    visibled=false;
                }
                else
                {
                    var error = e.Errors!.FirstOrDefault();
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, error);
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
                listvm = await _holidayService.GetHolidays(dt);
                
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
                        NotificationService.Notify(Radzen.NotificationSeverity.Success, deleteResponse.Data!.Message);
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
    }
}