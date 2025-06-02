using Applications.Commons.Mappers;
using Applications.Enumerations;
using Applications.Requests.Loans.Payments;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Domains.Models.Currencies;
using Domains.Models.Identity;
using Domains.Models.Loans;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments
{
    public partial class LateScheduleDetail
    {

        private readonly ILoanScheduleServiceImpl _scheduleService;
        private readonly ILoanServiceImpl _loanService;
        public LateScheduleDetail(ILoanScheduleServiceImpl scheduleService, ILoanServiceImpl loanService)
        {
            _scheduleService = scheduleService;
            _loanService = loanService;
        }

        [Parameter] public string? LoanId { get; set; }
        [Parameter] public int BranchId { get; set; }
        [Parameter] public DateTime? PaymentDate { get; set; } = DateTime.Now;
        [CascadingParameter]
        Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        IEnumerable<VSchedule> LateSchedules { get; set; } = [];
        bool allowRowSelectOnRowClick = true;
        RadzenDataGrid<VSchedule> grid = default!;
        IList<VSchedule>? SelectedSchedules;
        bool? isHeaderChecked = false;
        bool isRowChecked = false;
        VLoanDisbursments? LoanSchedule = new();
        private int counts = 0;
        string? UserId = string.Empty;
        protected override async Task OnInitializedAsync()
        {

            await GetLoanWithSchedules(BranchId, LoanId);

            await GetLateSchedule();

            var authState = await AuthenticationState;

            UserId = authState.User.GetId() ?? string.Empty;

            await base.OnInitializedAsync();
        }

        async Task GetLateSchedule()
        {
            var rsp = await _scheduleService.GetLateScedulesAsync(BranchId, LoanId, PaymentDate);

            LateSchedules = rsp.Items;

            counts = rsp.Count;
        }
        async Task GetLoanWithSchedules(int? branchId, string? loanId)
        {

            var rsp = await _loanService.GetLoanByIdAsync(loanId, BranchId, ScheduleStatus.All, false);

            if (rsp.IsSuccess == false)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = rsp.ErrMessage,
                    Duration = 3000
                });
            }
            else
            {
                LoanSchedule = rsp.Data;
            }
        }


        void RowSelectedChanged(VSchedule schedule)
        {
            //if (SelectedSchedules == null)
            //{
            //    SelectedSchedules = new List<VSchedule>();
            //}
            //var scheduleList = LateSchedules.ToList();

            //var firstSchedule = scheduleList.FirstOrDefault();

            //if (!SelectedSchedules!.Any())
            //{
            //    if (schedule != firstSchedule)
            //    {
            //        NotificationService.Notify(new NotificationMessage
            //        {
            //            Severity = NotificationSeverity.Error,
            //            Summary = "First Row Selected",
            //            Detail = "Only The first schedule is allow selected.",
            //            Duration = 3000
            //        });

            //        SelectedSchedules = [];

            //        return;
            //    }
            //    else
            //    {
            //        SelectedSchedules!.Add(schedule);
            //    }

            //}

            //int newIndex = scheduleList.IndexOf(schedule!);

            //var selectedIndices = SelectedSchedules!.Select(e => scheduleList.IndexOf(e)).ToList();

            //// Check if the new selection is contiguous
            //int minIndex = selectedIndices.Min();

            //int maxIndex = selectedIndices.Max();

            //bool isContiguous = newIndex == minIndex - 1 || newIndex == maxIndex + 1 || selectedIndices.Contains(newIndex);

            //if (isContiguous)
            //{
            //    if (!SelectedSchedules!.Contains(schedule!))
            //    {
            //        SelectedSchedules.Add(schedule!);
            //        //await gridSchedule.SelectRow(schedule!);
            //    }
            //}


            //await grid.Reload();

            //StateHasChanged();

        }

        void RowDeSelectedChanged(VSchedule schedule)
        {
            //var deSelectedIndex = SelectedSchedules!.IndexOf(schedule);

            //if (deSelectedIndex < SelectedSchedules.Count - 1)
            //{
            //    grid.SelectRow(schedule);
            //    return;
            //}
            //schedule.Selected = false;
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
            var options = new DialogOptions
            {
                Draggable = true,
                Left="5rem",
            };
            var loanPayment = new LoanPaymentDtoCreate
            {
                LoanRefId = LoanId,
                CreditAgentRefId = LoanSchedule?.CreditAgentRefId,
                PayDate = DateTime.Now,
                OfficialReceiptNo = "",
                BranchRefId = BranchId,
                UserRefId = UserId,
                CurrencyRefId = LoanSchedule?.CurrencyId,
                CustomerName = LoanSchedule!.AltName
            };

            var loanPaymentLines = LateSchedules.MapsTo();

            loanPayment.LoanPaymentLines = loanPaymentLines.ToList();


            var parameters = new Dictionary<string, object?>{
                {"LoanPaymentCreate", loanPayment},
                {"CurrencyId",LoanSchedule.CurrencyId }
            };

            DialogService.OpenAsync<PaymentModal>($"{loanPayment.CustomerName}", parameters, options);


        }

        bool IsPreviousRowSelected(int index)
        {
            return LateSchedules.ToList()[index].Selected == true;
        }






    }
}