using Applications.Commons.Mappers.LoanRequests;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Queries.GetCustomerAsPaging;
using Applications.Features.Customers.Response;
using Applications.Features.Documents.Dto;
using Applications.Features.Guarantor.Dtos;
using Applications.Features.Identity.Queries.GetUserDetailById;
using Applications.Features.LoanRequests.Command.CreateLoanRequest;
using Applications.Features.LoanRequests.Dtos;
using Applications.Features.LoanRequests.Queryies.IsCoPawnExist;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Settings.Queries.GetApprovedUsers;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Enumerations;
using Microsoft.AspNetCore.Components.Routing;
using TrapchumApp.Pages.Lists;
using TrapchumApp.Pages.Lists.Collaterals;
using TrapchumApp.Pages.Lists.Customers;
using TrapchumApp.Pages.Lists.OtherNames.Guarantors;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.LoanRequests;

public partial class LoanRequestCreateNoCustomer(ILogger<LoanRequestCreateNoCustomer> logger) : IDisposable
{
    [Parameter] public int? BranchId { get; set; } = 0;
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [CascadingParameter] CascadingAppState BState { get; set; } = default!;

    private LoanRequestDtoCreate model = new();

    private EditForm editForm = default!;

    private LoanRequestDocument DocumentRef = default!;

    private LoanRequestGaurantor GuarantorRef = default!;

    private LoanRequestCollaterals CollateralRef = default!;

    private List<VLoanRequestGuarantor> Guarantors = [];

    private List<VLoanRequestCollateralLines> requestCollateralLines = [];

    private List<VLoanRequestsDocuments> Documents = [];

    private VUser _user = new();

    private int selectedTabIndex = 0;

    private string? _userId = string.Empty;

    private List<CoPawnCustomerDto> CoCustomers = [];

    private CoRequestCustomerBox CoCustomerRef = default!;

    private string? _customerId = string.Empty;

    private IEnumerable<VCustomerList> _customers = [];

    private bool _isDirty = false;

    private IDisposable? _locationChangingRegistration;

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            editForm.EditContext!.OnFieldChanged += (_, _) => _isDirty = true;

            _locationChangingRegistration = NavigationManager.RegisterLocationChangingHandler(OnLocationChanging);
        }
        return base.OnAfterRenderAsync(firstRender);
    }

    private async ValueTask OnLocationChanging(LocationChangingContext context)
    {
        if (!_isDirty) return;

        var confirmed = await Swal.ConfirmAsync("លោកអ្នកមិនបានរក្សាទុកការផ្លាស់ប្តូរ។ តើអ្នកប្រាកដថាចង់ចាកចេញ?", "Unsaved Changes");

        if (confirmed != true)
        {
            context.PreventNavigation();
        }
    }

    public void Dispose()
    {
        _locationChangingRegistration?.Dispose();
    }

    protected override async Task OnInitializedAsync()
    {
        _userId = BState.UserId;

        model.CreditAgentRefId = await BState.GetEmployeeId();

        await GetCustomers();

        await GetLoanPurposes();

        await GetLoanCategories();

        await GetPeriods();

        SetDefault();

        await base.OnInitializedAsync();
    }

    private async Task GetApprovedUser()
    {
        var rsp = await mediator.Send(new GetUserDetailsByIdQuery(_userId));

        if (rsp.IsSuccess())
        {
            model.CreditAgentRefId = _user.EmployeeRefId;
        }
    }

    private async Task GetApprovedUser(int? branchId, int? currencyId, decimal? amount)
    {

        var command = new GetApprovedUserQuery(branchId, currencyId, amount, null);

        var rsp = await mediator.Send(command);

        AprovedUsers = rsp.Value ?? [];

        defaultUserId = AprovedUsers
                            .Where(x => x.IsDefault == true)
                            .Select(x => x.UserRefId)
                            .FirstOrDefault();

    }

    private async Task GetCustomers(string? filterText = null)
    {
        var command = new GetCustomerAsPagingQuery(BranchId, null, null, new QueryParam(0, 15, null, false, false, filterText));

        var rsp = await mediator.Send(command);

        _customers = rsp.Items;

        StateHasChanged();
    }


    async Task RemoveCoCustomer(CoPawnCustomerDto? c)
    {
        if (c is null) return;

        var confirm = await MsgBox.ConfirmAsync("Do you want to remove this co-customer?", "Remove Co-Customer");

        if (confirm == MsgBoxResult.No) return;

        if (CoCustomers.Remove(c))
        {
            await CoCustomerRef.CustomerReload();
        }
    }


    void AddCoCustomer()
    {
        var options = new DialogOptions
        {
            Width = "100vw"
        };

        var param = new Dictionary<string, object?>
        {
            { "BranchId", BranchId },

            { "PageType", PageType.Dialog }
        };

        DialogService.OpenAsync<CustomerCreatePage>("Add Customer", param, options: options);
    }





    private async Task OnSelectedCoCustomers(IEnumerable<VCustomerList> args)
    {
        var candidates = args as ICollection<VCustomerList> ?? args.ToList();

        if (candidates.Count == 0) return;

        if (candidates.Count > 3)
        {
            _toastService.ShowWarning("You can only select up to 3 co-customers.");
            return;
        }

        int slots = 3 - CoCustomers.Count;

        if (slots <= 0 || candidates.Count > slots)
        {
            _toastService.ShowWarning($"You can only select {slots} more co-customer(s).");
            return;
        }

        var newCustomers = candidates
            .Where(c => CoCustomers.All(existing => existing.CustomerId != c.CustomerId))
            .ToList();

        if (newCustomers.Count == 0) return;

        // Check all candidates in parallel
        var checks = await Task.WhenAll(
            newCustomers.Select(c => mediator
                .Send(new IsCoPawnExistQuery(BranchId, c.CustomerId))
                .ContinueWith(t => (Customer: c, Result: t.Result), TaskScheduler.Default)));

        var newCoPawnCustomers = new List<CoPawnCustomerDto>(newCustomers.Count);

        foreach (var (customer, result) in checks)
        {
            if (result.IsError)
            {
                _toastService.ShowError($"Could not verify co-customer {customer.AltName}.");
                continue;
            }

            if (result.Value)
            {
                _toastService.ShowWarning($"{customer.AltName} is already a co-customer in an active loan request or loan disbursement.");
                continue;
            }

            newCoPawnCustomers.Add(new CoPawnCustomerDto
            {
                CustomerId = customer.CustomerId,
                CustomerPhone = customer.Phone,
                CustomerAddress = customer.ToString(),
                Gender = customer.Salutation,
                CustomerName = customer.AltName,
                BranchId = BranchId ?? 0
            });
        }

        if (newCoPawnCustomers.Count == 0) return;

        CoCustomers.AddRange(newCoPawnCustomers);

        await CoCustomerRef.CustomerReload();
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

        var newCoPawnCustomer = new CoPawnCustomerDto
        {
            CustomerId = args.CustomerId,
            CustomerPhone = args.Phone,
            CustomerName = args.AltName,
            CustomerAddress = args.ToString(),
            Gender = args.Salutation,
            BranchId = BranchId ?? 0
        };

        if (newCoPawnCustomer is null) return;

        var coPawnExist = new IsCoPawnExistQuery(BranchId, newCoPawnCustomer.CustomerId);

        var rsp = await mediator.Send(coPawnExist);

        if (rsp.IsSuccess())
        {
            _toastService.ShowWarning("This co-customer is already selected.");

            return;
        }

        CoCustomers.Add(newCoPawnCustomer);

        await CoCustomerRef.CustomerReload();

    }



    void SelectTabIndexChanged(int tabIndex)
    {
        selectedTabIndex = tabIndex;
    }

    private async Task<bool> ConfirmActionAsync(string message, string title = "Loan Request")
    {
        var confirm = await DialogService.Confirm(message, title);

        if (confirm == false)
        {
            NotificationService.Notify(NotificationSeverity.Info, "Request was cancelled.");
            return false;
        }

        return true;
    }

    async Task HandleFileDialogue()
    {
        IEnumerable<DocumentDto>? uploadedFiles = await DialogService.OpenAsync<FileUploadDialogue>("Upload");

        if (uploadedFiles is null || !uploadedFiles.Any())
        {
            return;
        }

        foreach (var f in uploadedFiles)
        {
            Documents.Add(new VLoanRequestsDocuments
            {
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

    async Task RemoveFile(VLoanRequestsDocuments doc)
    {
        if (!await ConfirmActionAsync("Do you want to remove this document?", "Remove Document"))
            return;

        if (doc is not null && doc.Id == 0)
        {
            Documents.Remove(doc);

            DocumentRef.ReloadDocument();

            NotificationService.Notify(NotificationSeverity.Success, "Document was removed.");
        }
    }

    async Task SelectMany(IEnumerable<VGuarantor> gs)
    {
        if (!gs.Any()) return;

        foreach (var g in gs)
        {
            if (Guarantors.Any(x => x.GuarantorId == g.Id)) continue;

            Guarantors.Add(new VLoanRequestGuarantor
            {
                GuarantorId = g.Id,
                FullName = g.FullName,
                GuarantorName = g.GuarantorName,
                Phone = g.Phone,
                Address = g.ToString(),
            });
        }

        await GuarantorRef.GuarantorReload();
    }

    async Task SelectedGuarantors(VGuarantor? g)
    {
        if (g is null || string.IsNullOrEmpty(g.Id)) return;

        if (Guarantors.Any(x => x.GuarantorId == g.Id))
        {
            NotificationService.Notify(NotificationSeverity.Info, "Guarantor already exists.");
            return;
        }

        Guarantors.Add(new VLoanRequestGuarantor
        {
            GuarantorId = g.Id,
            FullName = g.FullName,
            GuarantorName = g.GuarantorName,
            Phone = g.Phone,
            Address = g.ToString(),
        });

        await GuarantorRef.GuarantorReload();
    }

    async Task AddGuarantors()
    {
        var options = new DialogOptions { Width = "60vw" };

        await DialogService.OpenAsync<GuarantorAddModal>("Add New Guarantor", options: options);
    }

    async Task RemoveGuarantor(VLoanRequestGuarantor g)
    {
        if (!await ConfirmActionAsync("Do you want to remove this guarantor?", "Remove Guarantor"))
            return;

        if (!Guarantors.Any()) return;

        var lastItem = Guarantors.Last();

        if (lastItem != g)
        {
            NotificationService.Notify(NotificationSeverity.Info, "Please select the last item to remove.");
            return;
        }

        if (Guarantors.Remove(g))
        {
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
            SubTypeName = c.SubTypeName,
            Quantity = c.Quantity,
            PawnPrice = c.PawnPrice,
            MarketPrice = c.MarketPrice,
            HotPrice = c.HotPrice
        };

        if (requestCollateralLines.Any(x => x.Id == item.Id))
        {
            NotificationService.Notify(NotificationSeverity.Info, "Collateral already exists.");
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

            requestCollateralLines.Add(new VLoanRequestCollateralLines
            {
                CollateralLineId = g.Id,
                CollateralId = g.CollateralId,
                OwnerName = g.OwnerName,
                ReferenceNo = g.ReferenceNo,
                SubTypeName = g.SubTypeName,
                TypeName = g.TypeName,
                Quantity = g.Quantity,
                PawnPrice = g.PawnPrice,
                MarketPrice = g.MarketPrice,
                HotPrice = g.HotPrice
            });
        }

        await CollateralRef.CollateralReload();
    }

    async Task AddCollaterals()
    {
        var param = new Dictionary<string, object?>
        {
            { "CollateralId", model.CustomerRefId },

            {"PageType", PageType.Dialog }
        };

        var options = new DialogOptions
        {
            Width = "100vw",

            Height = "100vh",

            AutoFocusFirstElement = false
        };

        await DialogService.OpenAsync<CustomerCollateralLineCreate>("Collateral", param, options);
    }

    async Task RemovedCollateral(VLoanRequestCollateralLines c)
    {

        if (await MsgBox.ConfirmAsync("Do you want to remove this collateral?", "Remove Collateral") == MsgBoxResult.No) return;

        if (!requestCollateralLines.Any()) return;

        var lastItem = requestCollateralLines.Last();

        if (lastItem != c)
        {
            _toastService.ShowInfo("Please select the last item to remove.");

            return;
        }

        if (requestCollateralLines.Remove(c))
        {
            await CollateralRef.CollateralReload();
        }
    }

    protected async Task HandleCreateNew()
    {
        if (editForm.EditContext?.Validate() == false) return;

        if (await MsgBox.ConfirmAsync("Do you want to save this loan request?") == MsgBoxResult.No) return;

        await HandleLoanRequest(RequestStatus.New);
    }

    decimal? _customerProfit = 0;
    void OnGetIncomeOrExpense(decimal? profit)
    {
        _customerProfit = profit;
    }

    protected async Task HandleCreateRequest()
    {
        if (editForm.EditContext?.Validate() == false)
        {
            return;
        }

        if (requestCollateralLines.Count() == 0)
        {
            _toastService.ShowWarning("Please add at least one collateral to proceed.");

            return;
        }

        if (Guarantors.Count() == 0)
        {
            _toastService.ShowWarning("Please add at least one guarantor to proceed.");

            return;
        }

        if (_customerProfit <= 0)
        {
            _toastService.ShowError("Customer's profits must be greater than zero to proceed with the loan request.");

            return;
        }


        if (await MsgBox.ConfirmAsync("Do you want to save this loan request?") == MsgBoxResult.No) return;

        await HandleLoanRequest(RequestStatus.Requested);
    }

    private async Task HandleLoanRequest(RequestStatus state)
    {
        logger.LogInformation("Starting loan request submission...");

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

        var coPawnCustomers = CoCustomers.Maps();

        model.CoPawnRequests.AddRange(coPawnCustomers);


        if (editForm.EditContext!.Validate() == false)
        {
            return;
        }

        model.BankRefId = 11;
        model.LoanSubTypeRefId = (int)LoanSubTypeOption.Standard;
        model.LoanProductRefId = (int)ProductTypeOption.Mortgage;
        model.BranchRefId = BranchId;
        model.RequestUserRefId = _userId;
        model.CreditAgentRefId = _user.EmployeeRefId;
        model.RequestDate = DateTime.UtcNow;
        model.LoanRequestStatusRefId = (int)state;
        model.AprovedAmount = model.RequestAmount;

        var commandCreate = new CreateLoanRequestCommand(model);

        var resp = await mediator.Send(commandCreate);

        if (!resp.IsError)
        {

            _toastService.ShowSuccess(resp.Value.Message);

            _isDirty = false;

            NavigationManager.NavigateTo($"/App/Loans/Requests/{BranchId}/Branches");
        }
        else
        {
            _toastService.ShowError(resp.GetMessage());
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
        model.FirstPaymentDate = DateTime.UtcNow.AddMonths(1);
    }
}

