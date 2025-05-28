using Applications.Requests.Lists.Holidays;
using Applications.Responses.Lists.Holidays;
using Applications.Responses.ViewModels.Holidays;
using Applications.Services.Lists;
using Mapster;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Lists.Holidays
{
    public partial class UpdateHolidayModal
    {
        [Parameter] public long id { get; set; }

        private readonly IHolidayServiceImpl _holidayService;

        IReadOnlyList<DateTime?> selectedDates = [];

        private HolidayCreateDto modelDTO = new();

        private IEnumerable<HolidayVm> listvm = [];


        void ChangedDate(DateTime dt)
        {
            // await GetHolidays(dt);
            StateHasChanged();
        }
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
                selectedDates = listvm.Select(x => x.HolidayDt).ToList();
            }
            else
            {
                selectedDates.ToList().Clear();
            }

            await Task.CompletedTask;

        }
        public UpdateHolidayModal(IHolidayServiceImpl holidayService)
        {
            _holidayService = holidayService;
        }



        protected HolidayCreateDto model = new HolidayUpdatesDto();
        private void Close() => DialogService.Close(false);
        protected async Task FormValidSubmit()
        {
            try
            {
                var response = await _holidayService.CreateOrUpdateHoliday(model);

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

        private void RowSelectedChanged(HolidayVm arg)
        {
            if (arg != null)
            {
                modelDTO.Id = arg.Id;
                model.HolidayDt = arg!.HolidayDt;
                modelDTO.HolidayName = arg!.HolidayName;
                modelDTO.Description = arg!.Description;
                modelDTO.Status = arg.Status;
            }
        }
        protected override async Task OnInitializedAsync()
        {

            var rsp = await _holidayService.GetHoliday(id);



            if (rsp.IsSuccess)
            {
                model = rsp.Data!.ToHolidayCreateOrEdit();

                listvm = await _holidayService.GetHolidays(model.HolidayDt);

            }

            await base.OnInitializedAsync();

        }


    }
}