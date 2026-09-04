using Applications.Features.Holidays.Command.UpdateHoliday;
using Applications.Features.Holidays.Dtos;
using Applications.Features.Holidays.Queries.GetUpdateHoliday;
using Applications.Interfaces.Lists;

namespace Web.UI.Pages.Lists.Holidays
{
    public partial class UpdateHolidayModal
    {
        [Parameter] public long id { get; set; }

        private readonly IHolidayService _holidayService;

        IReadOnlyList<DateTime?> selectedDates = [];

        private HolidayCreateDto modelDTO = new();

        private IEnumerable<HolidayDto> listvm = [];


        void ChangedDate(DateTime dt)
        {
            // await GetHolidays(dt);
            StateHasChanged();
        }
        private async void SelectedDateChanged()
        {
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



        protected HolidayDtoUpdate model = new HolidayDtoUpdate();
        private void Close() => DialogService.Close(false);
        protected async Task FormValidSubmit()
        {
            try
            {

                var command = new UpdateHolidayCommand(model);

                var rsp= await mediator.Send(command);

                if (rsp.IsSuccess())
                {
                    model = new();
                    NotificationService.Notify(NotificationSeverity.Success,"Holiday was updated successfully");
                    Close();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error,rsp.GetMessage());
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"{ex.Message}");
            }
        }

        private void RowSelectedChanged(HolidayDto arg)
        {
            if (arg != null)
            {
                model.Id = arg.Id;
                model.HolidayDt = arg.HolidayDt;
                model.HolidayNum = arg.HolidayNum;
                model.HolidayName = arg.HolidayName;
                model.Description = arg.Description;
                model.Status = arg.Status;
            }
        }
        protected override async Task OnInitializedAsync()
        {

            var command = new GetUpdateHolidayByIdQuery(id);

            var rsp = await mediator.Send(command);  //_holidayService.GetHoliday(id);

            if (rsp.IsSuccess())
            {
                model = rsp.Value;

                listvm = await _holidayService.GetHolidays(model.HolidayDt);

            }

            await base.OnInitializedAsync();

        }


    }
}