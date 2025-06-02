using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Responses.ViewModels.Reports;
using Applications.Schedules;
using Applications.Services.Identity;
using Applications.Services.Loans;
using QuestPDF.Fluent;

namespace Applications.Services.Reports
{
    public interface ILoanReportServiceImpl
    {
        Task<DisbursmentsRpt?> GetLoanScheduleAsync(int? branchId, string? loanId);
    }

    public sealed class LoanReportServiceImpl(ILoanReportService loanReportService) : ILoanReportServiceImpl
    {
        public async Task<DisbursmentsRpt?> GetLoanScheduleAsync(int? branchId, string? loanId)
        {
            return await loanReportService.GetLoanScheduleAsync(branchId, loanId);
        }

      
    }
}

//public async Task<Result<byte[]>> ExportScheduleAsPdf(string? loanId, int? branchId)
//{
//    var rsp = await loanService.GetLoanByIdAsync(loanId, branchId, ScheduleStatus.All);

//    if (!rsp.IsSuccess)
//    {
//        return Result.Failure<byte[]>(ErrorResponse.NotFound("No Schedule to print"));
//    }
//    var item = rsp.Data.Adapt<LoanScheduleVm>();

//    var pdf = new SchedulePdfService(item);

//    return Result.Success(pdf.GeneratePdf());
//}



//public async Task<Result<Stream>> DownLoadScheduleAsPdf(string? loanId, int? branchId)
//{

//    var rsp = await loanService.GetLoanByIdAsync(loanId, branchId, ScheduleStatus.All);

//    if (rsp.IsSuccess)
//    {

//        var item = rsp.Data.Adapt<LoanScheduleVm>();

//        var pdf = new SchedulePdfService(item);

//        var stream = new MemoryStream(pdf.GeneratePdf());

//        return Result.Success<Stream>(stream);

//    }

//    return Result.Failure<Stream>(ErrorResponse.NotFound(""));
//}
