using Applications.Responses;
using Applications.Schedules;
using Applications.Services.Reports;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;

namespace Applications.Services.Loans
{
    public sealed class PrintPdfServiceImpl(ILoanReportServiceImpl loanService, ILogger<PrintPdfServiceImpl> logger) : IPrintPdfServiceImpl
    {

        public async Task<Result<Stream>> DownLoadScheduleAsExcel(int? branchId,string? loanId)
        {

            logger.LogInformation($"getting data from Loan:{loanId} and branch: {branchId}");

            try
            {

                var data = await loanService.GetLoanScheduleAsync(branchId,loanId);

                if (data is null)
                {
                    return Result.Failure<Stream>(ErrorResponse.NotFound("There is no any schedule to print"));
                }

                var pdf = new SchedulePdfService(data);

                var stream =new MemoryStream(pdf.GeneratePdf());


                logger.LogInformation("gettign loan schedule successfull");

                return Result.Success<Stream>(stream);


            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);

                return Result.Failure<Stream>(ErrorResponse.NotFound($"Error due to: {ex.Message}"));
            }
        }

        public async Task<Result<Stream>> DownLoadScheduleAsPdf(int? branchId,string? loanId)
        {

            logger.LogInformation($"getting data from Loan:{loanId} and branch: {branchId}");

            try
            {

                //var data = await loanService.GetLoanScheduleAsync(branchId, loanId);

                var data = await loanService.PrintLoanScheduleAsync(branchId, loanId);


                if (data is null)
                {
                    return Result.Failure<Stream>(ErrorResponse.NotFound("There is no any schedule to print"));
                }


                var pdf = new SchedulePdfService(data);


                var stream = new MemoryStream(pdf.GeneratePdf());


                logger.LogInformation("gettign loan schedule successfull");

                return Result.Success<Stream>(stream);


            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);

                return Result.Failure<Stream>(ErrorResponse.NotFound($"Error due to: {ex.Message}"));
            }
        }


       
    }

}

//public async Task<Result<byte[]>> ExportScheduleAsPdf(string? loanId, int? branchId)
//{
//    logger.LogInformation($"getting data from Loan:{loanId} and branch: {branchId}");

//    try
//    {

//        var rsp = await loanService.GetLoanByIdAsync(loanId, branchId, ScheduleStatus.All);

//        if (!rsp.IsSuccess)
//        {
//            return Result.Failure<byte[]>(ErrorResponse.NotFound("There is no any schedule to print"));
//        }

//        var loanwithSchedules = rsp.Data?.ToLoanScheduleVm();

//        if (loanwithSchedules is null)
//        {
//            return Result.Failure<byte[]>(ErrorResponse.NotFound("There is no any schedule to print"));
//        }

//        var pdf = new SchedulePdfService(loanwithSchedules);


//        var stream = pdf.GeneratePdf();


//        logger.LogInformation("gettign loan schedule successfull");

//        return Result.Success<byte[]>(stream);


//    }
//    catch (Exception ex)
//    {
//        logger.LogError(ex.Message, ex);

//        return Result.Failure<byte[]>(ErrorResponse.NotFound($"Error due to: {ex.Message}"));
//    }
//}

//Task<Result<string>> CreateLoanAsync(LoanDtoCreate disbursement);
//Task<Result<string>> DeleteAsync(long loanId,int branchId);
//Task<Result<string>> UpdateAsync(Loan disbursement);
//Task<Result<string>> ChangedCreditAgent(int branchId, LoanChangedCaDtoCreate req);
//Task<Result<string>> ChangedCreditAgents(int branchId, List<LoanChangedCaDtoCreate> req);
//Task<Result<VLoanDisbursments>> GetLoanByIdAsync(long loanId, int branchId, ScheduleStatus status);
//Task<Pager<VLoanDisbursments>> GetLoansAsync(int branchId, long creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null);
//Task<Result<IEnumerable<VSchedule>>> GetPaymentSchedulesByLoanId(long? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All);


//Task<Result<IEnumerable<VSchedule>>> GetLoanPaymentSchedules(DateTime paymentDate, int branchId, bool includeLateSchedule = false, string? filter = null);
//Task<Result<Loan>> GetLoanByIdAsync(long loanId, int branchId, params string[] includes);
//Task<Result<IEnumerable<VPaymentLines>>> GetPayementScheduleLines(int branchId, long loanId, int installmentNo);
//Task<Result<IEnumerable<VSchedule>>> GetLatePaymentSchedules(int? branchId, DateTime? paymentDate = null, string? filter = null);
//Task<Result<VSchedule>> GetPaymentScheduleById(long loanId, long  loanScheduleId, int branchId, bool related);
//Task<Result<IEnumerable<VSchedule>>> GetCustomerPaymentSchedules(long? loanId, int? branchId, ScheduleStatus status, DateTime? selectedDate, bool onScheduleOnly = false, string? filter = null);
