using Applications.Commons.Authorizations;
using Applications.Commons.Authorizations.Handlers;
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
using TrapchumApp.Pages.Transactions.LoanRequests;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Managers;

public partial class ManagerLoanRequestDetail
{
    [CascadingParameter] Task<AuthenticationState> State { get; set; } = default!;
    [CascadingParameter] public CascadingAppState AppState { get; set; } = default!;
    [Parameter] public string? LoanRequestId { get; set; }
    [Parameter] public int? BranchId { get; set; }
    [Parameter] public string? CustomerId { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? CreditAgentId { get; set; } = string.Empty;

    VLoanRequests? data;

    private RequestResult _result = new();

    string memo = "";

    string? aprovedUserId = string.Empty;

    bool canApprove = false;

    bool canAllow = false;

    bool _isLoading = true;

    int _dataVersion;

    ClaimsPrincipal? user;

    private bool disableTextbox = false;

    BadgeStyle StatusBadgeStyle => data?.StatusId switch
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


        await userServices.GetLoanSizeByUserAsync();


        await LoadDataAsync();


        await AppState.WhoCanApproved((data, userServices.UserLoanSize));


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
        if (!await ConfirmActionAsync("Do you want to approve this request?"))
            return;

        var model = data?.MapToLoanRequestCreate();

        if (model is null)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Loan request data is null.");
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

            disableTextbox = data?.StatusId == (int)RequestStatus.Approved;

            _dataVersion++;
        }
    }

    async Task DisbruseLoanRequestNow()
    {

        if (data is null) return;

        var disburseRequestLoanUrl = $"/App/Loans/{LoanRequestId}/Disbursements/{BranchId}/Branches/CreateNew?CustomerId={CustomerId}";

        NavigationManager.NavigateTo(disburseRequestLoanUrl);

    }

    async Task DeniedLoanRequest()
    {
        if (!await ConfirmActionAsync("Do you want to reject this request?"))
            return;

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
        if (!await ConfirmActionAsync("Do you want to return back this request?"))
            return;

        var req = new SaveLoanRequestDtoCreate(BranchId, CustomerId, LoanRequestId, memo, RequestStatus.Rechecked);

        var command = new SaveLoanRequestsCommand(req);

        var rsp = await mediator.Send(command);

        if (rsp.IsSuccess())
        {
            NotificationService.Notify(NotificationSeverity.Success, rsp.Value.Message!);

            await RefreshDataAsync();
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
        }
    }

    async Task SaveLoanRequestAccount()
    {
        if (!await ConfirmActionAsync("Do you want to save this request?"))
            return;

        var saveReq = new SaveLoanRequestDtoCreate(BranchId, CustomerId, LoanRequestId, memo, RequestStatus.Approved);

        var command = new SaveLoanRequestsCommand(saveReq);

        var rsp = await mediator.Send(command);

        if (rsp.IsSuccess())
        {
            NotificationService.Notify(NotificationSeverity.Success, rsp.Value.Message!);

            await RefreshDataAsync();
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
        }
    }

    async Task SaveLoanRequestManager()
    {

        var dia = await MsgBox.ShowAsync("Do you want to save this request?", "Loan Request", MsgBoxButtons.YesNo, MsgBoxBoxIcon.Question);

        if(dia == MsgBoxResult.No)
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
        if(data is null) return;
    
        if (data.IsSubmitted==false)
        {
            _toastService.ShowInfo("This request has not been submitted yet.");

            return;
        }



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