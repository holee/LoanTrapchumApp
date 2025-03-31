// using Applications.Extensions;
// using Applications.Extensions.DateTimes;
// using Applications.Extensions.LoanCalculators;
// using Applications.Interfaces.Lists;
// using Applications.Interfaces.Loans;
// using Applications.Requests.Collaterals;
// using Applications.Requests.Lists.Entities.Guarantors;
// using Applications.Requests.Loans.Disbursements;
// using Applications.Requests.Loans.Payments;
// using Applications.Responses;
// using Applications.Responses.Loans.Disbursements;
// using Applications.Responses.ViewModels.Customers;
// using Applications.Responses.ViewModels.OtherNames;
// using Applications.Services.Customers;
// using Applications.Services.Lists;
// using Applications.Services.LIsts;
// using Applications.Services.Loans;
// using Applications.Services.Settings;
// using Microsoft.AspNetCore.Components;
// using Microsoft.AspNetCore.Components.Forms;
// using Radzen;
// using TrapchumApp.Components.Pages.Lists.OtherNames;
//
// namespace TrapchumApp.Components.Pages.Transactions.Loans.Disbursements;
//
// public partial class Create
// {
//     private readonly IGuarantorServiceImpl _gService;
//     private readonly ICollateralsServiceImpl _collateralService;
//     private readonly ISettingServiceImpl _settingService;
//     private readonly ILoanSettingService _loanSettingService;
//     private readonly IHolidayService _holidayService;
//     private readonly CreditAgentService _creditAgentService;
//     private readonly ILoanService _loanService;
//
//     public Create(IGuarantorServiceImpl gService, ICollateralsServiceImpl collateralsService,
//         ISettingServiceImpl settingService, ILoanSettingService loanSettingService, IHolidayService holidayService,
//         CreditAgentService creditAgentService, ILoanService loanService)
//     {
//         _gService = gService;
//         _collateralService = collateralsService;
//         _settingService = settingService;
//         _loanSettingService = loanSettingService;
//         _holidayService = holidayService;
//         _creditAgentService = creditAgentService;
//         _loanService = loanService;
//     }
//
//     [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
//     [Parameter, SupplyParameterFromQuery] public Guid? UserId { get; set; }
//     [Parameter, SupplyParameterFromQuery] public Guid? CustomerRequestId { get; set; }
//     [Parameter, SupplyParameterFromQuery] public Guid? LoanRequestId { get; set; }
//
//     protected IEnumerable<LoanInstallmentTypeDto>? LoanInstallments = [];
//     protected IEnumerable<LoanRepaymentModeDto>? LoanRepaymentModes = [];
//     protected IEnumerable<LoanScheduleStatusDto>? LoanScheduleStatus = [];
//     protected IEnumerable<LoanStatusDto>? LoanStatuses = [];
//     protected IEnumerable<LoanCategoryResponse>? loanCategories = [];
//     protected IEnumerable<VCustomerList>? CustomersList = [];
//     protected IEnumerable<SelectedCreditAgent>? CreditAgents = [];
//     protected IEnumerable<VGuarantor>? Guarantors = [];
//     protected IEnumerable<CollateralLineDtoCreate>? CollateralLines = [];
//     protected List<DateTime> Holidays = [];
//     protected VCustomerList? selectedCustomer = new();
//     protected SelectedCreditAgent? selectCreditAgent = new();
//     protected DateTime firstPayment;
//     private EditForm editForm = default!;
//     private decimal penaltyRate = 0.0M;
//     LoanCategoryResponse? SelectedLoanCategory;
//     LoanInstallmentTypeDto? SelectedInstallment;
//
//     async Task GetGaurantors()
//     {
//         var gResponse = await _gService!.GetGuarantorsAsync(null, true);
//
//         if (gResponse.IsSuccess)
//         {
//             Guarantors = gResponse.Data!;
//         }
//         else
//         {
//             Guarantors = [];
//         }
//     }
//
//     async Task GetCollaterals(Guid? customerId)
//     {
//         var collateralResponse = await _collateralService.GetCollateralByCustomerAsync(customerId);
//
//         if (collateralResponse.IsSuccess)
//         {
//             CollateralLines = collateralResponse.Data!.CollateralLines!.Select(x => new CollateralLineDtoCreate
//             {
//                 CollateralRefId = x.CollateralRefId,
//                 CollateralNo = x.CollateralNo,
//                 SubTypeName = x.SubTypeName,
//                 TypeName = x.SubTypeName,
//                 ReferenceNo = x.ReferenceNo,
//                 OwnerName = x.OwnerName,
//             });
//         }
//     }
//
//     protected override async Task OnParametersSetAsync()
//     {
//         //isProgressing = true;
//
//         //LoanRepaymentModes = await _loanSettingService!.GetRepaymentModes();
//
//         //LoanInstallments = await _loanSettingService!.GetLoanInstallmentTypes();
//
//         //loanCategories = await _loanSettingService.GetLoanCategories();
//
//         //if (loanCategories!.Any())
//         //{
//         //    SelectedLoanCategory = loanCategories!.FirstOrDefault();
//         //}
//
//         //var settingResponse = await _settingsServiceImpl.GetSettingByCode("PNLTY");
//
//         //if (settingResponse.IsSuccess)
//         //{
//         //    penaltyRate = settingResponse.Data!.Value.ToNumer<decimal>();
//         //}
//         //var branchResponse = await _branchService.GetBranchByIdAsync(BranchId);
//
//         //if (branchResponse.IsSuccess)
//         //{
//         //    branchName = branchResponse.Data!.BranchName;
//         //}
//
//         await GetCreditAgents();
//
//         await GetCustomers(null, 10);
//
//         //isProgressing = false;
//
//         await base.OnParametersSetAsync();
//     }
//
//     protected override async Task OnInitializedAsync()
//     {
//         try
//         {
//             SetDefault();
//
//             if (CustomerRequestId.IsNotNullOrEmpty())
//             {
//                 model.CustomerRefId = CustomerRequestId;
//
//                 var resp = await _loanRequestService.GetLoanRequestsAsync(BranchId, CustomerRequestId, LoanRequestId,
//                     false);
//
//                 if (resp.IsSuccess)
//                 {
//                     model.Principle = resp.Data!.RequestAmount;
//                     model.InterestRate = resp.Data!.InterestRate;
//                     model.Installments = resp.Data!.Installments;
//                     PrincipleTextChanged(resp.Data.RequestAmount);
//                 }
//
//                 SelectCustomerValueChanged();
//             }
//         }
//         catch (Exception ex)
//         {
//             NotificationService.Notify(NotificationSeverity.Error, ex.Message);
//         }
//
//         await base.OnInitializedAsync();
//     }
//
//     protected async void SelectCustomerValueChanged()
//     {
//         SelectedGuarantors = [];
//
//         CollateralLines = [];
//
//         Collaterals = [];
//
//         previewSchedule = [];
//
//         if (model.CustomerRefId > 0)
//         {
//             await GetHolidays(model.Installments!.Value);
//
//             var customerResponse = await _customerService.GetCustomerForRequestLoanAsync(model.CustomerRefId);
//
//             if (customerResponse.IsSuccess)
//             {
//                 selectedCustomer = customerResponse.Data!;
//             }
//
//             if (selectedCustomer is not null)
//             {
//                 await GetGaurantors();
//
//                 model.Circle = selectedCustomer.Circle!.Value + 1;
//
//                 customerName = selectedCustomer.AltName!;
//
//                 await GetCollaterals(selectedCustomer.CustomerId);
//             }
//         }
//         else
//         {
//             selectedCustomer = new();
//         }
//
//         StateHasChanged();
//     }
//
//     protected async void SelectedCustomerValueChanged(object e)
//     {
//         if (e is null && CustomerRequestId.IsNullOrEmpty())
//         {
//             Console.WriteLine("Clear.....");
//             SetDefault();
//             selectedCustomer = new();
//             return;
//         }
//
//         SelectedGuarantors = [];
//
//         CollateralLines = [];
//
//         Collaterals = [];
//
//         previewSchedule = [];
//
//         model.CustomerRefId = ((VCustomerList)e!).CustomerId;
//
//         if (model.CustomerRefId > 0)
//         {
//             await GetHolidays(model.Installments!.Value);
//
//             var customerResponse = await _customerService.GetCustomerForRequestLoanAsync(model.CustomerRefId);
//
//             if (customerResponse.IsSuccess)
//             {
//                 selectedCustomer = customerResponse.Data!;
//             }
//
//             if (selectedCustomer is not null)
//             {
//                 await GetGaurantors();
//
//                 model.Circle = selectedCustomer.Circle!.Value + 1;
//
//                 customerName = selectedCustomer.AltName!;
//
//                 await GetCollaterals(selectedCustomer.CustomerId);
//             }
//         }
//         else
//         {
//             selectedCustomer = new();
//         }
//
//         StateHasChanged();
//     }
//
//
//     async void SetDefault()
//     {
//         model.CurrencyRefId = 0;
//         firstPayment = DateTime.Now.AddMonths(1);
//         model.RepaymentModeRefId = 2;
//         model.RegisterDate = DateTime.Now;
//         model.StartDate = DateTime.Now;
//         model.FirstPaymentDate =
//             model.LastPaymentDate = DateTime.Now;
//         model.InterestRate = 1.10M;
//         model.AdminFeeRate = 3.00M;
//         model.CollectionFeeRate = 2.90M;
//         model.PayEvery = 1;
//         model.Installments = 12;
//         model.InstallmentType = "Monthly";
//         model.PeriodName = "Monthly";
//         model.Principle = 0.00M;
//         model.AdminFeeAmount = 0.00M;
//         model.CurrencyRefId = 1;
//         model.InstallmentTypeId = 1;
//         model.Period = model.Installments;
//         await GetHolidays(model.InstallmentTypeId!.Value);
//         HolidayHelper.SetHoliday(Holidays, ref firstPayment);
//         model.FirstPaymentDate = firstPayment;
//         selectedCustomer = new();
//         Guarantors = [];
//         CollateralLines = [];
//         SelectedGuarantors = [];
//         SelectedCollateralLines = [];
//         previewSchedule = [];
//
//         StateHasChanged();
//     }
//
//     private async Task GetHolidays(int period)
//     {
//         var cur = DateTime.Now;
//         period += 2;
//         var fut = cur.AddMonths(period);
//         var hols = await _holidayService.GetHolidays(false, cur, fut);
//         Holidays = hols.Select(x => x.HolidayName).ToList();
//     }
//
//     private async Task GetCreditAgents(string? filter = null)
//     {
//         var crs = await _creditAgentService!.GetCreditAgents(false, filter);
//
//         if (crs.Any())
//         {
//             CreditAgents = crs.Select(x => new SelectedCreditAgent
//             {
//                 Id = x.EmployeeRefId,
//                 Name = $"{x.FirstName} {x.LastName}"
//             }).ToList();
//             selectCreditAgent = CreditAgents
//                 .Select(x => new SelectedCreditAgent
//                 {
//                     Id = x.Id,
//                     Name = x.Name
//                 })
//                 .FirstOrDefault();
//         }
//     }
//
//     private async Task GetCustomers(Guid? customerId, int take = 10, string? filter = null)
//     {
//         var customerResponse = await _customerService!.GetCustomersAsync(customerId, take, false, true, filter);
//
//         if (customerResponse.IsSuccess)
//         {
//             CustomersList = customerResponse.Data;
//         }
//         else
//         {
//             CustomersList = [];
//             NotificationService.Notify(NotificationSeverity.Error, customerResponse.ErrMessage);
//         }
//     }
//
//     private async Task LoadCustomers(LoadDataArgs e)
//     {
//         //
//         var filterText = e.Filter ?? "";
//
//         var customersResponse = await _customerService!.GetCustomersAsync(null, e.Top!.Value, false, true, filterText);
//
//         if (!customersResponse.IsSuccess)
//         {
//             CustomersList = [];
//             NotificationService.Notify(NotificationSeverity.Error, customersResponse.ErrMessage);
//         }
//         else
//         {
//             CustomersList = customersResponse.Data;
//         }
//     }
//
//     protected async Task SubmitForm()
//     {
//         var collateralLines = SelectedCollateralLines!.Select(x => new CollateralLineDtoCreate
//         {
//             CollateralNo = x.CollateralNo,
//             SubTypeName = x.SubTypeName,
//             TypeName = x.SubTypeName,
//             CollateralRefId = x.CollateralRefId,
//             ReferenceNo = x.ReferenceNo,
//             OwnerName = x.OwnerName,
//         }).ToList();
//         model.BankRefId = 11;
//         model.TermTypeId = 11;
//         model.LoanStatusRefId = 4;
//         model.LoanSubTypeRefId = 1;
//         model.BranchRefId = BranchId;
//         model.UserRefId = UserId;
//         // model.LoanCollateralLines = collateralLines;
//         model.PenaltyRate = penaltyRate;
//         editForm.EditContext!.Validate();
//         var calculator = SelectLoanType(model.RepaymentModeRefId);
//         calculator.PayEvery = model.PayEvery!.Value;
//         calculator.EffectiveRate = model.CollectionFeeRate!.Value;
//         calculator.StartDate = model.StartDate!.Value;
//         calculator.FirstPayment = model.FirstPaymentDate!.Value;
//         calculator.CollectionFeeMode = SelectedCollectionFee;
//         calculator.InstallmentTypeId = model.InstallmentTypeId!.Value;
//         calculator.CurrencyId = model.CurrencyRefId!.Value;
//         calculator.SetHolidays(Holidays);
//         var schedule =
//             calculator.Calculate(model.Installments!.Value, model.InterestRate!.Value, model.Principle!.Value);
//         var scheduleDto = schedule.Select(x => new LoanScheduleDtoCreate
//         {
//             InstallmentNo = x.InstallmentNo,
//             PaymentDate = x.PaymentDate,
//             Principal = x.Principal,
//             Interest = x.Interest,
//             CollectionFee = x.CollectionFeeAmount,
//             LoanOutstanding = x.OutStanding,
//             TotalDay = x.TotalDay,
//             TotalPay = x.TotalPay,
//             AgentRefId = selectCreditAgent!.Id,
//             RepaymentModeRefId = model.RepaymentModeRefId,
//             ScheduleStatusRefId = 2
//         }).ToList();
//         var guarantors = SelectedGuarantors!.Select(x => new GuarantorDtoCreate
//         {
//             // CustomerRefId = selectedCustomer!.CustomerId,
//             EntId = x.Id,
//         }).ToList();
//
//         var lastDate = scheduleDto!.LastOrDefault()!.PaymentDate;
//
//         model.LastPaymentDate = lastDate;
//
//         try
//         {
//             model.LoanSchedules = scheduleDto ?? [];
//
//             // model.LoanGuarantors = guarantors ?? [];
//
//             var createResponse = await _loanService!.CreateLoanAsync(model);
//
//             if (!createResponse.IsSuccess)
//             {
//                 var error = createResponse.Errors!.FirstOrDefault();
//                 NotificationService.Notify(NotificationSeverity.Error, error);
//                 return;
//             }
//             else
//             {
//                 NotificationService.Notify(NotificationSeverity.Success, createResponse.Data!.Message);
//                 SetDefault();
//                 await GetCustomers(0, 10);
//             }
//         }
//         catch (Exception ex)
//         {
//             NotificationService.Notify(NotificationSeverity.Error, ex.Message);
//         }
//     }
//
//
//     private LoanCaloulator SelectLoanType(int? paymentMode)
//     {
//         return paymentMode switch
//         {
//             1 => new Balloon(),
//             2 => new Decline(),
//             3 => new Flat(),
//             4 => new Annuarity(),
//             _ => throw new NotImplementedException()
//         };
//     }
//
//     async Task AddGuarantorModals()
//     {
//         var resultDialogue = await DialogService.OpenAsync<GuarantorAddModal>("New Guarantor", null,
//             new DialogOptions()
//             {
//                 Width = "900px",
//                 Height = "480px",
//                 Draggable = true
//             });
//     }
// }
//

//protected async Task<IEnumerable<LoanCategoryDto>> SearchLoanCategories(string? searchText = null)
//{
//    var cats = loanCategories?.Where(p => p.LoancategoryCode!.ToLower() != null && p.LoancategoryCode.ToLower().Contains(searchText!) ||
//                       p.DesInKhmer != null && p.DesInKhmer.Contains(searchText!, StringComparison.InvariantCultureIgnoreCase));

//    return await Task.FromResult(cats!);
//}
//protected async Task<IEnumerable<SelectedCreditAgent>> SearchCreditAgent(string? searchText = null)
//{
//    await GetCreditAgents(searchText);
//    return await Task.FromResult(CreditAgents!);
//}
//protected async Task<IEnumerable<LoanCustomerQuery>> SearchCustomers(string? searchText = null)
//{
//    await GetCustomers(searchText);
//    return await Task.FromResult(CustomersList!);

//}

//bool IsSuccess = false;
//protected Task<bool> AddGuarantorValidated(bool param) => Task.FromResult(param);
//protected async Task AddGuarantorSucceed(bool param)
//{
//    if (param)
//    {
//        Guarantors = await _customerService!.GetGuarantorsAsync();
//        StateHasChanged();
//    }
//    await Task.CompletedTask;
//}
//protected Task<bool> AddCollateralValidated(bool param) => Task.FromResult(param);
//protected async Task AddCollateralSucceed(bool param)
//{
//    if (param)
//    {
//        var collateral = await CollateralService.GetCollateralById(selectedCustomer!.CustomerId);
//        if (collateral is not null)
//        {
//            CollateralLines = collateral.CollateralLines!.Select(x => new DisbursementCollateralLineDtoCreate
//            {
//                Id = x.Id,
//                SubTypeName = x.SubTypeName,
//                TypeName = x.TypeName,
//                CollateralNo = x.CollateralNo,
//                ReferenceNo = x.ReferenceNo
//            });

//            StateHasChanged();
//        }

//    }
//    await Task.CompletedTask;
//}
//private async Task AddCollateralByCustomer()
//{
//    if (selectedCustomer!.CustomerId <= 0)
//    {
//        NotificationService.Notify(NotificationSeverity.Error, "Please Select Customer First.");
//        return;
//    }
//await ModalService!.Show<CollateralModalAdd>(param =>
//{
//    param.Add(x => x.SelectedCustomer, selectedCustomer);
//    param.Add(x => x.OnSuccess, AddCollateralSucceed);
//    param.Add(x => x.onVildated, AddCollateralValidated);

//}, new ModalInstanceOptions
//{
//    UseModalStructure = false,
//    Size = ModalSize.ExtraLarge,
//    Animated = true,
//    ShowBackdrop = true,
//    Closing = OnModalClosing
//});


//private IEnumerable<LoanScheduleDtoCreate> Schedules(int paymentMode)
//{
//    if (paymentMode == 1)
//    {
//        var calculator = new Balloon();
//        calculator.PayEvery = model.PayEvery!.Value;
//        calculator.EffectiveRate = model.CollectionFeeRate!.Value;
//        calculator.StartDate = model.StartDate!.Value;
//        calculator.FirstPayment = model.FirstPaymentDate!.Value;
//        var schedule = calculator.Calculate(model.Installments!.Value, model.InterestRate!.Value, model.Principle!.Value);
//        return schedule.Select(x => new LoanScheduleDtoCreate
//        {
//            InstallmentNo = x.InstallmentNo,
//            PaymentDate = x.PaymentDate,
//            Principal = x.Principal,
//            Interest = x.Interest,
//            CollectionFee = x.CollectionFee,
//            LoanOutstanding = x.OutStanding,
//            TotalDay = x.TotalDay,
//            TotalPay = x.TotalPay,
//            AgentRefId = selectCreditAgent!.Id,
//            RepaymentModeRefId = model.RepaymentModeRefId,

//        }).ToList();
//    }
//    if (paymentMode == 2)
//    {
//        var calculator = new Decline();
//        calculator.PayEvery = model.PayEvery!.Value;
//        calculator.EffectiveRate = model.CollectionFeeRate!.Value;
//        calculator.StartDate = model.StartDate!.Value;
//        calculator.FirstPayment = model.FirstPaymentDate!.Value;
//        calculator.SelectedCollectionFee = SelectedCollectionFee;
//        var schedule = calculator.Calculate(model.Installments!.Value, model.InterestRate!.Value, model.Principle!.Value);
//        return schedule.Select(x => new LoanScheduleDtoCreate
//        {
//            InstallmentNo = x.InstallmentNo,
//            PaymentDate = x.PaymentDate,
//            Principal = x.Principal,
//            Interest = x.Interest,
//            CollectionFee = x.CollectionFee,
//            LoanOutstanding = x.OutStanding,
//            TotalDay = x.TotalDay,
//            TotalPay = x.TotalPay,
//            AgentRefId = selectCreditAgent!.Id,
//            RepaymentModeRefId = model.RepaymentModeRefId,

//        }).ToList();
//    }

//    if (paymentMode == 3)
//    {
//        var calculator = new Flat();
//        calculator.PayEvery = model.PayEvery!.Value;
//        calculator.EffectiveRate = model.CollectionFeeRate!.Value;
//        calculator.StartDate = model.StartDate!.Value;
//        calculator.FirstPayment = model.FirstPaymentDate!.Value;
//        var schedule = calculator.Calculate(model.Installments!.Value, model.InterestRate!.Value, model.Principle!.Value);
//        return schedule.Select(x => new LoanScheduleDtoCreate
//        {
//            InstallmentNo = x.InstallmentNo,
//            PaymentDate = x.PaymentDate,
//            Principal = x.Principal,
//            Interest = x.Interest,
//            CollectionFee = x.CollectionFee,
//            LoanOutstanding = x.OutStanding,
//            TotalDay = x.TotalDay,
//            TotalPay = x.TotalPay,
//            AgentRefId = selectCreditAgent!.Id,
//            RepaymentModeRefId = model.RepaymentModeRefId,

//        }).ToList();
//    }
//    var cal = new Annuarity();
//    cal.PayEvery = model.PayEvery!.Value;
//    cal.EffectiveRate = model.CollectionFeeRate!.Value;
//    cal.StartDate = model.StartDate!.Value;
//    cal.FirstPayment = model.FirstPaymentDate!.Value;
//    var sch = cal.Calculate(model.Installments!.Value, model.InterestRate!.Value, model.Principle!.Value);
//    return sch.Select(x => new LoanScheduleDtoCreate
//    {
//        InstallmentNo = x.InstallmentNo,
//        PaymentDate = x.PaymentDate,
//        Principal = x.Principal,
//        Interest = x.Interest,
//        CollectionFee = x.CollectionFee,
//        LoanOutstanding = x.OutStanding,
//        TotalDay = x.TotalDay,
//        TotalPay = x.TotalPay,
//        AgentRefId = selectCreditAgent!.Id,
//        RepaymentModeRefId = model.RepaymentModeRefId,

//    }).ToList();
//}