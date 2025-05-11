using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Schedules;
using Dapper;
using Infrastructures.DataAccess;

namespace Infrastructures.Implements
{
    public class SchedulesService : ISchedulesService
    {
        private readonly IDapperConnectionFactory _connectionFactory;

        public SchedulesService(IDapperConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task<Result<LoanScheduleVm>> GetCustomerSechedule(long loanId, long customerId, int branchId)
        {
            var connection = _connectionFactory.CreateConnection();

            var sql = "SELECT * FROM VCustomerLoans WHERE LoanId=@LoanId AND CustomerId=@customerId AND BranchId=@branchId";

            var loan = await connection.QueryFirstOrDefaultAsync<LoanScheduleVm>(sql, new
            {
                loanId,
                customerId,
                branchId
            });

            if (loan is null)
            {
                return Result.Failure<LoanScheduleVm>(ErrorResponse.NotFound("No Schedule to print."));
            }

            var sql1 = @$"SELECT 
                           {nameof(ScheduleViewModel.InstallmentNo)},
                           {nameof(ScheduleViewModel.CurrencyId)},
                           {nameof(ScheduleViewModel.PaymentDate)},
                           {nameof(ScheduleViewModel.TotalDay)},
                           {nameof(ScheduleViewModel.Principal)},
                           {nameof(ScheduleViewModel.Interest)},
                           {nameof(ScheduleViewModel.TotalPay)},
                           {nameof(ScheduleViewModel.LoanOutstanding)}
                        FROM VCustomerLoanSchedules s WHERE s.LoanId=@LoanId AND s.CustomerId=@customerId AND s.BranchId=@branchId
                        ";

            var schedules = await connection.QueryAsync<ScheduleViewModel>(sql1, new
            {
                loanId,
                customerId,
                branchId
            });

            if (schedules.Any())
            {
                loan.LoanSchedules = schedules.ToList();
            }

            return Result.Success<LoanScheduleVm>(loan);

        }
    }
}
