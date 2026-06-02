using Applications.Commons.Authorizations;
using Applications.Commons.Mappers;
using Applications.Extensions;
using Applications.Features.LoanRequests.Command.SaveEditLoanRequest;
using Applications.Features.LoanRequests.Command.SaveLoanRequests;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestDetailById;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TrapchumApp.Services;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.LoanRequests
{
    public partial class LoanRequestDetail
    {
        [CascadingParameter] Task<AuthenticationState> State { get; set; } = default!;
        [CascadingParameter] public CascadingAppState AppState { get; set; } = default!;
        [Parameter] public string? LoanRequestId { get; set; }
        [Parameter] public int? BranchId { get; set; }
        [Parameter] public string? CustomerId { get; set; }
        [Parameter, SupplyParameterFromQuery] public string? CreditAgentId { get; set; } = string.Empty;

        VLoanRequests? _request;

        private RequestResult _result = new();

        string memo = "";

        string? aprovedUserId = string.Empty;

        bool canApprove = false;

        bool canAllow = false;

        bool _isLoading = true;

        int _dataVersion;

        ClaimsPrincipal? user;

        private bool disableTextbox = false;

        BadgeStyle StatusBadgeStyle => _request?.StatusId switch
        {
            (int)RequestStatus.Approved => BadgeStyle.Success,
            (int)RequestStatus.Disbursed => BadgeStyle.Info,
            (int)RequestStatus.Denied => BadgeStyle.Danger,
            (int)RequestStatus.Rechecked or (int)RequestStatus.Rechecking => BadgeStyle.Warning,
            _ => BadgeStyle.Light
        };

        protected override async Task OnInitializedAsync()
        {
            user = (await State).User;

            aprovedUserId = user.GetId();

            _ = userServices.GetLoanSizeByUserAsync();

            //await AppState.GetAuthenticatedUser(user);

            await LoadDataAsync();

            _ = AppState.WhoCanApproved((_request, userServices.UserLoanSize));

            _ = AppState.WhoCanRequest(_request);

            _ = AppState.WhoCanDisbursemnt(_request);


            if (_request is not null)
            {
                await AppState.CheckedNormalApproved(user, _request);

                await AppState.CheckedMaximumApproved(user, _request);
            }

            await base.OnInitializedAsync();
        }

        private async Task LoadDataAsync()
        {
            _isLoading = true;

            try
            {
                await GetLoanRequestDetails(user!);
            }
            finally
            {
                _isLoading = false;
            }
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

        async Task CreateLoanRequest()
        {

            if (await MsgBox.ConfirmAsync("Do you want to approve this request?") == MsgBoxResult.No) return;

            var model = _request?.MapToLoanRequestCreate();

            if (model is null)
            {

                _toastService.ShowInfo("Loan request data is null.");

                return;
            }

            model.LoanRequestStatusRefId = (int)RequestStatus.Requested;

            if (memo.IsNotNullOrEmpty())
            {
                var description = new LoanDescriptionsDtoCreate(memo, aprovedUserId, LoanRequestId);
                model.LoanDescriptions.Add(description);
            }

            var command = new SaveEditLoanRequestCommand(model);

            var rsp = await mediator.Send(command);

            if (!rsp.IsError)
            {
                _result = rsp.Value with
                {
                    Message = rsp.Value.Message,
                    Success = rsp.Value.Success
                };

                _toastService.ShowSuccess("New Loan Request was request successfully.");

            }
            else
            {
                _toastService.ShowError(rsp.GetMessage());
            }

            StateHasChanged();
        }

        async Task GetLoanRequestDetails(ClaimsPrincipal user, bool include = true)
        {
            var query = new GetLoanRequestDetailByIdQuery(BranchId, CustomerId, LoanRequestId, include);

            var resp = await mediator.Send(query);

            if (!resp.IsError)
            {
                _request = resp.Value;

                canAllow = (await _authorizeService.AuthorizeAsync(user, _request, new SameUserRequestRequirement()))
                    .Succeeded;

                canApprove = (await _authorizeService.AuthorizeAsync(user, _request, new SameUserApprovedRequirement()))
                    .Succeeded;

                disableTextbox = _request?.StatusId == (int)RequestStatus.Approved;

                _dataVersion++;
            }
        }

        async Task DisbruseLoanRequestNow()
        {

            if (_request is null) return;

            var disburseRequestLoanUrl = $"/App/Loans/{LoanRequestId}/Disbursements/{BranchId}/Branches/CreateNew?CustomerId={CustomerId}";

            NavigationManager.NavigateTo(disburseRequestLoanUrl);

        }

        async Task DeniedLoanRequest()
        {

            var confirm = await MsgBox.ConfirmAsync("Do you want to reject this request?");

            if (confirm == MsgBoxResult.No) return;

            var req = new SaveLoanRequestDtoCreate(BranchId, CustomerId, LoanRequestId, memo, RequestStatus.Denied);

            var command = new SaveLoanRequestsCommand(req);

            var rsp = await mediator.Send(command);

            if (!rsp.IsError)
            {

                NotificationService.Notify(NotificationSeverity.Success, rsp.Value.Message!);

                await RefreshDataAsync();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
            }
        }

        async Task RecheckedLoanRequest()
        {

            var confirm = await MsgBox.ConfirmAsync("Do you want to return back this request?", "Loan Request");

            if (confirm == MsgBoxResult.No) return;

            var req = new SaveLoanRequestDtoCreate(BranchId, CustomerId, LoanRequestId, memo, RequestStatus.Rechecked);

            var command = new SaveLoanRequestsCommand(req);

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {

                _toastService.ShowSuccess(rsp.Value.Message);

                await RefreshDataAsync();
            }
            else
            {
                _toastService.ShowError(rsp.GetMessage());
            }
        }

        async Task SaveLoanRequestAccount()
        {

            if (await MsgBox.ConfirmAsync("Do you want to save this request?") == MsgBoxResult.No) return;

            var saveReq = new SaveLoanRequestDtoCreate(BranchId, CustomerId, LoanRequestId, memo, RequestStatus.Approved);

            var command = new SaveLoanRequestsCommand(saveReq);

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {
                _toastService.ShowSuccess(rsp.Value.Message);

                await RefreshDataAsync();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());

                _toastService.ShowError(rsp.GetMessage());
            }
        }

        async Task SaveLoanRequestManager()
        {

            var dia = await MsgBox.ShowAsync("Do you want to save this request?", "Loan Request", MsgBoxButtons.YesNo, MsgBoxBoxIcon.Question);

            if (dia == MsgBoxResult.No)
            {
                _toastService.ShowInfo("Request was cancelled.");

                return;
            }

            var saveReq = new SaveLoanRequestDtoCreate(BranchId, CustomerId, LoanRequestId, memo, RequestStatus.Requested);

            var command = new SaveLoanRequestsCommand(saveReq);

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {
                _toastService.ShowSuccess(rsp.Value.Message!);

                await RefreshDataAsync();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
            }
        }


        async Task ApproveLoanRequest()
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




        async Task SendLoanRequest()
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

            bool dialogueResult = await DialogService.OpenAsync<PassToModal>("Confirm", param!, options);

            if (dialogueResult && user is not null)
            {
                await GetLoanRequestDetails(user, true);

                disableTextbox = true;

                StateHasChanged();
            }
        }



        private async Task RefreshDataAsync()
        {
            if (user is not null)
            {
                await GetLoanRequestDetails(user, true);

                StateHasChanged();
            }
        }
    }
}