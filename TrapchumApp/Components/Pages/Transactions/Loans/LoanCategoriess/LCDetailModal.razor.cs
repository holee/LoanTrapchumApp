//using Applications.Requests.Lists.Holidays;
//using Applications.Responses.Loans.Disbursements;
//using Microsoft.AspNetCore.Components;

//namespace TCLMS.Front.Pages.Transactions.Loans.LoanCategoriess
//{
//    public partial class LCDetailModal
//    {
//        [Parameter] public EventCallback Closed { get; set; }
//        void DateChangedEvt(DateTime? d)
//        {
//            Console.WriteLine(d!.Value);
//        }
//        private async Task CloseTab()
//        {
//            await Closed.InvokeAsync();
//        }

//        void OnSelectRow(HolidayDto holiday)
//        {
//            if (holiday != null)
//            {
//                SelectedDate = holiday.HolidayName;
//                modelDTO.Id = holiday.Id;
//                modelDTO.HolidayName = holiday.HolidayName;
//                modelDTO.Description = holiday.Description;
//                modelDTO.Status = holiday.Status;
//            }
//        }
//        async void ChangedDate(DateTime dt)
//        {
//            Console.WriteLine("Change date...:" + dt.ToLongDateString());
//            if (dt.Month != currentdt.Month)
//            {
//                await GetHolidays(dt);
//                modelDTO = new();
//                currentdt = dt;
//            }
//            StateHasChanged();

//        }
//        [Parameter] public int Id { get; set; }
//        //[Inject] private ILoanCategoryService? service { get; set; }
//        [Parameter] public LoanCategoryResponse? category { get; set; }
   
//    }
//}