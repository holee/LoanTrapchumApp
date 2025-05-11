using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.ViewModels.Reports;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using MiniExcelLibs;

namespace Applications.Services.Loans
{
    public sealed class PrintExelServiceImpl(IWebHostEnvironment env, ILoanService loanService) : IPrintExelServiceImpl
    {
        public async Task<Result<Stream>> DownLoadScheduleAsExcel(string? loanId, int? branchId)
        {


            var rsp = await loanService.GetLoanByIdAsync(loanId, branchId, ScheduleStatus.All);


            if (!rsp.IsSuccess) {

                return Result.Failure<Stream>(ErrorResponse.NotFound("No data to print."));            
            
            }

            MemoryStream ms = new MemoryStream();

            var item = rsp!.Data!.Schedules;

            var loanInfo = rsp.Data!;

            var data = item.Adapt<IEnumerable<ScheduleRpt>>();

            var templatePath = Path.Combine(env.WebRootPath, "Reports/Schedule.xlsx");

            var dataTemplate = new
            {
                name = loanInfo.CustomerName,

                period = $"{loanInfo.Period} {loanInfo.PeriodName}",

                address = loanInfo.Address,

                circle = loanInfo.Circle,

                agent = loanInfo.AgentName,

                agentPhone = loanInfo.AgentPhone,

                lastDate = loanInfo.LastPaymentDate,

                phone = loanInfo.Phone,

                date = loanInfo.RegisterDate!.Value.ToString("dd/MM/yyyy"),

                l = data,

                totalDay = data.Sum(x => x.TotalDay),

                principle = data.Sum(x => x.Principal),

                interest = data.Sum(x => x.Interest)
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

