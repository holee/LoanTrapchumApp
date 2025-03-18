using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Schedules;
using Dapper;
using Infrastructures.DataAccess;

namespace Infrastructures.Implements
{
    public class SchedulesService : ISchedulesServiceImpl
    {
        private readonly IDapperConnectionFactory _connectionFactory;

        public SchedulesService(IDapperConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task<Result<VCustomerSchedules>> GetCustomerSechedule(long loanId, long customerId, int branchId)
        {
            var connection = _connectionFactory.CreateConnection();

            var sql = "SELECT * FROM VCustomerLoans WHERE LoanId=@LoanId AND CustomerId=@customerId AND BranchId=@branchId";

            var loan = await connection.QueryFirstOrDefaultAsync<VCustomerSchedules>(sql, new
            {
                loanId,
                customerId,
                branchId
            });

            if (loan is null)
            {
                return Result.Failure<VCustomerSchedules>(ErrorResponse.NotFound("No Schedule to print."));
            }

            var sql1 = @$"SELECT 
                           {nameof(VLoanSchedules.InstallmentNo)},
                           {nameof(VLoanSchedules.CurrencyId)},
                           {nameof(VLoanSchedules.PaymentDate)},
                           {nameof(VLoanSchedules.TotalDay)},
                           {nameof(VLoanSchedules.Principal)},
                           {nameof(VLoanSchedules.Interest)},
                           {nameof(VLoanSchedules.TotalPay)},
                           {nameof(VLoanSchedules.LoanOutstanding)}
                        FROM VCustomerLoanSchedules s WHERE s.LoanId=@LoanId AND s.CustomerId=@customerId AND s.BranchId=@branchId
                        ";

            var schedules = await connection.QueryAsync<VLoanSchedules>(sql1, new
            {
                loanId,
                customerId,
                branchId
            });

            if (schedules.Any())
            {
                loan.LoanSchedules = schedules.ToList();
            }

            return Result.Success<VCustomerSchedules>(loan);

        }
    }
}
