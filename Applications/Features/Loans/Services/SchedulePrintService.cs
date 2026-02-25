using Applications.Features.Schedules;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;

namespace Applications.Features.Loans.Services
{
    public sealed class SchedulePrintService(ILogger<SchedulePrintService> logger, ScheduleService loanService)
    {
        public async Task<Result<Stream>> DownLoadScheduleAsPdf(int? branchId, string? loanId)
        {

            logger.LogInformation($"getting data from Loan:{loanId} and branch: {branchId}");

            try
            {
                var rsp = await loanService.GetLoanSchedule2(branchId, loanId);

                if (rsp.IsError)
                {
                    return Result.Failure<Stream>(ErrorResponse.NotFound("There is no any schedule to print"));
                }

                var data = rsp.Value;

                if (data is null || !data.Any())
                {
                    return Result.Failure<Stream>(ErrorResponse.NotFound("There is no any schedule to print"));
                }


                var pdf = SchedulePdfPrintService.CreatePdf(data);


                var stream = new MemoryStream(pdf);


                logger.LogInformation("gettign loan schedule successfull");


                return Result.Success<Stream>(stream);


            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);

                return Result.Failure<Stream>(ErrorResponse.NotFound($"Error due to: {ex.Message}"));
            }
        }
        public async Task<Result<Stream>> DownLoadScheduleAsPdf(int? branchId, string? loanId, int? scheduleId = 1)
        {

            logger.LogInformation($"getting data from Loan:{loanId} and branch: {branchId}");

            try
            {


                var rsp = await loanService.CreatePdf(branchId, loanId,scheduleId);


                if (rsp.IsError)
                {
                    return Result.Failure<Stream>(ErrorResponse.NotFound("There is no any schedule to print"));
                }


                var data = rsp.Value;

                if (data is null)
                {
                    return Result.Failure<Stream>(ErrorResponse.NotFound("There is no any schedule to print"));
                }


                var pdf = new ScheduleByScheduleDocument(data);   //SchedulePdfPrintService.CreatePdf(data);


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
