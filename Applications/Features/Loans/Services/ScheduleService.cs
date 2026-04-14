using Applications.Features.Loans.Dtos.Responses;
using Applications.Interfaces;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace Applications.Features.Loans.Services;

public sealed class ScheduleService
{
    private readonly Interfaces.IScheduleService _scheduleService;

    private readonly ILogger<ScheduleService> _logger;
    public ScheduleService(Interfaces.IScheduleService scheduleService, ILogger<ScheduleService> logger)
    {
        _scheduleService = scheduleService;

        _logger = logger;
    }

    public async Task<ErrorOr<IEnumerable<LoanResponse>>> GetLoanSchedule2(int? branchId, string? loanId)
    {

        _logger.LogInformation("Fetching loan schedule for branchId: {BranchId} and loanId: {LoanId}", branchId, loanId);

        if (!branchId.HasValue || string.IsNullOrEmpty(loanId))
        {

            return Error.Validation($"We need branchId and LoanId to fetch data.");
        }

        return await _scheduleService.GetLoanWithSchedules2(branchId, loanId); 
    }

    public async Task<ErrorOr<LoanResponse>> CreatePdf(int? branchId, string? loanId, int? scheduleId)
    {
   
        _logger.LogInformation("Creating PDF for loan schedule with branchId: {BranchId}, loanId: {LoanId}, and scheduleId: {ScheduleId}", branchId, loanId, scheduleId);


        if (!branchId.HasValue || string.IsNullOrEmpty(loanId))
        {

            return Error.Validation($"We need branchId and LoanId to fetch data.");
        }

        return await _scheduleService.CreatdPdf(branchId, loanId, scheduleId);
    }

    public async Task<ErrorOr<IEnumerable<LoanResponse>>> GetLoanSchedule(int? branchId, string? loanId)
    {

        _logger.LogInformation("Fetching loan schedule for branchId: {BranchId} and loanId: {LoanId}", branchId, loanId);

        if (!branchId.HasValue || string.IsNullOrEmpty(loanId))
        {

            return Error.Validation($"We need branchId and LoanId to fetch data.");
        }

        return await _scheduleService.GetLoanDisbursments(branchId, loanId);
    }
}
