using Application.Features.Schedules.Dto;

namespace Application.Services.Loans;


public interface ILoanPaymentServiceImpl
{
    //Task<Result<string>> CreatePayment(LoanPaymentTransactionDtoCreate req, CancellationToken cancellation = default);
    //Task<Result<IEnumerable<VSchedule>>> GetPaidedScheduleAsync(int? branchId, string? loanRefId);
    //Task<Result<IEnumerable<LoanPaymentLineVm>>> GetPaymentLinesAsync(int? branchId, string? loanRefId, int? installmentNo);
    //Task<Result<ScheduleDetailsDto>> GetScheduleByIdAsync(long? scheduleId, int? branchId, string? loanRefId);
    //Task<Result<string>> RecievePayment(LoanPaymentDtoCreate req);
    //Task<Result<string>> RecievePayment(LoanPaymentTransactionDtoCreate req, int? brancId);
}



//public sealed class LoanPaymentService(ILoanPaymentService lnService) : ILoanPaymentServiceImpl
//{
//    public async Task<Result<IEnumerable<VSchedule>>> GetPaidedScheduleAsync(int? branchId, string? loanRefId)
//    {
//        return await lnService.GetPaidScheduleAsync(branchId, loanRefId);
//    }

//    public async Task<Result<ScheduleDetailsDto>> GetScheduleByIdAsync(long? scheduleId, int? branchId, string? loanRefId)
//    {
//        return await lnService.GetScheduleByIdAsync(scheduleId, branchId, loanRefId);
//    }



//    //public async Task<Result<IEnumerable<PaymentLineDetailsDto>>> GetPaymentLinesAsync(int? branchId, string? loanRefId, int? installmentNo)
//    //{
//    //    return await lnService.GetPaymentLinesAsync(branchId, loanRefId, installmentNo);
//    //}

//    //public async Task<Result<string>> RecievePayment(LoanPaymentDtoCreate req)
//    //{
//    //    return await lnService.RecievePayment(req);
//    //}

//    //public async Task<Result<string>> RecievePayment(LoanPaymentTransactionDtoCreate req, int? branchId)
//    //{

//    //    if (!branchId.HasValue)
//    //    {
//    //        return Result.Failure<string>(ErrorResponse.Validation("We required branch to do operation!."));
//    //    }

//    //    var model = req.ToLoanTransction(branchId);

//    //    return await lnService.RecievePayment(model);

//    //}

//    //public async Task<Result<string>> CreatePayment(LoanPaymentTransactionDtoCreate req, CancellationToken cancellation = default)
//    //{

//    //    var model = req.ToLoanTransction();

//    //    return await lnService.CreatePayment(model, cancellation);

//    //}

//}
