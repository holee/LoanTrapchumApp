using Applications.Enumerations;
using Applications.Extensions;
using Applications.Extensions.DateTimes;
using Applications.Extensions.mappers;
using Applications.Extensions.Maths;
using Applications.Interfaces.Lists;
using Applications.Interfaces.Loans;
using Applications.Interfaces.setting;
using Applications.Requests.Loans.Disbursements;
using Applications.Requests.Loans.LoanRequests;
using Applications.Requests.Loans.Payments;
using Applications.Responses.Auths;
using Applications.Responses.Loans.Disbursements;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Responses.ViewModels.Options;
using Domains.Models.Files;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestInfo(
        ILoanSettingService loanSettingService,
        IAppSettingService settingsServiceImpl,
        IHolidayService holidayService,
        ILogger<LoanRequestCreate> logger)
    {
        private readonly IAppSettingService _settingsServiceImpl = settingsServiceImpl;
        
        private readonly ILogger<LoanRequestCreate> _logger = logger;

        [Parameter] public LoanRequestDtoCreate Model { get; set; } = new();
        [Parameter] public string? UserId { get; set; }
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public EventCallback<string?> SelectCustomer { get; set; }
        [Parameter] public string? LoanRequestId { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        IEnumerable<LoanScheduleDtoCreate> previewSchedule = [];

        IEnumerable<ApprovedUser> AprovedUsers = [];

        IEnumerable<VCustomerList> Customers = [];

        IEnumerable<LoanRepaymentModeDto>? LoanRepaymentModes = [];

        IEnumerable<LoanScheduleStatusDto>? LoanScheduleStatus = [];

        IEnumerable<LoanStatusDto>? LoanStatuses = [];

        IEnumerable<LoanCategoryResponse>? loanCategories = [];

        IEnumerable<VCustomerList>? CustomersList = [];

        IEnumerable<LoanInstallmentTypeDto>? LoanInstallments = [];

        List<VLoanRequestGuarantors> Guarantors = [];

        List<VLoanRequestCollateralLines> CollateralLines = [];

        List<VLoanRequestsDocuments> Documents = [];

        IEnumerable<LoanRequestDocumentsDtoCreate> _documents = [];

        List<DateTime> Holidays = [];

        VCustomerList? selectedCustomer = new();

        DateTime firstPayment;

        LoanCategoryResponse? SelectedLoanCategory;
        
        long? size = 0;
        int counts = 0;
        bool isVible = false;
        bool isCustomerSelected = false;
        bool isLoading = false;
        bool isProgressing = false;
        int SelectedCollectionFee = 1;
        decimal? TotalAdminFee = 0;
        int SelectedPeriodName = 1;
        int installmentType = 1;
        int selectedTabIndex = 0;
        string? filterText = "";


        protected override async Task OnInitializedAsync()
        {
            var cat = await loanSettingService.GetLoanCategories();

            if (cat!.Any())
            {
                loanCategories = cat.Maps<LoanCategoryResponse>();

                SelectedLoanCategory = loanCategories!.FirstOrDefault();
            }

            await GetApprovedUser(BranchId);
            
            await base.OnInitializedAsync();
        }


        private async Task GetApprovedUser(int? branchId)
        {
            var aproveResp = await loanSettingService.GetAprovedUserAsync<ApprovedUser>(branchId);

            AprovedUsers = aproveResp ?? [];
        }

        private async Task GetCustomers(string? customerId, int take = 25, string? filter = null)
        {
            var queryParam = new QueryParam(0, take, string.Empty, filter: filter);

            var customerResponse = await _customerService!.GetCustomersAsync<VCustomerList>(customerId, queryParam);

            if (customerResponse.IsSuccess)
            {
                Customers = customerResponse.Data!.Items;
            }
        }

        async Task SetCustomerId(string? customerId)
        {
            if (customerId.IsNotNullOrEmpty())
            {
                await SelectCustomer.InvokeAsync(customerId);
            }
        }

        private async Task GetHolidays(int period)
        {
            var cur = DateTime.Now;
            period += 2;
            var fut = cur.AddMonths(period);
            var hols = await holidayService.GetHolidays(false, cur, fut);
            Holidays = hols.Select(x => x.HolidayDt!.Value).ToList();
        }

        void ChangedDate()
        {
            if (installmentType == 1)
            {
                firstPayment = DateTime.UtcNow.AddMonths(1);
            }

            if (installmentType == 2)
            {
                firstPayment = DateTime.UtcNow.AddDays(14);
            }

            if (installmentType == 3)
            {
                firstPayment = DateTime.UtcNow.AddDays(7);
            }

            if (installmentType == 4)
            {
                firstPayment = DateTime.UtcNow.AddDays(1);
            }

            HolidayHelper.SetHoliday(Holidays, ref firstPayment);

            Model.FirstPaymentDate = firstPayment;
        }

        void ChangedDate(DateTime? date)
        {
            if (installmentType == 1)
            {
                firstPayment = date!.Value.AddMonths(1);
            }

            if (installmentType == 2)
            {
                firstPayment = date!.Value.AddDays(14);
            }

            if (installmentType == 3)
            {
                firstPayment = date!.Value.AddDays(7);
            }

            if (installmentType == 4)
            {
                firstPayment = date!.Value.AddDays(1);
            }

            HolidayHelper.SetHoliday(Holidays, ref firstPayment);

            Model.FirstPaymentDate = firstPayment;
        }

        void CalculateAdminFeeAmount(decimal? value)
        {
            if (Model.CurrencyRefId == 1)
            {
                Model.AdminFeeAmount = (Model.RequestAmount * (value / 100)).RoundUpKhr(2);
            }
            else
            {
                Model.AdminFeeAmount = (Model.RequestAmount * (value / 100)).RoundUp(2);
            }
        }

        void SetDefault()
        {
            Model.CurrencyRefId = 1;
            Model.InterestRate = 1.10M;
            Model.CollectionFeeRate = 2.90M;
            Model.AdminFeeRate = 3.00M;
            Model.Installments = 12;
            Model.InstallmentType = "Monthly";
            Model.InstallmentTypeId = 1;
            Model.RepaymentModeRefId = 2;
            Model.RequestDate = DateTime.Now;
            Model.RequestAmount = 0.00M;
            Model.AdminFeeAmount = 0.00M;
            Model.PayEvery = 1;
            //previewSchedule = [];
            Model.FirstPaymentDate = DateTime.UtcNow.AddMonths(1);
            //installmentType = 1;
        }

        void PrincipleValueChanged(decimal? principle)
        {
            if (!principle.HasValue || principle < 0)
            {
                NotificationService.Notify(NotificationSeverity.Warning, "Invalid principle input");
                Model.RequestAmount = 0.00M;
                return;
            }

            if (principle <= 50000)
            {
                Model.CurrencyRefId = 2;
                Model.AdminFeeAmount = (principle * (Model.AdminFeeRate / 100)).RoundUp(2);
                return;
            }
            else
            {
                Model.CurrencyRefId = 1;
                Model.AdminFeeAmount = (principle * (Model.AdminFeeRate / 100)).RoundUp(-2);
                return;
            }
        }

        async Task PeriodChanged()
        {
            await GetHolidays(Model.Installments!.Value);
        }

        void SelectedInstallmentChanged(object? arg)
        {
            if (arg is not null)
            {
                var installType = (Items)arg;
                installmentType = installType.Value;
                Model.InstallmentType = installType.Label;
                ChangedDate();
            }
        }

        void CurrencySelectedChanged(object e)
        {
            var selectedValue = (Items)e;

            StateHasChanged();
        }

        void SelectedChargeModeChanged(object? e)
        {
            if (e is not null)
            {
                var selectedValue = (Items)e;

                Model.CollectionFeeModeName = selectedValue.Label;
            }
            else
            {
                Model.CollectionFeeModeId = 1;

                Model.CollectionFeeModeName = "Flat";
            }
        }

        void RepaymentModeSelectedChanged(object e)
        {
            var selectedValue = (Items)e;
            Model.RepaymentMode = selectedValue.Label;
            if (selectedValue.Value == 1)
            {
                Model.PayEvery = 4;
            }
            else
            {
                Model.PayEvery = 1;
            }
        }
    }
}

// async Task GetCustomerRequest(string? customerId)
// {
//     if (customerId.IsNotNullOrEmpty())
//     {
//         var customerResponse = await _customerService.GetCustomerForRequestLoanAsync(customerId);
//
//         if (customerResponse.IsSuccess)
//         {
//             //SelectedCustomer = customerResponse.Data!;
//         }
//     }
// }