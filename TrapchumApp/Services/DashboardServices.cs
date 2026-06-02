using Applications.Features.Dashbaord.Accountants.Dtos;
using Applications.Features.Dashbaord.Accountants.Queries.GetDbAccountLoanSummaery;
using Applications.Features.Dashbaord.Accountants.Queries.GetDbApprovedLoanRequest;
using Applications.Features.Dashbaord.CreditOfficers.Dtos;
using Applications.Features.Dashbaord.CreditOfficers.Queries.GetDbCreditOfficeLoanSummary;
using Applications.Features.Dashbaord.Managers.Dto;
using Applications.Features.Dashbaord.Managers.Queries.GetDbManagerLoanSummary;
using Applications.Interfaces;
using Applications.Utils;

namespace TrapchumApp.Services;

public sealed class DashboardServices(ICurrentUserServices userServices, BranchService branchService, IMediator mediator)
{
    public int NumberOfLoanRequests { get; private set; } = 0;
    public CoLoanSummary? CoSummaries { get; private set; } = null;
    public AccountSummary? AccSummaries { get; private set; } = null;
    public DbManagerLoanSummary? ManagerSummaries { get; private set; } = null;
    public async Task GetCoDashboard()
    {

        await branchService.GetCurrentBranchIdAsync();

        var branchId = branchService.BranchId;

        if (branchId.HasValue && branchId > 0)
        {

            var command3 = new GetDbCreditOfficeLoanSummaryQuery(branchId, userServices.UserId, null);

            var rsp3 = await mediator.Send(command3);

            if (rsp3.IsError)
            {
                CoSummaries = null;
            }
            else
            {
                CoSummaries ??= rsp3.Value;
            }

        }
    }


    public async Task GetAccDashboard()
    {

        await branchService.GetCurrentBranchIdAsync();

        var branchId = branchService.BranchId;

        if (branchId.HasValue && branchId > 0)
        {

            var command2 = new GetDbAccountLoanSummaryQuery(branchId, null);

            var rsp2 = await mediator.Send(command2);

            if (rsp2.IsError)
            {
                AccSummaries = null;
            }
            else
            {
                AccSummaries = rsp2.Value;
            }

            var command3 = new GetDbApprovedRequestQuery(branchId);

            var rsp3 = await mediator.Send(command3);

            NumberOfLoanRequests = rsp3.Value;

        }
        else
        {

            AccSummaries = null;

            NumberOfLoanRequests = 0;
        }
    }


    public async Task GetManagerDashboard()
    {

        await branchService.GetCurrentBranchIdAsync();

        var branchId = branchService.BranchId;

        if (branchId.HasValue && branchId > 0)
        {
            var command4 = new GetDbManagerLoanSummaryQuery(branchId, userServices.UserId);

            var rsp4 = await mediator.Send(command4);

            ManagerSummaries ??= rsp4.Value;
        }
    }





}
