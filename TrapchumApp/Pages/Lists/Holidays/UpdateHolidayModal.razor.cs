using Applications.Features.Holidays;
using Applications.Interfaces.Lists;
using Applications.Responses.ViewModels.Holidays;
using Applications.Services.Lists;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Lists.Holidays
{
    public partial class UpdateHolidayModal
    {
        [Parameter] public long id { get; set; }

        private readonly IHolidayService _holidayService;

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
        public UpdateHolidayModal(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }



        protected HolidayCreateDto model = new HolidayUpdatesDto();
        private void Close() => DialogService.Close(false);
        protected async Task FormValidSubmit()
        {
            try
            {
                var data=new Holiday
                {
                    Id = model.Id,
                    HolidayDt = model.HolidayDt,
                    HolidayNum = (int)model.HolidayNum,
                    HolidayName = model.HolidayName,
                    Description = model.Description,
                    Status = model.Status
                };
                var response = await _holidayService.CreateOrUpdateHoliday(data);

                if (response.IsSuccess)
                {
                    model = new();

                    NotificationService.Notify(NotificationSeverity.Success, response.Data);

                    Close();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"{ex.Message}");
            }
        }

        private void RowSelectedChanged(HolidayVm arg)
        {
            if (arg != null)
            {
                model.Id = arg.Id;
                model.HolidayDt = arg!.HolidayDt;
                model.HolidayNum = arg!.HolidayNum;
                model.HolidayName = arg!.HolidayName;
                model.Description = arg!.Description;
                model.Status = arg.Status;
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