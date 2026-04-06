using Applications.Calculations;
using Applications.Commons.Mappers;
using Applications.Commons.Mappers.Notifications;
using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Extensions;
using Applications.Features.Customers.Queries.GetCustomerAsPaging;
using Applications.Features.Customers.Response;
using Applications.Features.Guarantor.Dtos;
using Applications.Features.LoanRequests.Command.AddLoanRequestDocument;
using Applications.Features.LoanRequests.Command.DeleteLoanRequestCollateral;
using Applications.Features.LoanRequests.Command.DeleteLoanRequestDocument;
using Applications.Features.LoanRequests.Command.DeleteLoanRequestGuarantor;
using Applications.Features.LoanRequests.Command.SaveEditLoanRequest;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestDetailById;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestsDocuments;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Requests;
using Applications.Features.Notifications.Command.CreateNotification;
using Applications.Features.Notifications.Dto;
using Applications.Interfaces.Lists;
using Applications.Interfaces.Loans;
using Applications.Interfaces.setting;
using Applications.Responses.Loans.Disbursements;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Loans.LoanRequests;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Enumerations;
using Domains.Models.Files;
using Infrastructures.Authorizations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Pages.Lists.Collaterals;
using TrapchumApp.Pages.Lists.OtherNames;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
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
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public string? LoanRequestId { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [CascadingParameter] CascadingAppState bState { get; set; } = default!;
        IEnumerable<LoanScheduleDtoCreate> previewSchedule = [];
        IEnumerable<VCustomerList> _customers = [];
        IEnumerable<VCustomerDetails>? CustomersList = [];
        List<VLoanRequestGuarantor> Guarantors = [];
        List<VLoanRequestCollateralLines> CollateralLines = [];
        List<VLoanRequestsDocuments> Documents = [];
        List<VLoanRequestDescription> _loanDescriptions = [];
        IEnumerable<LoanRequestDocumentsDtoCreate> _documents = [];
        List<DateTime> Holidays = [];
        LoanRequestDtoCreate model = new();
        DateTime? RegisterDate = DateTime.Now;
        EditForm editForm = default!;
        VCustomerDetails? SelectedCustomer = new();
        LoanRequestDocument DocumentRef = default!;
        LoanRequestGaurantor GuarantorRef = default!;
        LoanRequestCollaterals CollateralRef = default!;
        List<VCollateralLines> _collateralLines = [];
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

            if (string.IsNullOrEmpty(LoanRequestId))
            {
                return;
            }

            if (!Documents.Any())
            {
                NotificationService.Notify(NotificationSeverity.Error, "There is no any file to upload");

                return;
            }

            var documents = Documents.ToLoanDocumentsEdit();

            var res = await mediator.Send(new AddLoanRequestDocumentsCommand(documents));

            if (!res.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Error, res.GetMessage());
                return;
            }

            await GetLoanRequestDocuments();

            NotificationService.Notify(NotificationSeverity.Success, res.Value);

        }

        async Task GetLoanRequestDocuments()
        {

            var query = new GetLoanRequestDocumentQuery(LoanRequestId, BranchId);

            var resp = await mediator.Send(query);

            if (!resp.IsError)
            {
                Documents = resp.Value.ToList();

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

                var command = new DeleteLoanRequestDocumentCommand(LoanRequestId, BranchId, doc.Id);

                var resp = await mediator.Send(command);

                if (!resp.IsError)
                {
                    Documents.Remove(doc);

                    DocumentRef.ReloadDocument();

                    NotificationService.Notify(NotificationSeverity.Success, "Document was removed.");

                    return;
                }

                NotificationService.Notify(NotificationSeverity.Error, resp.FirstError.Description);
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

            var dia = await DialogService.OpenAsync<GuarantorAddModal>("Add New Guarantor", options: options);

            if (dia is VGuarantor g && g is not null)
            {

            }
        }

        async void RemoveGaurantor(VLoanRequestGuarantor g)
        {
            var confirm = await DialogService.Confirm("Do you want to remove this guarantor?", "Remove Guarantor");

            if (confirm == false) return;

            if (Guarantors.Any() && g.Id > 0)
            {

                var command = new DeleteLoanRequestGuarantorCommand(LoanRequestId, BranchId, g.Id);

                var res = await mediator.Send(command);

                if (!res.IsError)
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
                { "CollateralId", model.CustomerRefId }
            };
            var options = new DialogOptions
            {

            };

            await DialogService.OpenAsync<CustomerCollateralLineCreate>("Collateral", param, options);
        }

        async void RemovedCollateral(VLoanRequestCollateralLines c)
        {
            var confirm = await DialogService.Confirm("Do you want to remove this collateral?", "Remove Collateral");

            if (confirm == false) return;

            if (CollateralLines.Any() && c.Id > 0)
            {

                var command = new DeleteLoanRequestCollateralCommand(BranchId, LoanRequestId, c.Id);

                var res = await mediator.Send(command);

                if (!res.IsError)
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

                var query = new GetLoanRequestDetailByIdQuery(BranchId, CustomerId, LoanRequestId, true);

                var reqResp = await mediator.Send(query);

                if (reqResp.IsError && reqResp.Value is null)
                {
                    NotificationService.Notify(NotificationSeverity.Error, reqResp.FirstError.Description);

                    NavigationManager.NavigateTo($"/App/Loan/Requests/{BranchId}/Branches");

                    return;
                }

                var data = reqResp.Value;

                isAllow = (await _authorizeService.AuthorizeAsync(user, data, new SameUserRequestRequirement())).Succeeded;

                model.CustomerRefId = CustomerId;

                FullName = reqResp.Value?.AltName;

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

            var command = new GetCustomerAsPagingQuery(null, null, customerId, queryParam);

            var rsp = await mediator.Send(command);

            _customers = rsp.Items;

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

                model.RequestDate = DateTime.Now;

                model.LoanRequestStatusRefId = (int)state;

                model.BranchRefId = BranchId;

                model.AprovedAmount = model.RequestAmount;

                var commandSaveEdit = new SaveEditLoanRequestCommand(model);

                var resp = await mediator.Send(commandSaveEdit);

                if (resp.IsError)
                {
                    NotificationService.Notify(NotificationSeverity.Error, resp.FirstError.Description);

                    return;
                }


                var notificationCreate = new NotificationDtoCreate
                {
                    BranchId = BranchId,
                    ReferenceId = LoanRequestId,
                    UserId = _userId,
                    Title = "Loan Request",
                    Type = NotificationType.Request.ToString(),
                    CustomerId = CustomerId,
                    Message = $"Loan request for {SelectedCustomer?.FullName} has been created.",
                    Url = $"/App/Loans/Requests/Details/{BranchId}/{CustomerId}/{LoanRequestId}",
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };

                var commandCreateNoficaiton = new CreateNotificationCommand(notificationCreate);

                var rsp = await mediator.Send(commandCreateNoficaiton);

                if (rsp.IsSuccess())
                {
                    _notiService.AddNotification(notificationCreate.ToDtoAddNotification());
                }

                NotificationService.Notify(NotificationSeverity.Success, resp.Value.Message);

                NavigationManager.NavigateTo($"/App/Loans/Requests/{BranchId}/Branches");

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);

                _logger.LogError(ex, ex.Message);
            }
        }



    }
}
