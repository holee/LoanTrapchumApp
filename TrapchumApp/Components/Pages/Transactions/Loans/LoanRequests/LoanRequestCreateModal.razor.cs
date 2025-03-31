using Applications.Extensions;
using Applications.Extensions.LoanCalculators;
using Applications.Extensions.mappers;
using Applications.Extensions.Maths;
using Applications.Extensions.NumberExtions;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans.Disbursements;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestCreateModal
    {
        private readonly ILoanSettingServiceImpl _settingService;

        public LoanRequestCreateModal(ILoanSettingServiceImpl loanSettingService)
        {
            _settingService = loanSettingService;
        }

        [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
        [Parameter, SupplyParameterFromQuery] public Guid? UserId { get; set; }
        [Parameter] public Guid? CustomerId { get; set; } = Guid.Empty;
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        IEnumerable<LoanScheduleDtoCreate> previewSchedule = [];
        IEnumerable<AprovePerson> AprovedLevels = [];
        IEnumerable<VCustomerList> Customers = [];
        LoanRequestDtoCreate model = new();
        DateTime? RegisterDate = DateTime.Now;
        IEnumerable<VBranchResponse> Branches = [];
        IEnumerable<LoanInstallmentTypeDto>? LoanInstallments = [];
        SelectedCreditAgent? selectCreditAgent = new();
        EditForm editForm = default!;
        LoanInstallmentTypeDto? SelectedInstallment { get; set; }
        VCustomerList? SelectedCustomer = new();
        bool isVible = false;
        int selectedTabIndex = 0;
        bool isProgressing = false;
        bool isCollateralTab = false;
        bool isPreviewScheduleTab = false;
        int SelectedCollectionFee = 1;
        decimal? TotalAdminFee = 0;
        int SelectedPeriodName = 1;
        int installId = 1;
        string branchName = "";
        string customerName = "";
        protected override async Task OnParametersSetAsync()
        {
            var rsp = await _settingService!.GetInstallmentTypes();

            if (rsp.Any())
            {
                LoanInstallments = rsp.Maps<LoanInstallmentTypeDto>();
            }

            var branchResponse = await _branchService.GetBranchById(BranchId);

            if (branchResponse.IsSuccess)
            {
                branchName = branchResponse.Data!.BranchName;
            }

            await base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (CustomerId.IsNullOrEmpty())
                {
                    await GetCustomers(CustomerId, 10);
                }
                else
                {
                    await GetCustomers(CustomerId, 10);
                }

                var resp = await _customerService.GetCustomerDetailsByIdAsync(CustomerId);

                if (resp.IsSuccess)
                {
                    SelectedCustomer = resp.Data!;

                    model.CustomerRefId = SelectedCustomer.CustomerId;

                }

                var aproveResp = await _settingService.GetAprovedUserAsync(BranchId);

                if (aproveResp.Any())
                {
                    AprovedLevels = aproveResp;
                }
                SetDefault();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }

            await base.OnInitializedAsync();
        }


        private async Task GetCustomers(Guid? customerId, int take = 10, string? filter = null)
        {
            var rsp = await _customerService!.GetCustomersAsync(customerId, take, false, true, filter);

            if (rsp.IsSuccess)
            {
                Customers = rsp.Data!;
            }
            else
            {
                Customers = [];

                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }


        }

        private async Task LoadCustomers(LoadDataArgs e)
        {
            //
            var filterText = e.Filter ?? "";

            var rsp = await _customerService!.GetCustomersAsync(null, e.Top!.Value, false, false, filterText);

            if (!rsp.IsSuccess)
            {
                Customers = [];

                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
            else
            {
                Customers = rsp.Data!;
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
            model.RequestAmount = 0.00M;
            previewSchedule = [];
        }
        protected async Task SubmitForm()
        {
            editForm.EditContext!.Validate();
            var calculator = SelectLoanType(3);
            calculator.PayEvery = 1;
            calculator.EffectiveRate = model.CollectionFeeRate!.Value;
            calculator.CollectionFeeMode = SelectedCollectionFee;
            calculator.InstallmentTypeId = 1;
            calculator.CurrencyId = model.CurrencyRefId!.Value;
            var schedule = calculator.Calculate(model.Installments, model.InterestRate, model.RequestAmount);
            var scheduleDto = schedule.Select(x => new LoanRequestScheduleCreateDto
            {
                InstallmentNo = x.InstallmentNo,
                Principal = x.Principal,
                InterestAmount = x.Interest,
                CollectionFeeAmount = x.CollectionFeeAmount,
                LoanOutstanding = x.OutStanding,
                TotalPay = x.TotalPay
            }).ToList();
            try
            {
                model.RequestUserRefId = UserId;
                model.RequestDate = DateTime.UtcNow;
                model.RepaymentMode = "Flat";
                model.LoanRequestStatusRefId = 1;
                model.BranchRefId = BranchId;
                //model.LoanRequestSchedules.AddRange(scheduleDto);
                
                var resp = await _loanRequestService.CreateLoanRequestAsync(model);

                if (resp.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Error, resp.Data);

                    NavigationManager.NavigateTo("Loan/Request");
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }

        }

        private LoanCaloulator SelectLoanType(int? paymentMode)
        {
            return paymentMode switch
            {
                1 => new Balloon(),
                2 => new Decline(),
                3 => new Flat(),
                4 => new Annuarity(),
                _ => new Flat(),
            };
        }

        void PrincipleTextChanged(decimal? principle)
        {
            if (principle is null || principle < 0)
            {
                NotificationService.Notify(NotificationSeverity.Warning, "Invalid principle input");
                model.RequestAmount = 0.00M;
                return;
            }

            if (principle <= 50000)
            {
                model.CurrencyRefId = 2;
                model.AdminFeeAmount = (model.RequestAmount!.Value * (model.AdminFeeRate!.Value / 100)).RoundUp(2);
            }
            else
            {
                model.CurrencyRefId = 1;
                model.AdminFeeAmount = (model.RequestAmount!.Value * (model.AdminFeeRate!.Value / 100)).RoundUp(-2);
            }
        }



        void PrincipleValueChanged(ChangeEventArgs e)
        {
            var principle = e.Value?.ToNumer<decimal>();

            if (principle is null || principle < 0)
            {
                NotificationService.Notify(NotificationSeverity.Warning, "Invalid principle input");
                model.RequestAmount = 0.00M;
                return;
            }

            if (principle <= 50000)
            {
                model.CurrencyRefId = 2;
                model.AdminFeeAmount = (principle.Value * (model.AdminFeeRate!.Value / 100)).RoundUp(2);
            }
            else
            {
                model.CurrencyRefId = 1;
                model.AdminFeeAmount = (principle!.Value * (model.AdminFeeRate!.Value / 100)).RoundUp(-2);
            }
        }


        void ShowLoanPreviews()
        {
            if (model.RequestAmount == 0) return;
            selectedTabIndex = 2;
            var calculator = SelectLoanType(3);
            calculator.PayEvery = 1;
            calculator.EffectiveRate = model.CollectionFeeRate!.Value;
            calculator.StartDate = DateTime.Now;
            calculator.FirstPayment = DateTime.Now.AddMonths(1);
            calculator.CollectionFeeMode = SelectedCollectionFee;
            calculator.InstallmentTypeId = 1;
            calculator.CurrencyId = model.CurrencyRefId!.Value;
            var schedule = calculator.Calculate(model.Installments!.Value, model.InterestRate!.Value, model.RequestAmount!.Value);
            previewSchedule = schedule.Select(x => new LoanScheduleDtoCreate
            {
                InstallmentNo = x.InstallmentNo,
                PaymentDate = x.PaymentDate,
                Principal = x.Principal,
                Interest = x.Interest,
                CollectionFee = x.CollectionFeeAmount,
                LoanOutstanding = x.OutStanding,
                TotalDay = x.TotalDay,
                TotalPay = x.TotalPay,
                AgentRefId = selectCreditAgent!.Id,
                ScheduleStatusRefId = 2

            }).ToList();
        }

        void SelectedInstallmentChanged(object arg)
        {
            if (arg is not null)
            {
                var installType = (LoanInstallmentTypeDto)arg;
                installId = installType.InstallmentTypeId;
                model.InstallmentType = installType.InstallmentTypeCode;
            }


        }
        protected async void SelectCustomerValueChanged()
        {
            previewSchedule = [];

            if (model.CustomerRefId.IsNotNullOrEmpty())
            {
                var customerResponse = await _customerService.GetCustomerForRequestLoanAsync(CustomerId);

                if (customerResponse.IsSuccess)
                {
                    SelectedCustomer = customerResponse.Data!;
                }
            }
            else
            {
                SelectedCustomer = new();
            }
            StateHasChanged();
        }



        async void SelectCustomerChanged(object? e)
        {
            if (e is not null)
            {
                var id = ((VCustomerList)e).CustomerId;
                var customerResponse = await _customerService.GetCustomerForRequestLoanAsync(id);
                if (customerResponse.IsSuccess)
                {
                    //model.CustomerRefId = id;
                    SelectedCustomer = customerResponse.Data!;
                }
                else
                {
                    SelectedCustomer = new();
                }
            }
            else
            {
                SelectedCustomer = new();
            }
            StateHasChanged();
        }

    }
}