using Applications.Calculations;
using Applications.Enumerations;
using Applications.Extensions;
using Applications.Extensions.LoanCalculators;
using Applications.Interfaces.Lists;
using Applications.Interfaces.Loans;
using Applications.Interfaces.setting;
using Applications.Requests.Loans.Disbursements;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Loans.Disbursements;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans.LoanRequests;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Files;
using Domains.Models.Loans;
using Infrastructures.Authorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using System.Security.Claims;
using TrapchumApp.Components.Pages.Lists.Collaterals;
using TrapchumApp.Components.Pages.Lists.OtherNames;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestEdit
    {
        private readonly ILoanSettingService _loanSettingService;
        private readonly IAppSettingService _settingsServiceImpl;
        private readonly IHolidayService _holidayService;
        private readonly ILogger<LoanRequestCreate> _logger;

        public LoanRequestEdit(ILoanSettingService loanSettingService, IAppSettingService settingService,
            IHolidayService holidayService, ILogger<LoanRequestCreate> logger)
        {
            _loanSettingService = loanSettingService;
            _settingsServiceImpl = settingService;
            _holidayService = holidayService;
            _logger = logger;
        }

        [Parameter] public string? UserId { get; set; }
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public string? CustomerId { get; set; }
        [Parameter, SupplyParameterFromQuery] public string? LoanRequestId { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        IEnumerable<LoanScheduleDtoCreate> previewSchedule = [];

        //IEnumerable<ApprovedUser> AprovedUsers = [];

        IEnumerable<VCustomerList> Customers = [];
        IEnumerable<VCustomerList>? CustomersList = [];
        List<VLoanRequestGuarantors> Guarantors = [];

        List<VLoanRequestCollateralLines> CollateralLines = [];

        List<VLoanRequestsDocuments> Documents = [];

        List<VLoanRequestDescription> _loanDescriptions = []; 

        IEnumerable<LoanRequestDocumentsDtoCreate> _documents = [];

        List<DateTime> Holidays = [];

        LoanRequestDtoCreate model = new();

        DateTime? RegisterDate = DateTime.Now;

        EditForm editForm = default!;
        //LoanInstallmentTypeDto? SelectedInstallment { get; set; }

        VCustomerList? SelectedCustomer = new();

        LoanRequestDocument DocumentRef = default!;

        LoanRequestGaurantor GuarantorRef = default!;

        LoanRequestCollaterals CollateralRef = default!;

        List<VCollateralLines> _collateralLines = [];

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
        bool isAllow = false;
        IEnumerable<FileModel>? files = [];

        IEnumerable<System.IO.FileInfo> UploadFiles = [];

        decimal? LoanRequestAmount = 0;

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

            IEnumerable<FileModel>? uploadedFiles = await DialogService.OpenAsync<FileUploadDialogue>("Upload", options: options);


            if (uploadedFiles is null || !uploadedFiles.Any())
            {
                return;
            }

            if (uploadedFiles is not null)
            {
                foreach (var f in uploadedFiles)
                {
                    Documents.Add(new VLoanRequestsDocuments
                    {
                        BranchRefId = BranchId,
                        LoanRequestId = LoanRequestId,
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

            if (LoanRequestId.IsNotNullOrEmpty())
            {
                if (!Documents.Any())
                {
                    NotificationService.Notify(NotificationSeverity.Error, "There is no any file to upload");

                    return;
                }

                var documents = Documents.ToLoanDocumentsEdit();

                var res = await _loanRequestService.AddLoanRequestDocumentAsync(documents);

                if (!res.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Error, res.ErrMessage);

                    return;
                }

                await GetLoanRequestDocuments();

                NotificationService.Notify(NotificationSeverity.Success, res.Data);
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


        async void RemoveFile(VLoanRequestsDocuments doc)
        {
            var confirm = await DialogService.Confirm("Do you want to remove this document?", "Remove Document");

            if (confirm == false) return;

            if (!Documents.Any()) return;

            var lastItem = Documents.Last();

            if (lastItem != doc)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Please selected last document to delete.");
                return;
            }

            if (LoanRequestId.IsNotNullOrEmpty() && doc.Id > 0)
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
            if (doc.LoanRequestId.IsNotNullOrEmpty() || doc.Id == 0)
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
                Width = "65rem"
            };

            bool? results = await DialogService.OpenAsync<GuarantorAddModal>("Select Guarantor", options: options);

            if (results is not null && results == true)
            {
                //var gs = results.Select(g => new VLoanRequestGuarantors
                //{
                //    EmployeeRefId = g.Id,
                //    FullName = g.FullName,
                //    AltName = g.GuarantorName,
                //    Phone = g.Phone,
                //    VilInKhmer = g.VilInKhmer,
                //    ComInKhmer = g.ComInKhmer,
                //    DisInKhmer = g.DisInKhmer,
                //    ProInKhmer = g.ProInKhmer
                //});
                //foreach (var item in gs)
                //{
                //    if (Guarantors.Contains(item)) continue;

                //    Guarantors.Add(item);
                //}

                //await GuarantorRef.GuarantorReload();
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

            if (Guarantors.Any() && (g.Id is null || g.Id == 0))
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
                Width = "85vw"
            };

            await DialogService.OpenAsync<CollateralModalAdd>("Collateral", param, options);
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

                    await CollateralRef.CollateralReload();
                }
            }

            if (CollateralLines.Any() && c.Id == 0)
            {
                CollateralLines.Remove(c);

                await CollateralRef.CollateralReload();
            }
        }

        protected override async Task OnInitializedAsync()
        {

            var auth = await AuthenticationState;

            var user = auth.User;

            if (user is not null) {

                await GetLoanRequestInfo(user);
            }
           
            await base.OnInitializedAsync();
        }

        async Task GetLoanRequestInfo(ClaimsPrincipal user)
        {
            try
            {
                isLoading = true;

                await GetCustomers(CustomerId, 1);

                var reqResp = await _loanRequestService.GetLoanRequestAsync(BranchId, CustomerId, LoanRequestId, true);

                if (!reqResp.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Error, reqResp.ErrMessage);
                    NavigationManager.NavigateTo($"/App/Request/{BranchId}/Branches/?UserId={UserId}");
                    return;
                }
                var data = reqResp.Data;

                isAllow = (await _authorizeService.AuthorizeAsync(user, data, new SameUserRequestRequirement())).Succeeded;

                model.CustomerRefId = CustomerId;

                FullName = reqResp.Data?.FullName;

                CollateralLines = data!.ToLoanRequestCollaterals();

                Guarantors = data!.ToLoanRequestGuarantors();

                Documents = data!.ToLoanRequestDocuments();

                _loanDescriptions=data!.ToLoanRequestDecriptions();

                model = data!.ToLoanRequestDtoCreate();

                isLoading = false;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
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
            else
            {
                Customers = [];

                NotificationService.Notify(NotificationSeverity.Error, customerResponse.ErrMessage);
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

        protected async Task HandleLoanRequest(RequestStatus state)
        {
            _logger.LogInformation("get start loan requesting...");

            if (!string.IsNullOrEmpty(model.Memo))
            {
                var descriptions = new LoanDescriptionsDtoCreate(model.Memo, UserId,LoanRequestId);

                model.LoanDescriptions.Add(descriptions);
            }
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
                    NotificationService.Notify(NotificationSeverity.Success, resp.Data);

                    NavigationManager.NavigateTo($"/App/Request/{BranchId}/Branches/?UserId={UserId}");
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
    }
}


//var uri = new Uri(NavigationManager.Uri);
//var queryParam = HttpUtility.ParseQueryString(uri.Query).Get(nameof(ActionMode));

//if (Enum.TryParse(queryParam, true, out ActionMode result))
//{
//    ActionMode = result;
//}

//IEnumerable<LoanRepaymentModeDto>? LoanRepaymentModes = [];

//IEnumerable<LoanScheduleStatusDto>? LoanScheduleStatus = [];

//IEnumerable<LoanStatusDto>? LoanStatuses = [];

//IEnumerable<LoanCategoryResponse>? loanCategories = [];
//IEnumerable<LoanInstallmentTypeDto>? LoanInstallments = [];
//if (CustomerId.IsNotNullOrEmpty() || LoanRequestId.IsNotNullOrEmpty())
//{
//    isCustomerSelected = true;
//}

//var cat = await _loanSettingService.GetLoanCategories();

//if (cat!.Any())
//{
//    loanCategories = cat.Maps<LoanCategoryResponse>();

//    SelectedLoanCategory = loanCategories!.FirstOrDefault();
//}

//var aproveResp = await _loanSettingService.GetAprovedUserAsync<ApprovedUser>(BranchId);

//AprovedUsers = aproveResp ?? [];