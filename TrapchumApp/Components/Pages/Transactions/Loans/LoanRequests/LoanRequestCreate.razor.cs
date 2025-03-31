using Applications.Enumerations;
using Applications.Extensions;
using Applications.Extensions.DateTimes;
using Applications.Extensions.LoanCalculators;
using Applications.Extensions.mappers;
using Applications.Extensions.Maths;
using Applications.Interfaces.Lists;
using Applications.Interfaces.Loans;
using Applications.Interfaces.setting;
using Applications.Requests.Lists.Entities.Guarantors;
using Applications.Requests.Loans.Disbursements;
using Applications.Requests.Loans.LoanRequests;
using Applications.Requests.Loans.Payments;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.Loans.Disbursements;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Responses.ViewModels.Options;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Files;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using System.Web;
using Applications.Responses.ViewModels.Customers;
using TrapchumApp.Components.Pages.Lists.Collaterals;
using TrapchumApp.Components.Pages.Lists.OtherNames;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestCreate
    {
        private readonly ILoanSettingService _loanSettingService;
        private readonly ISettingService _settingsServiceImpl;
        private readonly IHolidayService _holidayService;

        public LoanRequestCreate(ILoanSettingService loanSettingService, ISettingService settingService,
            IHolidayService holidayService)
        {
            _loanSettingService = loanSettingService;
            _settingsServiceImpl = settingService;
            _holidayService = holidayService;
        }

        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter] public Guid? LoanRequestId { get; set; }
        [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
        [Parameter, SupplyParameterFromQuery] public Guid? UserId { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        IEnumerable<LoanScheduleDtoCreate> previewSchedule = [];
        IEnumerable<AprovePerson> AprovedPeople = [];
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
        LoanRequestDtoCreate model = new();
        DateTime? RegisterDate = DateTime.Now;
        EditForm editForm = default!;
        LoanInstallmentTypeDto? SelectedInstallment { get; set; }
        VCustomerList? SelectedCustomer = new();
        LoanRequestDocument DocumentRef = default!;
        LoanRequestGaurantor GuarantorRef = default!;
        LoanRequestCollaterals CollateralRef = default!;
        VCustomerList? selectedCustomer = new();
        DateTime firstPayment;
        LoanCategoryResponse? SelectedLoanCategory;
        VUser _user = new();
        long? size = 0;
        int counts = 0;
        bool isVible = false;
        bool isLoading = false;
        int selectedTabIndex = 0;
        bool isProgressing = false;
        int SelectedCollectionFee = 1;
        decimal? TotalAdminFee = 0;
        int SelectedPeriodName = 1;
        int installmentType = 1;
        string? filterText = "";
        public ActionMode ActionMode { get; set; }
        IEnumerable<FileModel>? files = [];
        IEnumerable<System.IO.FileInfo> UploadFiles = [];
        decimal? LoanRequestAmount = 0;

        async void SelectTabIndexChanged(int tabIndex)
        {
            if (tabIndex == 1 && selectedCustomer?.CustomerId == Guid.Empty)
            {
                await GetCustomerRequest(CustomerId);
                return;
            }
        }

        async Task HandleFileDialogue()
        {
            var options = new DialogOptions
            {
                Width = "45vw"
            };

            IEnumerable<FileModel> uploadedFiles =
                await DialogService.OpenAsync<FileUploadDialogue>("Upload", options: options);

            if (uploadedFiles.Any())
            {
                foreach (var f in uploadedFiles)
                {
                    Documents.Add(new VLoanRequestsDocuments
                    {
                        OriginalName = f.OriginalName,
                        NewFileName = f.FileName,
                        FileUrl = f.FileUrl,
                        AltText = f.OriginalName,
                        Description = f.OriginalName
                    });
                }

                DocumentRef.ReloadDocument();
            }

            if (LoanRequestId.HasValue)
            {
                if (!Documents.Any())
                {
                    NotificationService.Notify(NotificationSeverity.Error, "There is any file to upload");
                    return;
                }

                var document =
                    LoanDocumentsExtensions.ToLoanRequestDocuments(CustomerId, LoanRequestId, BranchId, Documents);

                var res = await _loanRequestService.AddLoanRequestDocumentAsync(document);

                if (!res.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Error, res.Data);
                    return;
                }

                await GetLoanRequestDocuments();
                NotificationService.Notify(NotificationSeverity.Error, res.ErrMessage);
            }
        }

        async Task GetLoanRequestDocuments()
        {
            var resp = await _loanRequestService.GetLoanRequestDocumentAsync(LoanRequestId, BranchId);

            if (resp.IsSuccess)
            {
                Documents = resp.Data!.ToList();
                DocumentRef.ReloadDocument();
            }
        }

        void CurrencySelectedChanged(object e)
        {
            var selectedValue = (Items)e;
            Console.WriteLine(selectedValue.Value);
            StateHasChanged();
        }

        async void RemoveFile(VLoanRequestsDocuments doc)
        {
            var confirm = await DialogService.Confirm("Do you want to remove this document?", "Remove Document");

            if (confirm == false) return;

            if (doc is not null && (LoanRequestId.NotEmpty() && doc.Id > 0))
            {
                var resp = await _loanRequestService.DeleteLoanRequestDocumentAsync(LoanRequestId, BranchId, doc.Id);

                if (resp.IsSuccess)
                {
                    Documents.Remove(doc);

                    DocumentRef.ReloadDocument();

                    NotificationService.Notify(NotificationSeverity.Success, "Document was removed.");

                    return;
                }

                NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
            }

            if (doc is not null && (LoanRequestId.NotEmpty() || doc.Id == 0))
            {
                Documents.Remove(doc);

                DocumentRef.ReloadDocument();

                NotificationService.Notify(NotificationSeverity.Success, "Document was removed.");

                return;
            }
        }

        async Task AddGuarantors()
        {
            var options = new DialogOptions
            {
                Width = "60vw"
            };

            IList<VGuarantor> results =
                await DialogService.OpenAsync<GuarantorsList>("Select Guarantor", options: options);

            if (results.Any())
            {
                var gs = results.Select(g => new VLoanRequestGuarantors
                {
                    EntId = g.Id,
                    FullName = g.FullName,
                    AltName = g.GuarantorName,
                    Phone = g.Phone,
                    VilInKhmer = g.VilInKhmer,
                    ComInKhmer = g.ComInKhmer,
                    DisInKhmer = g.DisInKhmer,
                    ProInKhmer = g.ProInKhmer
                });
                foreach (var item in gs)
                {
                    if (Guarantors.Contains(item)) continue;

                    Guarantors.Add(item);
                }

                await GuarantorRef.GuarantorReload();
            }
        }

        async void RemoveGaurantor(VLoanRequestGuarantors g)
        {
            var confirm = await DialogService.Confirm("Do you want to remove this guarantor?", "Remove Guarantor");

            if (confirm == false) return;

            if (Guarantors.Any() && g.Id > 0)
            {
                var res = await _loanRequestService.DeleteLoanRequestGuarantorAsync(LoanRequestId, BranchId, g.Id);
                if (res.IsSuccess)
                {
                    Guarantors.Remove(g);
                    await GuarantorRef.GuarantorReload();
                }
            }

            if (Guarantors.Any() && g.Id == 0)
            {
                Guarantors.Remove(g);
                await GuarantorRef.GuarantorReload();
            }
        }

        async void AddCollaterals()
        {
            var param = new Dictionary<string, object?>
            {
                { "CustomerId", model.CustomerRefId }
            };
            var options = new DialogOptions
            {
                Width = "60vw"
            };

            IList<VCollateralLines> results =
                await DialogService.OpenAsync<CollateralLoanRequest>("Collateral", param, options);
            if (results.Any())
            {
                var collateralLines = results.Select(c => new VLoanRequestCollateralLines
                {
                    CollateralLineId = c.Id,
                    CollateralId = c.CollateralId!.ToGuid(),
                    OwnerName = c.OwnerName,
                    ReferenceNo = c.ReferenceNo,
                }).ToList();
                foreach (var item in collateralLines)
                {
                    if (CollateralLines.Contains(item)) continue;
                    CollateralLines.Add(item);
                }

                CollateralRef.CollateralReload();
            }
        }

        async void RemovedCollateral(VLoanRequestCollateralLines c)
        {
            var confirm = await DialogService.Confirm("Do you want to remove this collateral?", "Remove Collateral");

            if (confirm == false) return;

            if (CollateralLines.Any() && c.Id > 0)
            {
                var res = await _loanRequestService.DeleteLoanRequestCollateralAsync(BranchId, LoanRequestId, c.Id);

                if (res.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Collateral was removed.");
                    CollateralLines.Remove(c);
                    CollateralRef.CollateralReload();
                }
            }

            if (CollateralLines.Any() && c.Id == 0)
            {
                CollateralLines.Remove(c);
                CollateralRef.CollateralReload();
            }
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

        protected override async Task OnParametersSetAsync()
        {
            //LoanRepaymentModes = await _loanSettingService.GetLoanRepaymentMode();

            //LoanInstallments = await _loanSettingService.GetLoanInstallmentTypes();

            var cat = await _loanSettingService.GetLoanCategories();


            if (cat!.Any())
            {
                loanCategories = cat.Maps<LoanCategoryResponse>();

                SelectedLoanCategory = loanCategories!.FirstOrDefault();
            }

            var aproveResp = await _loanSettingService.GetAprovedUserAsync(BranchId);
            
            AprovedPeople = aproveResp ?? [];

            var userResp = await _userService.GetUsersAsync(UserId.ToString());

            if (userResp.IsSuccess)
            {
                _user = userResp.Data!.First();
            }

            await base.OnParametersSetAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            var uri = new Uri(NavigationManager.Uri);

            var queryParam = HttpUtility.ParseQueryString(uri.Query).Get(nameof(ActionMode));

            if (Enum.TryParse(queryParam, true, out ActionMode result))
            {
                ActionMode = result;
            }

            await GetLoanRequestInfo(ActionMode);

            await base.OnInitializedAsync();
        }

        async Task GetLoanRequestInfo(ActionMode action)
        {
            try
            {
                isLoading = true;

                if (action == ActionMode.Edit)
                {
                    await GetCustomers(CustomerId, 1);

                    var reqResp =
                        await _loanRequestService.GetLoanRequestByCustomerAsync(BranchId, CustomerId, LoanRequestId,
                            true);

                    if (reqResp.IsSuccess)
                    {
                        var data = reqResp.Data;
                        model.CustomerRefId = CustomerId;
                        CollateralLines = data?.Collaterals.ToList() ?? [];
                        Guarantors = data?.Guarantors.ToList() ?? [];
                        Documents = data?.Documents.ToList() ?? [];
                        model = data!.ToLoanRequestDtoCreate();
                    }

                    isLoading = false;

                    return;
                }

                if (action == ActionMode.Create && !CustomerId.NotEmpty())
                {
                    await GetCustomers(CustomerId, 10);

                    SetDefault();

                    isLoading = false;

                    return;
                }

                if (action == ActionMode.Create && CustomerId.NotEmpty())
                {
                    await GetCustomers(CustomerId, 1);

                    model.CustomerRefId = CustomerId;

                    SetDefault();

                    isLoading = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }

        private async Task GetCustomers(Guid? customerId, int take = 10, string? filter = null)
        {
            var customerResponse = await _customerService!.GetCustomersAsync(customerId, take, false, true, filter);

            if (customerResponse.IsSuccess)
            {
                Customers = customerResponse.Data!;
            }
            else
            {
                Customers = [];

                NotificationService.Notify(NotificationSeverity.Error, customerResponse.ErrMessage);
            }
        }

        private async Task LoadCustomers(LoadDataArgs e)
        {
            //
            var filterText = e.Filter ?? "";

            var customersResponse =
                await _customerService!.GetCustomersAsync(null, e.Top!.Value, false, false, filterText);

            if (!customersResponse.IsSuccess)
            {
                Customers = [];

                NotificationService.Notify(NotificationSeverity.Error, customersResponse.ErrMessage);
            }
            else
            {
                Customers = customersResponse.Data!;
            }
        }

        protected async Task HandleCreateNew()
        {
            editForm.EditContext?.Validate();

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
            editForm.EditContext?.Validate();

            var confirm = await DialogService.Confirm("Do you want to save this loan request.", "Loan Request");

            if (confirm == false)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Loan request was cancel.");

                return;
            }

            await HandleLoanRequest(RequestStatus.Requested);
        }

        protected async Task HandleLoanRequest(RequestStatus state)
        {
            var oldGuarantors = Guarantors.Select(x => new LoanGuarantorDtoCreate
            {
                Id = x.Id,
                BranchId = BranchId,
                EntityNameRefId = x.EntId,
                LoanRequestRefId = x.LoanRequestId,
                IsActive = true
            }).ToList() ?? [];

            var oldCollaterals = CollateralLines.Select(c =>
                new Applications.Requests.Loans.LoanRequests.CollateralLinkDtoCreate
                {
                    Id = c.Id,
                    BranchId = BranchId,
                    LoanRequestRefId = c.LoanRequestId,
                    CollateralRefId = c.CollateralId,
                    CollateralLineRefId = c.CollateralLineId,
                    IsActive = true
                }).ToList() ?? [];

            var documents = Documents.LoanDocumentDtoCreate(BranchId, LoanRequestId);
            //model.LoanDocuments.AddRange(documents);
            model.CollateralLinesLink.AddRange(oldCollaterals);
            model.GuarantorsLink.AddRange(oldGuarantors);
            editForm.EditContext!.Validate();
            try
            {
                model.BankRefId = 11;
                model.LoanSubTypeRefId = 1;
                model.BranchRefId = BranchId;
                model.RequestUserRefId = UserId;
                model.CreditAgentRefId = _user.EmployeeRefId;
                model.RequestDate = DateTime.UtcNow;
                model.LoanRequestStatusRefId = (int)state;
                model.BranchRefId = BranchId;
                model.AprovedAmount = model.RequestAmount;

                var resp = await _loanRequestService.CreateLoanRequestAsync(model);

                if (resp.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Error, resp.Data);
                    NavigationManager.NavigateTo($"/Loan/Request?BranchId={BranchId}&UserId={UserId}");
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

        LoanCaloulator SelectLoanType(int? paymentMode)
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
            var schedule = calculator.Calculate(model.Installments, model.InterestRate, model.RequestAmount);
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
                AgentRefId = UserId,
                ScheduleStatusRefId = 2
            }).ToList();
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


        async void SelectCustomerChanged(object? e)
        {
            if (e is not null)
            {
                CustomerId = ((VCustomerList)e).CustomerId;

                await GetCustomerRequest(CustomerId);
            }
        }

        async Task GetCustomerRequest(Guid? customerId)
        {
            if (customerId.HasValue)
            {
                var customerResponse = await _customerService.GetCustomerForRequestLoanAsync(customerId);

                if (customerResponse.IsSuccess)
                {
                    SelectedCustomer = customerResponse.Data!;
                }
            }
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

        void SelectedPeriodChanged()
        {
            ChangedDate();
        }

        async Task PeriodChanged()
        {
            await GetHolidays(model.Installments!.Value);
        }

        private async Task GetHolidays(int period)
        {
            var cur = DateTime.Now;
            period += 2;
            var fut = cur.AddMonths(period);
            var hols = await _holidayService.GetHolidays(false, cur, fut);
            Holidays = hols.Select(x => x.HolidayName).ToList();
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
            previewSchedule = [];
            model.FirstPaymentDate = DateTime.UtcNow.AddMonths(1);
            installmentType = 1;
        }
    }
}