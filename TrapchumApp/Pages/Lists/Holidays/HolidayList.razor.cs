using Applications.Features.Holidays.Command.DeleteHoliday;
using Applications.Features.Holidays.Queries.GetHolidaysAsPaging;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Holidays;
using Domains.Models.Lists;
using Radzen;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Lists.Holidays
{
    public partial class HolidayList
    {
        protected string? searchField = string.Empty;

        protected int holiid;

        protected bool isActive = false;

        public bool IsPreviousSelected = false;

        protected List<HolidayVm>? HolidaysList = [];

        int pageIndex = 0;

        int pageSize = 10;

        bool sortDescending = false;

        string sortColumn = "HolidayDt";

        int counts = 0;

        public Holiday holidayvm { get; set; } = default!;

        protected async void PreviousSelectedChanged()
        {
            await GetsHolidays(searchField);
        }

        protected async override Task OnInitializedAsync()
        {
            await GetsHolidays(searchField);

            await base.OnInitializedAsync();
        }
        protected async Task GetsHolidays(string? q = null)
        {
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, IsPreviousSelected, sortDescending, q);

            var gridItems = await mediator.Send(new GetHolidaysAsPagingQuery(param));// _holidayService.GetHolidays(param);

            HolidaysList = gridItems.Items;

            counts = gridItems.Count;

            isLoading = false;

            StateHasChanged();
        }
        protected void DisplayModal()
        {
            // visibleAdd = true;
            // SelectedIndex = 1;
            var newKey = Guid.NewGuid().ToString("N");

            RenderFragment content = (builder) =>
            {
                builder.OpenComponent(0, typeof(CreateHolidayModal));
                builder.AddAttribute(1, "Closed", EventCallback.Factory.Create(this, () =>
                {
                    CloseAddTab(newKey);
                }));
                builder.SetKey(newKey);
                builder.CloseComponent();
            };

            var newTab = new CustomTab(newKey, "Create Holiday", TabActions.Create, content);

            _tabs.Add(newTab);

            SelectedIndex = _tabs.Count;
        }

        private void CloseAddTab(string key)
        {
            var tabToRemove = _tabs.FirstOrDefault(t => t.Key == key);

            if (tabToRemove != null)
            {
                _tabs.Remove(tabToRemove);

                SelectedIndex = _tabs.Count;
            }
        }


        protected async void OnSelectChange()
        {
            await GetsHolidays(searchField);
        }
        protected async void SearchEvent(string e)
        {
            searchField = e ?? string.Empty;

            await GetsHolidays(searchField);
        }


        protected async void SelectedPage(int page)
        {
            await GetsHolidays(searchField);
        }

        protected async Task ShowEdit(long id)
        {
            var param = new Dictionary<string, object?>
            {
                {"id", id}
            };

            var options = new DialogOptions()
            {
                ShowClose = true,
                Width = "75rem",
                Height = "35rempx",
                Resizable = true,
                Draggable = true,
            };
            await DialogService.OpenAsync<UpdateHolidayModal>("Holiday", parameters: param, options);

            await GetsHolidays(searchField);

        }
        protected async Task Delete(long id)
        {
            ContextMenuService.Close();

            var comfirm = await DialogService.Confirm("Holiday deleted will not be able to recover, would you like to continue?",
                "Delete Holiday",
                     new ConfirmOptions
                     {
                         OkButtonText = "OK",
                         CancelButtonText = "Cancel",
                         CloseDialogOnEsc = true,

                     });
            if (comfirm == true)
            {

                var command = new DeleteHolidayCommand(id);


                var rsp = await mediator.Send(command);// _holidayService!.DeleteHoliday(id);

                if (rsp.IsSuccess())
                {
                    await OnInitializedAsync();
                    NotificationService.Notify(NotificationSeverity.Success, rsp.Value.Message);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
                }
                StateHasChanged();

            }
            else
            {

                NotificationService.Notify(NotificationSeverity.Warning, "Delete was cancel");
            }
        }


    }
}