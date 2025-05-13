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
    public partial class LoanRequestInfo
    {

        private readonly ILoanSettingService _loanSettingService;
        private readonly IAppSettingService _settingsServiceImpl;
        private readonly IHolidayService _holidayService;
        private readonly ILogger<LoanRequestCreate> _logger;

        [Parameter] public LoanRequestDtoCreate model { get; set; } = new();

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

        VUser _user = new();
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


        protected override void OnParametersSet()
        {
            if (model.CustomerRefId.IsNotNullOrEmpty() || model.LoanRequestId.IsNotNullOrEmpty())
            {
                isCustomerSelected = true;
            }

            base.OnParametersSet();
        }
        protected override async Task OnInitializedAsync()
        {

            var cat = await _loanSettingService.GetLoanCategories();

            if (cat!.Any())
            {
                loanCategories = cat.Maps<LoanCategoryResponse>();

                SelectedLoanCategory = loanCategories!.FirstOrDefault();
            }

            var aproveResp = await _loanSettingService.GetAprovedUserAsync<ApprovedUser>(BranchId);

            AprovedUsers = aproveResp ?? [];

            await base.OnInitializedAsync();
        }



        public LoanRequestInfo(ILoanSettingService loanSettingService, IAppSettingService settingsServiceImpl, IHolidayService holidayService, ILogger<LoanRequestCreate> logger)
        {
            _loanSettingService = loanSettingService;
            _settingsServiceImpl = settingsServiceImpl;
            _holidayService = holidayService;
            _logger = logger;
        }

        async Task GetCustomerRequest(string? customerId)
        {
            if (customerId.IsNotNullOrEmpty())
            {
                var customerResponse = await _customerService.GetCustomerForRequestLoanAsync(customerId);

                if (customerResponse.IsSuccess)
                {
                    //SelectedCustomer = customerResponse.Data!;
                }
            }
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
            if (customerId.IsNotNullOrEmpty()) {

                await SelectCustomer.InvokeAsync(customerId);

            }

        }

        private async Task GetHolidays(int period)
        {
            var cur = DateTime.Now;
            period += 2;
            var fut = cur.AddMonths(period);
            var hols = await _holidayService.GetHolidays(false, cur, fut);
            Holidays = hols.Select(x => x.HolidayDt).ToList();
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

            model.FirstPaymentDate = firstPayment;
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

            model.FirstPaymentDate = firstPayment;
        }

        void CalculateAdminFeeAmount(decimal? value)
        {
            if (model.CurrencyRefId == 1)
            {
                model.AdminFeeAmount = (model.RequestAmount * (value / 100)).RoundUpKhr(2);
            }
            else
            {
                model.AdminFeeAmount = (model.RequestAmount * (value / 100)).RoundUp(2);
            }
        }

        void SetDefault()
        {
            model.CurrencyRefId = 1;
            model.InterestRate = 1.10M;
            model.CollectionFeeRate = 2.90M;
            model.AdminFeeRate = 3.00M;
            model.Installments = 12;
            model.InstallmentType = "Monthly";
            model.InstallmentTypeId = 1;
            model.RepaymentModeRefId = 2;
            model.RequestDate = DateTime.Now;
            model.RequestAmount = 0.00M;
            model.AdminFeeAmount = 0.00M;
            model.PayEvery = 1;
            //previewSchedule = [];
            model.FirstPaymentDate = DateTime.UtcNow.AddMonths(1);
            //installmentType = 1;
        }

        void PrincipleValueChanged(decimal? principle)
        {
            if (!principle.HasValue || principle < 0)
            {
                NotificationService.Notify(NotificationSeverity.Warning, "Invalid principle input");
                model.RequestAmount = 0.00M;
                return;
            }

            if (principle <= 50000)
            {
                model.CurrencyRefId = 2;
                model.AdminFeeAmount = (principle * (model.AdminFeeRate / 100)).RoundUp(2);
                return;
            }
            else
            {
                model.CurrencyRefId = 1;
                model.AdminFeeAmount = (principle * (model.AdminFeeRate / 100)).RoundUp(-2);
                return;
            }
        }

        async Task PeriodChanged()
        {
            await GetHolidays(model.Installments!.Value);
        }

        void SelectedInstallmentChanged(object arg)
        {
            if (arg is not null)
            {
                var installType = (Items)arg;
                installmentType = installType.Value;
                model.InstallmentType = installType.Label;
                ChangedDate();
            }
        }

        void CurrencySelectedChanged(object e)
        {
            var selectedValue = (Items)e;

            StateHasChanged();
        }

        void SelectedChargeModeChanged(object e)
        {
            if (e is not null)
            {
                var selectedValue = (Items)e;

                model.CollectionFeeModeName = selectedValue.Label;
            }
            else
            {
                model.CollectionFeeModeId = 1;

                model.CollectionFeeModeName = "Flat";
            }
        }

        void RepaymentModeSelectedChanged(object e)
        {
            var selectedValue = (Items)e;
            model.RepaymentMode = selectedValue.Label;
            if (selectedValue.Value == 1)
            {
                model.PayEvery = 4;
            }
            else
            {
                model.PayEvery = 1;
            }
        }

    }
}