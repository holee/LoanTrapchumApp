using Applications.Responses.ViewModels.Loans.Schedules;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Repayments
{
    public partial class LateScheduleDetail
    {
        //private readonly ILoanPaymentService _loanPaymentService;
        //private readonly ISchedulesService _schedulesService;
        [Parameter] public long LoanId { get; set; }
        [Parameter] public int BranchId { get; set; }
        [Parameter] public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        IEnumerable<VSchedule> LateSchedules { get; set; } = [];
        bool allowRowSelectOnRowClick = true;
        RadzenDataGrid<VSchedule> grid = default!;
        IList<VSchedule>? SelectedSchedules;
        bool? isHeaderChecked = false;
        bool isRowChecked = false;

        //public LateScheduleDetail(ILoanPaymentService loanPaymentService, ISchedulesService schedulesService)
        //{
        //    _loanPaymentService = loanPaymentService;
        //    _schedulesService = schedulesService;
        //}

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //var response = await _schedulesService.GetLateScheduleLineDetai(LoanId, BranchId, PaymentDate);

            //if (response.IsSuccess)
            //{
            //    LateSchedules = response.Data!;
            //}
            //else
            //{
            //    LateSchedules = [];
            //}


        }

        void RowSelectedChanged(VSchedule schedule)
        {
            var selectedIndex = LateSchedules.ToList().IndexOf(schedule);

            Console.WriteLine(selectedIndex);

            if (selectedIndex > 0 && !IsPreviousRowSelected(selectedIndex - 1))
            {
                schedule.Selected = false;
                grid.SelectRow(schedule, false);
                NotificationService.Notify(NotificationSeverity.Error, "Previous schedule is not selected yet.");
                return;
            }

            schedule.Selected = true;
            //grid.SelectRow(schedule);
        }

        void RowDeSelectedChanged(VSchedule schedule)
        {
            var deSelectedIndex = SelectedSchedules!.IndexOf(schedule);

            if (deSelectedIndex < SelectedSchedules.Count - 1)
            {
                grid.SelectRow(schedule);
                return;
            }
            schedule.Selected = false;
            //grid.SelectRow(schedule, false);
        }

        void SelectionChanged(bool? args)
        {

            if (args == true)
            {
                if (SelectedSchedules is null)
                    SelectedSchedules = [];
                var Unionschedules = LateSchedules.Where(i => !(SelectedSchedules.Select(s => s.PaymentDate)
                                                                                .Contains(i.PaymentDate)))
                                                                                .ToList();
                SelectedSchedules = SelectedSchedules.Union(Unionschedules).ToList();
                SelectedSchedules.ToList()
                                  .ForEach(x =>
                                  {
                                      x.Selected = true;
                                  });
            }
            else
            {
                SelectedSchedules!.ToList()
                                  .ForEach(x =>
                                  {
                                      x.Selected = false;
                                  });
                SelectedSchedules = [];

            }

            if (SelectedSchedules is null || SelectedSchedules!.Any() != true)
            {
                isHeaderChecked = false;
            }
            else
            {

                isHeaderChecked = !LateSchedules.All(i => SelectedSchedules
                                                          .Select(x => x.PaymentDate)
                                                          .Contains(i.PaymentDate))
                                                          ? null
                                                          : LateSchedules.Any(i => SelectedSchedules
                                                                                   .Select(x => x.PaymentDate)
                                                                                   .Contains(i.PaymentDate));
            }
        }

        void SelectionCheckChanged(bool args, VSchedule data)
        {
            var selectedIndex = LateSchedules.ToList().IndexOf(data);
            if (selectedIndex > 0 && !IsPreviousRowSelected(selectedIndex - 1))
            {
                args = false;
                LateSchedules.ToList()[selectedIndex].Selected = false;
                grid.SelectRow(data, false);
                NotificationService.Notify(NotificationSeverity.Error, "Previous schedule is not selected yet 1.");
                return;
            }
            if (!allowRowSelectOnRowClick)
            {
                if (SelectedSchedules is not null || SelectedSchedules!.Any())
                {
                    data.Selected = SelectedSchedules!.Select(s => s.PaymentDate)
                                                   .Contains(data.PaymentDate);
                    grid.SelectRow(data);

                }
            }

            if (!args && !allowRowSelectOnRowClick)
            {
                var removeIndex = SelectedSchedules!.IndexOf(data);
                if (removeIndex < SelectedSchedules!.Count - 1)
                {
                    data.Selected = true;
                    grid.SelectRow(data);
                    return;
                }

                data.Selected = false;
                grid.SelectRow(data, false);
            }
            isHeaderChecked = SelectedSchedules!.All(x => x.Selected == true);

        }



        void RecievePayments()
        {
            Console.WriteLine(SelectedSchedules!.Count);
        }

        bool IsPreviousRowSelected(int index)
        {
            return LateSchedules.ToList()[index].Selected == true;
        }






    }
}