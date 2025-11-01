using Applications.Enumerations;
using Applications.Extensions.DateTimes;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Response;
using Applications.Features.Customers.UseCases;
using Applications.Features.LoanPayments.Requests;
using Applications.Features.LoanRequests.Interfaces;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Requests;
using Applications.Interfaces.Lists;
using Applications.Interfaces.Loans;
using Applications.Interfaces.setting;
using Applications.Responses.Auths;
using Applications.Responses.Loans.Disbursements;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Responses.ViewModels.Options;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Files;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Pages.Lists.Collaterals;
using TrapchumApp.Pages.Lists.OtherNames;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestCreate
    {
        private readonly ILoanSettingService _loanSettingService;
        private readonly IAppSettingService _settingsServiceImpl;
        private readonly ICreateNewLoanRequestUseCase _createNewLoanRequestUseCase;
        private readonly ISaveLoanRequestUseCase _saveLoanRequestUseCase;
        private readonly IHolidayService _holidayService;
        private readonly ILogger<LoanRequestCreate> _logger;
        private readonly GetCustomersUseCase _getCustomersUseCase;
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public string? CustomerId { get; set; } = string.Empty;
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        IEnumerable<LoanScheduleDtoCreate> previewSchedule = [];

        IEnumerable<ApprovedUser> AprovedUsers = [];

        IEnumerable<VCustomerList> _customers = [];

        IEnumerable<LoanRepaymentModeDto>? LoanRepaymentModes = [];

        IEnumerable<LoanScheduleStatusDto>? LoanScheduleStatus = [];

        IEnumerable<LoanStatusDto>? LoanStatuses = [];

        IEnumerable<LoanCategoryResponse>? loanCategories = [];

        IEnumerable<VCustomerDetails>? CustomersList = [];

        IEnumerable<LoanInstallmentTypeDto>? LoanInstallments = [];

        List<VLoanRequestGuarantor> Guarantors = [];

        List<VLoanRequestCollateralLines> requestCollateralLines = [];

        List<VLoanRequestsDocuments> Documents = [];

        IEnumerable<LoanRequestDocumentsDtoCreate> _documents = [];

        List<DateTime> Holidays = [];

        LoanRequestDtoCreate model = new();

        DateTime? RegisterDate = DateTime.Now;

        EditForm editForm = default!;
        LoanInstallmentTypeDto? SelectedInstallment { get; set; }

        VCustomerDetails? SelectedCustomer = new();

        LoanRequestDocument DocumentRef = default!;

        LoanRequestGaurantor GuarantorRef = default!;

        LoanRequestCollaterals CollateralRef = default!;

        VCustomerDetails? selectedCustomer = new();
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
        private string? _userId = string.Empty;
        public ActionMode ActionMode { get; set; }
        IEnumerable<FileModel>? files = [];

        IEnumerable<System.IO.FileInfo> UploadFiles = [];

        decimal? LoanRequestAmount = 0;

        public LoanRequestCreate(
            ILoanSettingService loanSettingService,
            IAppSettingService settingService,
            IHolidayService holidayService,
            ILogger<LoanRequestCreate> logger,
            ISaveLoanRequestUseCase saveLoanRequestUseCase,
            ICreateNewLoanRequestUseCase createNewLoanRequestUseCase,
            GetCustomersUseCase getCustomersUseCase)
        {
            _holidayService = holidayService;
            _logger = logger;
            _loanSettingService = loanSettingService;
            _settingsServiceImpl = settingService;
            _createNewLoanRequestUseCase = createNewLoanRequestUseCase;
            _saveLoanRequestUseCase = saveLoanRequestUseCase;
            _getCustomersUseCase = getCustomersUseCase;
        }

        void SelectTabIndexChanged(int tabIndex)
        {
            if (tabIndex == 1 && selectedCustomer?.CustomerId == string.Empty)
            {
                return;
            }
        }


        async Task HandleFileDialogue()
        {
            var options = new DialogOptions
            {
            };
            IEnumerable<FileModel> uploadedFiles =
                await DialogService.OpenAsync<FileUploadDialogue>("Upload", options: options);

            if (uploadedFiles is null || !uploadedFiles.Any())
            {
                return;
            }

            if (uploadedFiles.Any())
            {
                foreach (var f in uploadedFiles)
                {
                    Documents.Add(new VLoanRequestsDocuments
                    {
                        OriginalName = f.OriginalName,
                        NewFileName = f.FileName,
                        FileUrl = f.FileUrl,
                        FileSize = f.FileSize,
                        AltText = f.OriginalName,
                        Description = f.OriginalName
                    });
                }

                DocumentRef.ReloadDocument();
            }
        }


        async void RemoveFile(VLoanRequestsDocuments doc)
        {
            var confirm = await DialogService.Confirm("Do you want to remove this document?", "Remove Document");

            if (confirm == false) return;


            if (doc is not null && doc.Id == 0)
            {
                Documents.Remove(doc);

                DocumentRef.ReloadDocument();

                NotificationService.Notify(NotificationSeverity.Success, "Document was removed.");

                return;
            }
        }

        async Task SelectMany(IEnumerable<VGuarantor> gs)
        {
            if (!gs.Any()) return;

            foreach (var g in gs)
            {
                if (Guarantors.Any(x => x.GuarantorId == g.Id)) continue;

                var item = new VLoanRequestGuarantor
                {
                    GuarantorId = g.Id,
                    FullName = g.FullName,
                    GuarantorName = g.GuarantorName,
                    Phone = g.Phone,
                    Address = g.ToString(),
                };


                Guarantors.Add(item);
            }

            await GuarantorRef.GuarantorReload();
        }

        async Task SelectedGuarantors(VGuarantor? g)
        {
            if (g is null || string.IsNullOrEmpty(g.Id)) return;

            var gs = new VLoanRequestGuarantor
            {
                GuarantorId = g.Id,
                FullName = g.FullName,
                GuarantorName = g.GuarantorName,
                Phone = g.Phone,
                Address = g.ToString(),
            };

            if (Guarantors.Any(x => x.GuarantorId == g.Id))
            {
                NotificationService.Notify(NotificationSeverity.Info, "Guarantor already exists");

                return;
            }

            Guarantors.Add(gs);

            await GuarantorRef.GuarantorReload();
        }

        async Task AddGuarantors()
        {
            var options = new DialogOptions
            {
                Width = "60vw"
            };

            bool? results = await DialogService.OpenAsync<GuarantorAddModal>("Add New Guarantor", options: options);
        }

        async void RemoveGaurantor(VLoanRequestGuarantor g)
        {
            var confirm = await DialogService.Confirm("Do you want to remove this guarantor?", "Remove Guarantor");

            if (confirm == false) return;

            if (Guarantors.Any() == false) return;

            var lastItem = Guarantors.Last();

            if (lastItem != g)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Please select the last item to remove.");
                return;
            }

            if (Guarantors.Contains(g))
            {
                Guarantors.Remove(g);

                await GuarantorRef.GuarantorReload();
            }
        }

        async Task SingleSelected(VCollateralLines? c)
        {
            if (c is null || c.Id <= 0) return;

            var item = new VLoanRequestCollateralLines
            {
                Id = c.Id,
                CollateralLineId = c.Id,
                CollateralId = c.CollateralId,
                OwnerName = c.OwnerName,
                ReferenceNo = c.ReferenceNo,
                TypeName = c.TypeName,
                Quantity = c.Quantity,
            };


            if (requestCollateralLines.Any(x => x.Id == item.Id))
            {
                NotificationService.Notify(NotificationSeverity.Info, "Collateral already exists");
                return;
            }

            requestCollateralLines.Add(item);

            await CollateralRef.CollateralReload();
        }

        async Task MultipleSelected(IEnumerable<VCollateralLines> cs)
        {
            if (!cs.Any()) return;

            foreach (var g in cs)
            {
                if (requestCollateralLines.Any(x => x.CollateralLineId == g.Id)) continue;

                var item = new VLoanRequestCollateralLines
                {
                    CollateralLineId = g.Id,
                    CollateralId = g.CollateralId,
                    OwnerName = g.OwnerName,
                    ReferenceNo = g.ReferenceNo,
                    TypeName = g.TypeName,
                    Quantity = g.Quantity,
                };

                requestCollateralLines.Add(item);
            }

            await CollateralRef.CollateralReload();
        }

        async void AddCollaterals()
        {
            var param = new Dictionary<string, object?>
            {
                { "CustomerId", model.CustomerRefId }
            };
            var options = new DialogOptions
            {
                Width = "100vw",
                Height = "100vh",
                AutoFocusFirstElement = false
            };

            await DialogService.OpenAsync<CollateralModalAdd>("Collateral", param, options);
        }

        async void RemovedCollateral(VLoanRequestCollateralLines c)
        {
            var confirm = await DialogService.Confirm("Do you want to remove this collateral?", "Remove Collateral");

            if (confirm == false) return;

            if (requestCollateralLines.Any() == false) return;

            var lastItem = requestCollateralLines.Last();

            if (lastItem != c)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Please select last item to remove.");

                return;
            }

            if (requestCollateralLines.Contains(c))
            {
                requestCollateralLines.Remove(c);
            }

            await CollateralRef.CollateralReload();
        }


        protected override async Task OnInitializedAsync()
        {

            _userId = (await AuthenticationState).GetUserId();

            var rsp1 = await _userService.GetUserByIdAsync(_userId, CancellationToken.None, "EmployeeRefId");

            if (rsp1 is { IsSuccess: true, Data: not null })
            {
                model.CreditAgentRefId = _user.EmployeeRefId;
            }

            SetDefault();

            await base.OnInitializedAsync();
        }


        private async Task GetCustomers(string? customerId, int take = 25, string? filter = null)
        {
            var queryParam = new QueryParam(0, take, string.Empty, filter: filter);

            var customerResponse = await _getCustomersUseCase!.ExecuteAsync(0, null, customerId, queryParam);

            if (customerResponse.Items.Any())
            {
                _customers = customerResponse.Items;
            }
            else
            {
                _customers = [];
            }
        }


        protected async Task HandleCreateNew()
        {
            if (editForm.EditContext?.Validate() == false)
            {
                return;
            }

            var confirm = await DialogService.Confirm("Do you want to save this loan request.", "Loan Request");

            if (confirm == false)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Loan request was cancel.");

                return;
            }

            await HandleLoanRequest(RequestStatus.New);
        }

        protected async Task HandleCreateRequest()
        {
            if (editForm.EditContext?.Validate() == false)
            {
                return;
            }

            var confirm = await DialogService.Confirm("Do you want to save this loan request.", "Loan Request");

            if (confirm == false)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Loan request was cancel.");

                return;
            }

            await HandleLoanRequest(RequestStatus.Requested);
        }

        private async Task HandleLoanRequest(RequestStatus state)
        {
            _logger.LogInformation("get start loan requesting...");

            var oldGuarantors = Guarantors.ToGuarantorDtoCreate(BranchId);

            if (!string.IsNullOrEmpty(model.Memo))
            {
                var descriptions = new LoanDescriptionsDtoCreate(model.Memo, _userId);

                model.LoanDescriptions.Add(descriptions);
            }

            model.GuarantorsLink.AddRange(oldGuarantors);

            var oldCollaterals = requestCollateralLines.ToCollateralLinkDtoCreate(BranchId);

            model.CollateralLinesLink.AddRange(oldCollaterals);

            var documents = Documents.LoanDocumentDtoCreate();

            model.LoanDocuments.AddRange(documents);

            editForm.EditContext!.Validate();

            try
            {
                model.BankRefId = 11;
                model.LoanSubTypeRefId = 1;
                model.BranchRefId = BranchId;
                model.RequestUserRefId = _userId;
                model.CreditAgentRefId = _user.EmployeeRefId;
                model.RequestDate = DateTime.UtcNow;
                model.LoanRequestStatusRefId = (int)state;
                model.BranchRefId = BranchId;
                model.AprovedAmount = model.RequestAmount;

                var resp = state switch
                {
                    RequestStatus.New => await _createNewLoanRequestUseCase.ExecuteAsync(model),

                    RequestStatus.Requested => await _createNewLoanRequestUseCase.ExecuteAsync(model),

                    _ => await _saveLoanRequestUseCase.ExecuteAsync(model)

                };

                if (resp.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, resp.ErrMessage);

                    NavigationManager.NavigateTo($"/app/loan/request/{BranchId}/branches");
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);

                _logger.LogError(ex, ex.Message);
            }
        }


        void SelectedInstallmentChanged(object? arg)
        {
            if (arg is not null)
            {
                var installType = (Items)arg;
                installmentType = installType.Value;
                model.InstallmentType = installType.Label;
                ChangedDate();
            }
        }


        private async Task GetHolidays(int period)
        {
            var cur = DateTime.Now;
            period += 2;
            var fut = cur.AddMonths(period);
            var hols = await _holidayService.GetHolidays(false, cur, fut);
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

            model.FirstPaymentDate = firstPayment;
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
            previewSchedule = [];
            model.FirstPaymentDate = DateTime.UtcNow.AddMonths(1);
            installmentType = 1;
        }
    }
}


//void PrincipleValueChanged(decimal? principle)
//{
//    if (!principle.HasValue || principle < 0)
//    {
//        NotificationService.Notify(NotificationSeverity.Warning, "Invalid principle input");
//        model.RequestAmount = 0.00M;
//        return;
//    }

//    if (principle <= 50000)
//    {
//        model.CurrencyRefId = 2;
//        model.AdminFeeAmount = (principle * (model.AdminFeeRate / 100)).RoundUp(2);
//        return;
//    }
//    else
//    {
//        model.CurrencyRefId = 1;
//        model.AdminFeeAmount = (principle * (model.AdminFeeRate / 100)).RoundUp(-2);
//        return;
//    }
//}

//async Task PeriodChanged()
//{
//    await GetHolidays(model.Installments!.Value);
//}


//void ChangedDate(DateTime? date)
//{
//    if (installmentType == 1)
//    {
//        firstPayment = date!.Value.AddMonths(1);
//    }

//    if (installmentType == 2)
//    {
//        firstPayment = date!.Value.AddDays(14);
//    }

//    if (installmentType == 3)
//    {
//        firstPayment = date!.Value.AddDays(7);
//    }

//    if (installmentType == 4)
//    {
//        firstPayment = date!.Value.AddDays(1);
//    }

//    HolidayHelper.SetHoliday(Holidays, ref firstPayment);

//    model.FirstPaymentDate = firstPayment;
//}

//void CalculateAdminFeeAmount(decimal? value)
//{
//    if (model.CurrencyRefId == 1)
//    {
//        model.AdminFeeAmount = (model.RequestAmount * (value / 100)).RoundUpKhr(2);
//    }
//    else
//    {
//        model.AdminFeeAmount = (model.RequestAmount * (value / 100)).RoundUp(2);
//    }
//}

//async Task GetLoanRequestInfo()
//{
//    try
//    {
//        isLoading = true;


//        if (CustomerId.IsNullOrEmpty())
//        {
//            await GetCustomers(CustomerId, 10);

//            SetDefault();

//            isLoading = false;

//            return;
//        }

//        if (CustomerId.IsNotNullOrEmpty())
//        {
//            await GetCustomers(CustomerId, 1);

//            model.CustomerRefId = CustomerId;

//            FullName = Customers.FirstOrDefault()?.FullName;

//            model.FullName = Customers.FirstOrDefault()?.FullName;

//            model.CustomerRefId = CustomerId;

//            SetDefault();

//            isLoading = false;

//            return;
//        }
//    }
//    catch (Exception ex)
//    {
//        NotificationService.Notify(NotificationSeverity.Error, ex.Message);
//    }
//}

//void SelectedChargeModeChanged(object e)
//{
//    if (e is not null)
//    {
//        var selectedValue = (Items)e;

//        Model.CollectionFeeModeName = selectedValue.Label;
//    }
//    else
//    {
//        Model.CollectionFeeModeId = 1;
//        Model.CollectionFeeModeName = "Flat";
//    }
//}

//void RepaymentModeSelectedChanged(object e)
//{
//    var selectedValue = (Items)e;

//    Model.RepaymentMode = selectedValue.Label;

//    if (selectedValue.Value == 1)
//    {
//        Model.PayEvery = 4;
//    }
//    else
//    {
//        Model.PayEvery = 1;
//    }
//}

//LoanCaloulator SelectLoanType(int? paymentMode)
//{
//    return paymentMode switch
//    {
//        1 => new Balloon(),
//        2 => new Decline(),
//        3 => new Flat(),
//        4 => new Annuarity(),
//        _ => new Flat(),
//    };
//}

//async Task GetCustomerRequest(string? customerId)
//{
//    if (customerId.IsNotNullOrEmpty())
//    {
//        var customerResponse = await _customerService.GetCustomerForRequestLoanAsync(customerId);

//        if (customerResponse.IsSuccess)
//        {
//            SelectedCustomer = customerResponse.Data!;
//        }
//    }
//}


//async void SelectCustomerChanged(object? e)
//{
//    if (e is not null)
//    {
//        CustomerId = ((VCustomerList)e).CustomerId;

//        await GetCustomerRequest(CustomerId);
//    }
//}
//void ShowLoanPreviews()
//{
//    if (Model.RequestAmount == 0) return;
//    selectedTabIndex = 2;
//    var calculator = SelectLoanType(3);
//    calculator.PayEvery = 1;
//    calculator.EffectiveRate = Model.CollectionFeeRate!.Value;
//    calculator.StartDate = DateTime.Now;
//    calculator.FirstPayment = DateTime.Now.AddMonths(1);
//    calculator.CollectionFeeMode = SelectedCollectionFee;
//    calculator.InstallmentTypeId = 1;
//    calculator.CurrencyId = Model.CurrencyRefId!.Value;
//    var schedule = calculator.Calculate(Model.Installments, Model.InterestRate, Model.RequestAmount);
//    previewSchedule = schedule.Select(x => new LoanScheduleDtoCreate
//    {
//        InstallmentNo = x.InstallmentNo,
//        PaymentDate = x.PaymentDate,
//        Principal = x.Principal,
//        Interest = x.Interest,
//        CollectionFee = x.CollectionFeeAmount,
//        LoanOutstanding = x.OutStanding,
//        TotalDay = x.TotalDay,
//        TotalPay = x.TotalPay,
//        AgentRefId = UserId,
//        ScheduleStatusRefId = 2
//    }).ToList();
//}
//if (doc is not null && (LoanRequestId.IsNotNullOrEmpty() && doc.Id > 0))
//{
//    var resp = await _loanRequestService.DeleteLoanRequestDocumentAsync(LoanRequestId, BranchId, doc.Id);

//    if (resp.IsSuccess)
//    {
//        Documents.Remove(doc);

//        DocumentRef.ReloadDocument();

//        NotificationService.Notify(NotificationSeverity.Success, "Document was removed.");

//        return;
//    }

//    NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
//}


//if (LoanRequestId.IsNotNullOrEmpty())
//{
//    if (!Documents.Any())
//    {
//        NotificationService.Notify(NotificationSeverity.Error, "There is no any file to upload");
//        return;
//    }

//    var documents = LoanDocumentsExtensions.ToLoanDocumentsCreate(CustomerId, LoanRequestId, BranchId, Documents);

//    var res = await _loanRequestService.AddLoanRequestDocumentAsync(documents);

//    if (!res.IsSuccess)
//    {
//        NotificationService.Notify(NotificationSeverity.Error, res.ErrMessage);

//        return;
//    }

//    await GetLoanRequestDocuments();

//    NotificationService.Notify(NotificationSeverity.Success, res.Data);
//}
//if (CollateralLines.Any() && c.Id > 0)
//{
//    var res = await _loanRequestService.DeleteLoanRequestCollateralAsync(BranchId, LoanRequestId, c.Id);

//    if (res.IsSuccess)
//    {
//        NotificationService.Notify(NotificationSeverity.Success, "Collateral was removed.");
//        CollateralLines.Remove(c);
//        CollateralRef.CollateralReload();
//    }
//}        //async Task GetLoanRequestDocuments()
//{
//    var resp = await _loanRequestService.GetLoanRequestDocumentAsync(LoanRequestId, BranchId);

//    if (resp.IsSuccess)
//    {
//        Documents = resp.Data!.ToList();

//        DocumentRef.ReloadDocument();
//    }
//}
//if (Guarantors.Any() && g.Id > 0)
//{
//    var res = await _loanRequestService.DeleteLoanRequestGuarantorAsync(LoanRequestId, BranchId, g.Id);

//    if (res.IsSuccess)
//    {
//        Guarantors.Remove(g);

//        await GuarantorRef.GuarantorReload();

//    }

//}