using Applications.Commons.Mappers;
using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.LoanRequests.Command.AddDescription;
using Applications.Features.LoanRequests.Command.DenyLoanRequest;
using Applications.Features.LoanRequests.Command.RecheckedLoanRequest;
using Applications.Features.LoanRequests.Command.SaveEditLoanRequest;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestDetailById;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Command.SaveLoanDisbursemenent;
using Applications.Features.Loans.Responses;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Identity;
using Infrastructures.Authorizations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TrapchumApp.Helpers.Extensions;
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

        private RequestResult _result = new();

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

            await AppState.GetAuthenticatedUser(user);

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

            model!.LoanRequestStatusRefId = (int)RequestStatus.Requested;

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

            var command = new SaveEditLoanRequestCommand(model);

            var rsp = await mediator.Send(command);

            _result = rsp.Value with
            {
                Message = rsp.Value.Message,
                Success = rsp.Value.Success
            };

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

        async Task DisbruseLoanRequestNow()
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

            bool dialogueResult = await DialogService.OpenAsync<ConfirmModal>("Confirm", param!, options);

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

                var cmd = new AddDescriptionCommand(description);

                await mediator.Send(cmd);

                NotificationService.Notify(NotificationSeverity.Success, rsp.Value.Message!);
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


            var command = new RecheckedLoanRequestCommand(req);


            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Value.Message!);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
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

            var saveReq = new SaveLoanDisbursementDtoCreate(BranchId, CustomerId, LoanRequestId, memo);

            var command = new SaveLoanDisbursementCommand(saveReq);

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Value.Message!);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
            }
        }
    }
}