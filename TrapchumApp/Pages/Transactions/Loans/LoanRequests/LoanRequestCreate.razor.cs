using Applications.Enumerations;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Documents.Dto;
using Applications.Features.Guarantor.Dtos;
using Applications.Features.Identity.Queries.GetUserDetailById;
using Applications.Features.LoanRequests.Command.CreateLoanRequest;
using Applications.Features.LoanRequests.Requests;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Enumerations;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Pages.Lists.Collaterals;
using TrapchumApp.Pages.Lists.OtherNames.Guarantors;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestCreate(ILogger<LoanRequestCreate> logger)
    {
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public string? CustomerId { get; set; } = string.Empty;
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [CascadingParameter] CascadingAppState BState { get; set; } = default!;

        LoanRequestDtoCreate model = new();

        EditForm editForm = default!;

        LoanRequestDocument DocumentRef = default!;

        LoanRequestGaurantor GuarantorRef = default!;

        LoanRequestCollaterals CollateralRef = default!;

        List<VLoanRequestGuarantor> Guarantors = [];

        List<VLoanRequestCollateralLines> requestCollateralLines = [];

        List<VLoanRequestsDocuments> Documents = [];

        Applications.Responses.ViewModels.Auths.VUser _user = new();

        int selectedTabIndex = 0;

        private string? _userId = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            _userId = (await AuthenticationState).GetUserId();

            await BState.GetEmployeeId();

            var rsp = await mediator.Send(new GetUserDetailsByIdQuery(_userId));

            if (rsp.IsSuccess())
            {
                model.CreditAgentRefId = _user.EmployeeRefId;
            }

            SetDefault();

            await base.OnInitializedAsync();
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
            IEnumerable<DocumentDto> uploadedFiles = await DialogService.OpenAsync<FileUploadDialogue>("Upload");

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
                Quantity = c.Quantity,
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
                    TypeName = g.TypeName,
                    Quantity = g.Quantity,
                });
            }

            await CollateralRef.CollateralReload();
        }

        async Task AddCollaterals()
        {
            var param = new Dictionary<string, object?>
            {
                { "CollateralId", model.CustomerRefId }
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
            if (!await ConfirmActionAsync("Do you want to remove this collateral?", "Remove Collateral"))
                return;

            if (!requestCollateralLines.Any()) return;

            var lastItem = requestCollateralLines.Last();

            if (lastItem != c)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Please select the last item to remove.");
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

            if (!await ConfirmActionAsync("Do you want to save this loan request?"))
                return;

            await HandleLoanRequest(RequestStatus.New);
        }

        protected async Task HandleCreateRequest()
        {
            if (editForm.EditContext?.Validate() == false)
            {
                return;
            }

            if (!await ConfirmActionAsync("Do you want to save this loan request?"))
                return;

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
                NotificationService.Notify(NotificationSeverity.Success, resp.Value.Message!);

                NavigationManager.NavigateTo($"/App/Loans/Requests/{BranchId}/Branches");
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, resp.GetMessage());
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
}

