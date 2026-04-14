namespace Applications.Features.LoanPayments.UseCases;

//public sealed class GetPaymentScheduleUseCase : IGetPaymentScheduleUseCase
//{

//}


//public sealed class GetPaymentScheduleByLoanIdUseCase : IPaymentScheduleByLoanIdUseCase
//{
//    //private readonly ILoanPaymentService _loanPaymentService;

//    //private readonly ILoanService _loanService;
//    //public GetPaymentScheduleByLoanIdUseCase(ILoanPaymentService loanPaymentService, ILoanService loanService)
//    //{
//    //    _loanPaymentService = loanPaymentService;

//    //    _loanService = loanService;
//    //}


//    //public async Task<Result<VLoanDisbursments>> ExecuteAsync(string? loanId, int? branchId, ScheduleStatus state,bool include,CancellationToken cancellation=default)
//    //{

//    //    if (string.IsNullOrEmpty(loanId) || !branchId.HasValue)
//    //    {
//    //        return Result.Failure<VLoanDisbursments>(ErrorResponse.Validation("LoanId and BranchId must be provided."));
//    //    }

//    //    return await _loanService.GetLoanByIdAsync(loanId, branchId, state, include,cancellation);

//    //}
//}