using Applications.Commons.Mappers;
using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.LoanRequests.Command.CreateLoanRequest;
using Applications.Features.LoanRequests.Command.DenyLoanRequest;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestDetailById;
using Applications.Features.LoanRequests.Requests;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Identity;
using Infrastructures.Authorizations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestDetail
    {
        [CascadingParameter] Task<AuthenticationState> State { get; set; } = default!;
        [CascadingParameter] public CascadingAppState AppState { get; set; } = default!;
        [Parameter] public string? LoanRequestId { get; set; }
        [Parameter] public int? BranchId { get; set; }
        [Parameter] public string? CustomerId { get; set; }
        [Parameter, SupplyParameterFromQuery] public string? CreditAgentId { get; set; } = string.Empty;

        VLoanRequests? data = new();

        string memo = "";

        string? aprovedUserId = string.Empty;

        bool success = false;

        bool canApprove = false;

        bool canAllow = false;

        ClaimsPrincipal? user;

        protected override async Task OnInitializedAsync()
        {
            user = (await State).User;

            aprovedUserId = user.GetId();

            await AppState.CheckedCanApprved(user);

            await AppState.CheckedCanDisbursemnt(user);

            await AppState.CheckedCanRequest(user);

            await GetLoanRequestDetails(user);

            await base.OnInitializedAsync();
        }

        async Task CreateLoanRequest()
        {
            var confirm = await DialogService.Confirm("Do you want to aprove this request?", "Loan Request");

            if (confirm == false)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Reuqest wa cancel.");

                return;
            }

            var model = data?.MapToLoanRequestCreate();

            if (memo.IsNotNullOrEmpty() && model is not null)
            {
                var description = new LoanDescriptionsDtoCreate(memo, aprovedUserId, LoanRequestId);

                model.LoanDescriptions.Add(description);
            }

            if (model is null)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Loan request data is null.");

                return;
            }

            var command = new CreateLoanRequestCommand(model);


            var rsp = await mediator.Send(command);

            if (!rsp.IsError)
            {

                NotificationService.Notify(NotificationSeverity.Success, "Success");

                StateHasChanged();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
            }
        }


        async Task GetLoanRequestDetails(ClaimsPrincipal user, bool include = true)
        {

            var query = new GetLoanRequestDetailByIdQuery(BranchId, CustomerId, LoanRequestId, include);

            var resp = await mediator.Send(query);

            if (!resp.IsError)
            {
                data = resp.Value;

                canAllow = (await _authorizeService.AuthorizeAsync(user, data, new SameUserRequestRequirement()))
                    .Succeeded;

                canApprove = (await _authorizeService.AuthorizeAsync(user, data, new SameUserApprovedRequirement()))
                    .Succeeded;

                success = data?.StatusId == (int)RequestStatus.Requested ? false : true;

                disableTextbox = data?.StatusId == (int)RequestStatus.Approved ? true : false;
            }
        }

        private bool disableTextbox = false;

        async Task AprovedLoanRequest()
        {
            var options = new DialogOptions
            {
            };
            var param = new Dictionary<string, object?>
            {
                { "LoanRequestId", LoanRequestId },
                { "BranchId", BranchId },
                { "CustomerId", CustomerId }
            };

            bool dialogueResult = await DialogService.OpenAsync<ConfirmModal>("Confirm", param, options);

            if (dialogueResult && user is not null)
            {
                await GetLoanRequestDetails(user, true);

                disableTextbox = true;

                StateHasChanged();
            }
        }

        async Task DeniedLoanRequest()
        {
            var confirm = await DialogService.Confirm("Do you want to reject this request?", "Loan Request");

            if (confirm == false)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Reuqest wa cancel.");
                return;
            }

            var req = new DeniedRequestDtoCreate(BranchId, CustomerId, LoanRequestId, DateTime.UtcNow, memo);

            var command = new DeniedLoanRequestCommand(req);

            var rsp = await mediator.Send(command);

            if (!rsp.IsError)
            {
                var description = new LoanDescriptionsDtoCreate(memo, aprovedUserId, LoanRequestId);

                await _loanRequestService.AddDescriptionAsync(description);

                NotificationService.Notify(NotificationSeverity.Success, rsp.Value.Message);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
            }
        }

        async Task RecheckedLoanRequest()
        {
            var confirm = await DialogService.Confirm("Do you want to return back this request?", "Loan Request");

            if (confirm == false)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Reuqest wa cancel.");

                return;
            }

            var req = new RecheckedRequestDtoCreate(BranchId, CustomerId, LoanRequestId, DateTime.UtcNow, RequestStatus.Rechecked, memo);

            var rsp = await _loanRequestService.RecheckedLoanRequestAsync(req);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "");
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }

        async Task SaveLoanRequest()
        {
            var confirm = await DialogService.Confirm("Do you want to save this request?", "Loan Request");

            if (confirm == false)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Reuqest wa cancel.");

                return;
            }

            var req = new RecheckedRequestDtoCreate(BranchId, CustomerId, LoanRequestId, DateTime.UtcNow, RequestStatus.Rechecked, memo);

            var rsp = await _loanRequestService.VerifiedLoanRequestAsync(req);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }
    }
}