using Applications.Extensions.DateTimes;
using Applications.Extensions.Maths;
using Applications.Responses;
using Applications.Services.Reports;
using Microsoft.AspNetCore.Hosting;
using MiniExcelLibs;

namespace Applications.Services.Loans
{
    public sealed class PrintExelServiceImpl(IWebHostEnvironment env, ILoanReportServiceImpl loanService) : IPrintExelServiceImpl
    {
        public async Task<Result<Stream>> DownLoadScheduleAsExcel(string? loanId, int? branchId)
        {


            var rsp = await loanService.GetLoanScheduleAsync(branchId, loanId);

            MemoryStream ms = new MemoryStream();

            var item = rsp!.Schedules;

            var templatePath = Path.Combine(env.WebRootPath, "Reports/Schedule.xlsx");

            var dataTemplate = new
            {
                name = rsp.FullName,

                period = rsp.PeriodName,

                address = rsp.Address,

                circle = rsp.Circle,

                agent = rsp.AgentName,

                agentPhone = rsp.AgentPhone,

                lastDate = rsp.LastPaymentDate,

                phone = rsp.Phone,

                date = rsp.RegisterDate.DayFirst(),

                l = rsp.Schedules,

                totalDay = rsp.TotalDay,

                principle = rsp.TotalPrinciple.SetFormat(rsp.CurrencyId),

                interest = rsp.TotalInterest.SetFormat(rsp.CurrencyId)
            };

            await ms.SaveAsByTemplateAsync(templatePath, dataTemplate);

            ms.Seek(0, SeekOrigin.Begin);

            return Result.Success<Stream>(ms);
        }

        public Task<Result<byte[]>> ExportScheduleAsExcel(string? loanId, int? branchId)
        {
            throw new NotImplementedException();
        }
    }

}

