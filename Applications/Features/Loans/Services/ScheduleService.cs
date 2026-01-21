using Applications.Features.Loans.Responses;
using Applications.Interfaces;
using Applications.Responses;

namespace Applications.Features.Loans.Services
{
    public sealed class ScheduleService
    {
        private readonly ILoanScheduleRepo _scheduleService;

        public ScheduleService(ILoanScheduleRepo scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public async Task<Result<IEnumerable<LoanScheduleResponse>>> GetLoanSchedule2(int? branchId, string? loanId)
        {

            if (!branchId.HasValue || string.IsNullOrEmpty(loanId))
            {

                return Result.Failure<IEnumerable<LoanScheduleResponse>>(ErrorResponse.Validation($"We need branchId and LoanId to fetch data."));
            }

            return await _scheduleService.GetLoanWithSchedules2(branchId, loanId); 
        }

        public async Task<Result<LoanScheduleResponse>> CreatePdf(int? branchId, string? loanId, int? scheduleId)
        {

            if (!branchId.HasValue || string.IsNullOrEmpty(loanId))
            {

                return Result.Failure<LoanScheduleResponse>(ErrorResponse.Validation($"We need branchId and LoanId to fetch data."));
            }

            return await _scheduleService.CreatdPdf(branchId, loanId, scheduleId);
        }

        public async Task<Result<IEnumerable<LoanScheduleResponse>>> GetLoanSchedule(int? branchId, string? loanId)
        {

            if (!branchId.HasValue || string.IsNullOrEmpty(loanId))
            {

                return Result.Failure<IEnumerable<LoanScheduleResponse>>(ErrorResponse.Validation($"We need branchId and LoanId to fetch data."));
            }

            return await _scheduleService.GetLoanDisbursments(branchId, loanId);
        }
    }
}
