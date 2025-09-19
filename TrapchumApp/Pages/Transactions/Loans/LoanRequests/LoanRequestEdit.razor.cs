using System.Security.Claims;
using Applications.Calculations;
using Applications.Commons.Mappers;
using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.Customers.Extensions;
using Applications.Features.LoanRequests.Interfaces;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.LoanRequests.UseCases;
using Applications.Features.Loans.Requests;
using Applications.Interfaces.Lists;
using Applications.Interfaces.Loans;
using Applications.Interfaces.setting;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Loans.Disbursements;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans.LoanRequests;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Files;
using Infrastructures.Authorizations;
using Microsoft.AspNetCore.Authorization;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Pages.Lists.Collaterals;
using TrapchumApp.Pages.Lists.OtherNames;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestEdit
    {
        private readonly ILoanSettingService _loanSettingService;
        private readonly IAppSettingService _settingsServiceImpl;
        private readonly IHolidayService _holidayService;
        private readonly ILogger<LoanRequestCreate> _logger;
        private readonly IEditLoanRequestUseCase _saveLoanRequestUseCase;
        private readonly ICreateNewLoanRequestUseCase _createNewLoanRequestUseCase;
        public LoanRequestEdit(ILoanSettingService loanSettingService, IAppSettingService settingService,
            IHolidayService holidayService, ILogger<LoanRequestCreate> logger, IEditLoanRequestUseCase saveLoanRequestUseCase, ICreateNewLoanRequestUseCase createNewLoanRequestUseCase)
        {
            _loanSettingService = loanSettingService;
            _settingsServiceImpl = settingService;
            _holidayService = holidayService;
            _logger = logger;
            _saveLoanRequestUseCase = saveLoanRequestUseCase;
            _createNewLoanRequestUseCase = createNewLoanRequestUseCase;
        }
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public string? LoanRequestId { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        IEnumerable<LoanScheduleDtoCreate> previewSchedule = [];
        IEnumerable<VCustomerList> Customers = [];
        IEnumerable<VCustomerList>? CustomersList = [];
        List<VLoanRequestGuarantor> Guarantors = [];
        List<VLoanRequestCollateralLines> CollateralLines = [];
        List<VLoanRequestsDocuments> Documents = [];
        List<VLoanRequestDescription> _loanDescriptions = [];
        IEnumerable<LoanRequestDocumentsDtoCreate> _documents = [];
        List<DateTime> Holidays = [];
        LoanRequestDtoCreate model = new();
        DateTime? RegisterDate = DateTime.Now;
        EditForm editForm = default!;
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
        bool isAllow = false;
        int SelectedCollectionFee = 1;
        decimal? TotalAdminFee = 0;
        decimal? LoanRequestAmount = 0;
        int SelectedPeriodName = 1;
        int installmentType = 1;
        int selectedTabIndex = 0;
        string? filterText = "";
        private string? _userId = string.Empty;
        IEnumerable<FileModel>? files = [];
        IEnumerable<System.IO.FileInfo> UploadFiles = [];
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

            bool? results = await DialogService.OpenAsync<GuarantorAddModal>("Add New Guarantor", options: options);

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

        async void RemoveGaurantor(VLoanRequestGuarantor g)
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
                Width = "100vw",
                Height = "100vh",
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

            _userId = auth.GetUserId();

            if (user is not null)
            {

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

                if (!reqResp.IsSuccess && reqResp.Data is null)
                {
                    NotificationService.Notify(NotificationSeverity.Error, reqResp.ErrMessage);

                    NavigationManager.NavigateTo($"/app/loan/request/{BranchId}/branches");

                    return;
                }

                var data = reqResp.Data;

                isAllow = (await _authorizeService.AuthorizeAsync(user, data, new SameUserRequestRequirement())).Succeeded;

                model.CustomerRefId = CustomerId;

                FullName = reqResp.Data?.FullName;

                CollateralLines = data!.ToLoanRequestCollaterals();

                Guarantors = data!.ToLoanRequestGuarantors();

                Documents = data!.ToLoanRequestDocuments();

                _loanDescriptions = data!.ToLoanRequestDecriptions();

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


            var customerResponse = await _customerService.GetCustomersAsync(customerId, queryParam);

            if (customerResponse.Items.Any())
            {
                Customers = customerResponse.Items;
            }
            else
            {
                Customers = [];
            }
        }


        protected async Task HandleSaveLoanRequest()
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

            if (editForm.EditContext!.Validate() == false)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Please fill all required fields.");
                return;
            }

            if (model.Memo.IsNotNullOrEmpty())
            {

                var descriptions = new LoanDescriptionsDtoCreate(model.Memo, _userId, LoanRequestId);

                model.LoanDescriptions.Add(descriptions);
            }

            try
            {
                model.BankRefId = 11;

                model.LoanSubTypeRefId = 1;

                model.BranchRefId = BranchId;

                //model.RequestUserRefId = _userId;

                //model.CreditAgentRefId = _user.EmployeeRefId;

                //model.CustomerRefId = CustomerId;

                model.RequestDate = DateTime.Now;

                model.LoanRequestStatusRefId = (int)state;

                model.BranchRefId = BranchId;

                model.AprovedAmount = model.RequestAmount;

                var resp = state switch 
                {
                    RequestStatus.New => await _saveLoanRequestUseCase.ExecuteAsync(model),
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



    }
}
