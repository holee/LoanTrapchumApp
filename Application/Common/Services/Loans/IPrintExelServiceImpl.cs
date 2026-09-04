using Application.Responses;

namespace Application.Services.Loans
{
    public interface IPrintExelServiceImpl 
    {
        Task<Result<Stream>> DownLoadScheduleAsExcel(string? loanId, int? branchId);
    }

}


