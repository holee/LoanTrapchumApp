using Applications.Enumerations;
using Applications.Extensions;
using Applications.Extensions.LoanCalculators;
using Applications.Requests.Lists.Collaterals;
using Applications.Requests.Lists.Entities.Guarantors;
using Applications.Requests.Loans.CreditAgents;
using Applications.Requests.Loans.Disbursements;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Loans;
using Domains.Models.Files;
using Domains.Models.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;
using TrapchumApp.Components.Pages.Lists.Collaterals;
using TrapchumApp.Components.Pages.Lists.OtherNames;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestEdit
    {
        [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
        [Parameter, SupplyParameterFromQuery] public Guid? UserId { get; set; }
        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter] public Guid? LoanRequestId { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        LoanRequestDocument DocumentRef = default!;
        LoanRequestCollaterals CollateralRef = default!;
        LoanRequestGaurantor GuarantorRef = default!;
        IEnumerable<LoanScheduleDtoCreate> previewSchedule = [];
        IEnumerable<VCustomerList> Customers = [];
        IEnumerable<VCustomerList>? CustomersList = [];
        IEnumerable<SelectedCreditAgent>? CreditAgents = [];
        IEnumerable<CollateralLineDtoCreate>? CollateralLines = [];
        IEnumerable<CollateralLineDtoCreate>? SelectedCollateralLines = [];
        IEnumerable<VBranchResponse> Branches = Enumerable.Empty<VBranchResponse>();
        List<VLoanRequestGuarantors> Guarantors = [];
        List<VLoanRequestCollateralLines> Collaterals = [];
        List<VLoanRequestsDocuments> Documents = [];
        RadzenDataGrid<VLoanRequestCollateralLines> gridColaterals = default!;
        RadzenDataGrid<VLoanRequestGuarantors> gridGuarantors = default!;
        RadzenDataGrid<VLoanRequestsDocuments> gridDocuments = default!;
        string branchName = "";
        string customerName = "";
        bool isProgressing = false;
        VLoanRequests data = new();
        LoanRequestDtoCreate model = new();
        bool IsAccepted = false;
        bool isLoading = false;
        // const long maxFileSize = 30_000_000;

        string editUrl = "";
        string backUrl = "";

        void ShowLoanPreviews()
        {
            if (model.RequestAmount == 0) return;
            var calculator = SelectLoanType(3);
            calculator.PayEvery = 1;
            calculator.EffectiveRate = model.CollectionFeeRate!.Value;
            calculator.StartDate = DateTime.Now;
            calculator.FirstPayment = DateTime.Now.AddMonths(1);
            calculator.CollectionFeeMode = 2;
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
                AgentRefId = Guid.NewGuid(),
                ScheduleStatusRefId = 2
            }).ToList();
        }
        async Task CreateLoanRequest(VLoanRequests req)
        {
            var confirm = await DialogService.Confirm("Do you want to request this loan?.", "Loan Requesr");

            if (confirm == false)
            {

                NotificationService.Notify(NotificationSeverity.Info, "Loan request cancel.");
                return;
            }

            var request = new RequestDtoBaseCreate(BranchId, CustomerId, LoanRequestId, string.Empty);

            var resp = await _loanRequestService.AppliedLoanRequestAsync(request);

            if (resp.IsSuccess)
            {
                NavigationManager.NavigateTo($"/Loan/Request?BranchId={BranchId}&UserId={UserId}");
            }

        }
        async void Remove(VLoanRequestsDocuments d)
        {
            var confirm = await DialogService.Confirm("Do you want to remove this document?", "Remove Document");

            if (confirm == false) return;


            var resp = await _loanRequestService.DeleteLoanRequestDocumentAsync(LoanRequestId, BranchId, d.Id);

            if (resp.IsSuccess)
            {
                Documents.Remove(d);
                DocumentRef.ReloadDocument();
                NotificationService.Notify(NotificationSeverity.Warning, resp.Data);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Warning, resp.ErrMessage);
            }


        }

        async Task HandleFileDialogue()
        {

            var options = new DialogOptions
            {

            };

            IEnumerable<FileModel> uploadedFiles = await DialogService.OpenAsync<FileUploadDialogue>("Upload", options: options);

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
            }
            if (Documents.Any())
            {
                await HandleFileSave(Documents);
            }
        }

        async void AddGuarantors()
        {
            var options = new DialogOptions
            {
                Width = "60vw"
            };

            IList<VGuarantor> results = await DialogService.OpenAsync<GuarantorsList>("Select Guarantor", options: options);

            if (!results.Any()) return;

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
                if (Guarantors.Any(x => x.EntId == item.EntId)) continue;
                Guarantors.Add(item);
            }
            var gaurantors = new LoanRequestGuarantors
            {
                CustomerRefId = CustomerId,
                BranchRefId = BranchId,
                LoanRequestId = LoanRequestId,
                //GuarantorsLink = Guarantors.Select(g => new LoanGuarantorDtoCreate
                //{
                //    Id = g.Id,
                //    BranchId = BranchId,
                //    EntityNameRefId = g.EntId,
                //    LoanRequestRefId = LoanRequestId,
                //    IsActive = true
                //}).ToList()
            };

            //var resp = await _loanRequestService.AddLoanRequestGuarantorAsync(gaurantors);

            //if (resp.IsSuccess)
            //{
            //    // await gridGuarantors.Reload();
            //    await GuarantorRef.GuarantorReload();
            //}


        }

        async void RemoveGuarantor(VLoanRequestGuarantors g)
        {

            var confirm = await DialogService.Confirm("Do you want to remove guarantor from link?", "Delete");

            if (confirm == false) return;

            if (g is not null)
            {
                var resp = await _loanRequestService.DeleteLoanRequestGuarantorAsync(LoanRequestId, BranchId, g.Id);

                if (resp.IsSuccess)
                {
                    Guarantors.Remove(g);
                    await GuarantorRef.GuarantorReload();
                    NotificationService.Notify(NotificationSeverity.Success, resp.Data);
                }

            }

            return;
        }

        async void AddCollaterals()
        {
            var param = new Dictionary<string, object?>
        {
            {"CustomerId",CustomerId}
        };
            var options = new DialogOptions
            {
                Width = "60vw"
            };

            IList<VCollateralLines> results = await DialogService.OpenAsync<CollateralLoanRequest>("Collateral", param, options);

            if (!results.Any()) return;

            var collateralLines = results.Select(c => new VLoanRequestCollateralLines
            {
                CollateralLineId = c.Id,
                CollateralId = c.CollateralId!.ToGuid(),
                TypeName = c.TypeName,
                OwnerName = c.OwnerName,
                ReferenceNo = c.ReferenceNo,
                Quantity = c.Quantity
            }).ToList();

            foreach (var item in collateralLines)
            {
                if (Collaterals.Any(x => x.CollateralLineId == item.CollateralLineId && x.CollateralId == item.CollateralId))
                {
                    continue;
                }
                Collaterals.Add(item);
            }

            if (!Collaterals.Any()) return;

            var collaterals = new LoanCollateralRequest
            {
                CustomerId = CustomerId,
                LoanRequestId = LoanRequestId,
                BranchId = BranchId,
                //CollateralLinesLink = Collaterals.Select(c => new CollateralLinkDtoCreate
                //{
                //    Id = c.Id,
                //    BranchId = BranchId,
                //    LoanRequestRefId = c.LoanRequestId,
                //    CollateralRefId = c.CollateralId,
                //    CollateralLineRefId = c.CollateralLineId,
                //    IsActive = true
                //}).ToList()
            };

            //var resp = await _loanRequestService.AddCollateralsLoanRequestAsync(collaterals);

            //if (resp.IsSuccess)
            //{
            //    //await gridColaterals.Reload();

            //    CollateralRef.CollateralReload();

            //}
            //else
            //{
            //    var message = resp.Errors!.First();
            //    NotificationService.Notify(NotificationSeverity.Error, message);
            //}
        }

        async void RemoveCollateral(VLoanRequestCollateralLines c)
        {
            if (c is null) return;

            bool? confirm = await DialogService.Confirm("Do you want to remove collaterals from Link?", "Confirm");

            if (confirm == false) return;

            var resp = await _loanRequestService.DeleteLoanRequestCollateralAsync(BranchId, LoanRequestId, c.Id);

            if (resp.IsSuccess)
            {
                Collaterals.Remove(c);
                CollateralRef.CollateralReload();
                NotificationService.Notify(NotificationSeverity.Success, resp.Data);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Success, resp.ErrMessage);
            }
        }

        protected async Task HandleCreateRequest()
        {
            await HandleLoanRequest();
        }

        protected async Task HandleLoanRequest()
        {
            try
            {
                var req = new LoanChangeStatusDtoCreate
                {
                    LoanRequestId = LoanRequestId,
                    StatusId = 2,
                    BranchId = BranchId,
                    AprovedUserId = UserId
                };
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

        async Task HandleFileSave(IEnumerable<VLoanRequestsDocuments> doc)
        {

            if (!doc.Any())
            {
                NotificationService.Notify(NotificationSeverity.Error, "There is any file to upload");
                return;
            }
            var document = LoanDocumentsExtensions.ToLoanRequestDocuments(CustomerId, LoanRequestId, BranchId, Documents);
            var res = await _loanRequestService.AddLoanRequestDocumentAsync(document);
            if (!res.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Error, res.Data);
                return;
            }
            await GetLoanRequestDocuments();
            NotificationService.Notify(NotificationSeverity.Error, res.ErrMessage);
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
        protected override async Task OnParametersSetAsync()
        {
            backUrl = $"/Loan/Request?BranchId={BranchId}&UserId={UserId}";
            editUrl = $"/Loan/{CustomerId}/Customer/{LoanRequestId}/Request?BranchId={BranchId}&UserId={UserId}&ActionMode={ActionMode.Edit}";
            isLoading = true;
            if (CustomerId.NotEmpty() && LoanRequestId.NotEmpty())
            {
                //var reqResp = await _loanRequestService.GetLoanRequestsAsync(BranchId, CustomerId, LoanRequestId, true);

                //if (reqResp.IsSuccess)
                //{
                //    data = reqResp.Data!;
                //    Collaterals = reqResp.Data!.Collaterals.ToList();
                //    Guarantors = reqResp.Data!.Guarantors.ToList();
                //    Documents = reqResp.Data!.Documents.ToList();
                //}
                IsAccepted = data.StatusId == 2 ? true : false;
            }

            isLoading = false;

            await base.OnParametersSetAsync();

        }


    }
}