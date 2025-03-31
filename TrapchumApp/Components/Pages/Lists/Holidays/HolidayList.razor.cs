using Applications.Interfaces.Lists;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Lists.Holidays
{
    public partial class HolidayList
    {
        private readonly IHolidayService _holidayService;

        public HolidayList(IHolidayService holidayService)
        {
            this._holidayService = holidayService;
        }

        protected string? searchField = string.Empty;
        protected int holiid;
        protected bool isActive = false;
        public bool IsPreviousSelected = false;
        protected List<Holiday>? HolidaysList = [];
        int pageIndex = 0;
        int pageSize = 10;
        bool sortDescending = false;
        string sortColumn = "Id";
        int counts = 0;
        public Holiday holidayvm { get; set; } = default!;
        List<int> SelectedRow = [10, 20, 50, 100];

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
            try
            {
                var gridItems = await _holidayService.GetHolidays(pageIndex, pageSize, sortColumn, IsPreviousSelected, sortDescending, q);
                HolidaysList = gridItems.Items;
                counts = gridItems.Count;
                isLoading = false;
            }
            catch (Exception ex)
            {
                HolidaysList = [];
                NotificationService.Notify(Radzen.NotificationSeverity.Success, $"{ex.InnerException!.Message}");

            }

            StateHasChanged();
        }
        protected void DisplayModal()
        {
            visibleAdd = true;
            SelectedIndex = 1;
        }
        protected async void OnSelectChange()
        {
            await GetsHolidays(searchField);
        }
        protected async void SearchEvent(ChangeEventArgs e)
        {
            searchField = e.Value!.ToString();
            await GetsHolidays(searchField);
        }


        protected async void SelectedPage(int page)
        {
            await GetsHolidays(searchField);
        }

        protected async Task ShowEdit(long id)
        {
            await DialogService.OpenAsync<UpdateHolidayModal>("New Holiday");

        }
        protected async Task Delete(long id)
        {
            var comfirm = await DialogService.Confirm("Holiday deleted will not be able to recover, would you like to continue?",
                "Delete Holiday",
                     new Radzen.ConfirmOptions
                     {
                         OkButtonText = "OK",
                         CancelButtonText = "Cancel",
                         CloseDialogOnEsc = true,

                     });
            if (comfirm == true)
            {
                var deleteResponse = await _holidayService!.DeleteHoliday(id);

                if (deleteResponse.IsSuccess)
                {
                    await OnInitializedAsync();
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, deleteResponse.Data);
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, deleteResponse.ErrMessage);
                }
                StateHasChanged();
                ContextMenuService.Close();
            }
            else
            {
                ContextMenuService.Close();
                
                NotificationService.Notify(Radzen.NotificationSeverity.Warning, "Delete was cancel");
            }
        }
        protected Task<bool> ValidateSuccess(bool success) => Task.FromResult(success);
        protected async Task CreateSuccess(bool param)
        {
            if (param)
            {
                await OnInitializedAsync();
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task UpdateSuccess(bool param)
        {
            if (param)
            {
                await OnInitializedAsync();
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task OnCheckChanged(long id)
        {

            holidayvm.Status = false;
            //var message = await _holidayService.ActivateOrDeactivat(id, holidayvm);
            //if (message!.IsSuccess)
            //{
            //    NotificationService.Notify(Radzen.NotificationSeverity.Success, message.Message);
            //}
            //else
            //    NotificationService.Notify(Radzen.NotificationSeverity.Error, message.Message);
            
            
            await Task.CompletedTask;
        }
    }
}