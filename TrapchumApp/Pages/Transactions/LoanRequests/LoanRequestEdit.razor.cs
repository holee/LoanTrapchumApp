using Applications.Calculations;
using Applications.Commons.Mappers;
using Applications.Commons.Mappers.Notifications;
using Applications.Extensions;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Extensions;
using Applications.Features.Customers.Response;
using Applications.Features.Documents.Dto;
using Applications.Features.Guarantor.Dtos;
using Applications.Features.LoanRequests.Command.AddCoPawnCustomer;
using Applications.Features.LoanRequests.Command.AddCoPawnCustomers;
using Applications.Features.LoanRequests.Command.AddLoanRequestDocument;
using Applications.Features.LoanRequests.Command.DeleteCoPawnCustomer;
using Applications.Features.LoanRequests.Command.DeleteLoanRequestCollateral;
using Applications.Features.LoanRequests.Command.DeleteLoanRequestDocument;
using Applications.Features.LoanRequests.Command.DeleteLoanRequestGuarantor;
using Applications.Features.LoanRequests.Command.SaveEditLoanRequest;
using Applications.Features.LoanRequests.Dtos;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestDetailById;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestsDocuments;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Dtos.Requests;
using Applications.Features.Notifications.Command.CreateNotification;
using Applications.Features.Notifications.Dto;
using Applications.Responses.Loans.Disbursements;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Loans.LoanRequests;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Enumerations;
using Domains.Models.Files;
using Infrastructures.Authorizations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TrapchumApp.Components.Messageboxes;
using TrapchumApp.Pages.Lists.Collaterals;
using TrapchumApp.Pages.Lists.OtherNames.Guarantors;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.LoanRequests;

public partial class LoanRequestEdit(ILogger<LoanRequestEdit> logger)
{
    [Parameter] public int? BranchId { get; set; } = 0;
    [Parameter] public string? CustomerId { get; set; }
    [Parameter] public string? LoanRequestId { get; set; }
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [CascadingParameter] CascadingAppState bState { get; set; } = default!;
    private IEnumerable<LoanScheduleDtoCreate> previewSchedule = [];
    private IEnumerable<VCustomerList> _customers = [];
    private IEnumerable<VCustomerDetails>? CustomersList = [];
    private List<CoPawnCustomerDto> CoCustomers = [];
    private List<VLoanRequestGuarantor> Guarantors = [];
    private List<VLoanRequestCollateralLines> CollateralLines = [];
    private List<VLoanRequestsDocuments> Documents = [];
    private List<VLoanRequestDescription> _loanDescriptions = [];
    private IEnumerable<LoanRequestDocumentsDtoCreate> _documents = [];
    private List<DateTime> Holidays = [];
    private LoanRequestDtoCreate model = new();
    private DateTime? RegisterDate = DateTime.Now;
    private EditForm editForm = default!;
    private VCustomerDetails? SelectedCustomer = new();
    private LoanRequestDocument DocumentRef = default!;
    private LoanRequestGaurantor GuarantorRef = default!;
    private CoRequestCustomerBox CoCustomerRef = default!;
    private LoanRequestCollaterals CollateralRef = default!;
    private List<VCollateralLines> _collateralLines = [];
    private VCustomerDetails? selectedCustomer = new();
    private DateTime firstPayment;
    private LoanCategoryDto? SelectedLoanCategory;
    private VUser _user = new();
    private long? size = 0;
    private int counts = 0;
    private bool isVible = false;
    private bool isCustomerSelected = false;
    private bool isLoading = false;
    private bool isProgressing = false;
    private bool isAllow = false;
    private int SelectedCollectionFee = 1;
    private decimal? TotalAdminFee = 0;
    private decimal? LoanRequestAmount = 0;
    private int SelectedPeriodName = 1;
    private int installmentType = 1;
    private int selectedTabIndex = 0;
    private string? filterText = "";
    private string? _userId = string.Empty;
    private IEnumerable<FileModel>? files = [];
    private IEnumerable<System.IO.FileInfo> UploadFiles = [];
    private void SelectTabIndexChanged(int tabIndex)
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

        IEnumerable<DocumentDto>? uploadedFiles = await DialogService.OpenAsync<FileUploadDialogue>("Upload", options: options);


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
                    NewFileName = f.NewFileName,
                    FileUrl = f.FileUrl,
                    FileSize = f.FileSize,
                    AltText = f.OriginalName,
                    Description = f.OriginalName,
                    DocumentId = f.Id
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

        await GetLoanRequestInfo(user);

        await base.OnInitializedAsync();
    }

    async Task GetLoanRequestInfo(ClaimsPrincipal user)
    {
        try
        {
            isLoading = true;

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

            CoCustomers = data.CoPawnRequests.ToList();

            model = data!.ToLoanRequestDtoCreate();



            isLoading = false;

        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
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
        logger.LogInformation("get start loan requesting...");

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

            NotificationService.Notify(NotificationSeverity.Success, resp.Value.Message!);

            NavigationManager.NavigateTo($"/App/Loans/Requests/{BranchId}/Branches");

        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);

            logger.LogError(ex, ex.Message);
        }
    }



    private async Task OnSelectedCoCustomers(IEnumerable<VCustomerList> args)
    {
        if (!args.Any()) return;

        if (args.Count() > 3)
        {
            _toastService.ShowWarning("You can only select up to 3 co-customers.");

            return;
        }

        var newCustomers = args.Where(c => CoCustomers.All(existing => existing.CustomerId != c.CustomerId));


        var coPawnCustomers = newCustomers.Select(x => new CoPawnRequestsDtoCreate
        {
            CustomerId = x.CustomerId,
            BranchId = BranchId,
            LoanRequestId = LoanRequestId

        }).ToList();


        var command = new AddCoPawnCustomersCommand(BranchId, LoanRequestId, coPawnCustomers);

        var rsp = await mediator.Send(command);

        if (rsp.IsSuccess())
        {
            CoCustomers.AddRange(rsp.Value);

            await CoCustomerRef.CustomerReload();
        }
        else
        {
            _toastService.ShowError(rsp.GetMessage());
        }

    }


    async Task OnRemoveCoPawnCustomer(CoPawnCustomerDto? arg)
    {
        if (arg is null) return;

        var dia = await MsgBox.ShowAsync("Do you want to remove this co-customer?", "Remove Co-Customer", MsgBoxButtons.YesNo, MsgBoxBoxIcon.Question);

        if (dia == MsgBoxResult.No) return;


        var command = new DeleteCoPawnCustomerCommand(arg);

        var rsp = await mediator.Send(command);

        if (rsp.IsSuccess())
        {

            CoCustomers.Remove(arg);

            await CoCustomerRef.CustomerReload();

            _toastService.ShowSuccess("Co-customer was removed successfully.");
        }
        else
        {
            _toastService.ShowError(rsp.GetMessage());
        }
    }

    private async Task OnSelectedCoCustomer(VCustomerList args)
    {
        if (CoCustomers.Count >= 3)
        {
            _toastService.ShowWarning("You can only select up to 3 co-customers.");
            return;
        }

        if (CoCustomers.Any(c => c.CustomerId == args.CustomerId))
        {
            _toastService.ShowWarning("This co-customer is already selected.");
            return;
        }

        var command = new AddCoPawnCustomerCommand(new CoPawnRequestsDtoCreate
        {
            BranchId = BranchId,
            CustomerId = args.CustomerId,
            LoanRequestId = LoanRequestId,
        });

        var rsp = await mediator.Send(command);

        if (rsp.IsSuccess())
        {

            CoCustomers.Add(rsp.Value);

            await CoCustomerRef.CustomerReload();
        }
        else
        {
            _toastService.ShowError(rsp.GetMessage());
        }
    }
}
