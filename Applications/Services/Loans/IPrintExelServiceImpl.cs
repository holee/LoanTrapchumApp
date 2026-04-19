using Applications.Responses;

namespace Applications.Services.Loans
{
    public interface IPrintExelServiceImpl 
    {
        Task<Result<Stream>> DownLoadScheduleAsExcel(string? loanId, int? branchId);
    }

}


