using Applications.Extensions;
using Applications.Extensions.Attrs;
using Applications.Extensions.Maths;
using MiniExcelLibs;
namespace Applications.Services.Loans;

public sealed class PrintExelServiceImpl(ILoanReportServiceImpl loanService) : IPrintExelServiceImpl
{
    public async Task<Result<Stream>> DownLoadScheduleAsExcel(string? loanId, int? branchId)
    {


        var rsp = await loanService.PrintLoanScheduleAsync(branchId, loanId);

        MemoryStream ms = new MemoryStream();

        var item = rsp!.Schedules;

        var templatePath = Path.Combine("/", "Reports/Schedule.xlsx");

        var dataTemplate = new
        {
            name = rsp.AltName,

            period = rsp.PeriodName,

            address = rsp.Address,

            circle = rsp.Circle,

            agent = rsp.AgentName,

            agentPhone = rsp.AgentPhone,

            lastDate = rsp.LastPaymentDate,

            phone = rsp.Phone,

            date = rsp.RegisterDate.DayFirst(),

            l = rsp.Schedules,

            totalDay = rsp.Circle,

            principle = rsp.OutStanding.SetFormat(rsp.CurrencyId),

            interest = rsp.PaidInterest.SetFormat(rsp.CurrencyId)
        };

        await ms.SaveAsByTemplateAsync(templatePath, dataTemplate);

        ms.Seek(0, SeekOrigin.Begin);

        return Result.Success<Stream>(ms);
    }

}

