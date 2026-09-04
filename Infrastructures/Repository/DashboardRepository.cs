using Applications.Features.Dashbaord.Accountants.Dtos;
using Applications.Features.Dashbaord.CreditOfficers.Dtos;
using Applications.Features.Dashbaord.Dto;
using Applications.Features.Dashbaord.Managers.Dto;
using Applications.Interfaces;
using Applications.Responses.Paginations;
using Dapper;
using ErrorOr;
using Infrastructures.DataAccess;

namespace Infrastructures.Repository
{
    public sealed class DashboardRepository(IDapperConnectionFactory connectionFactory) : IDashboardService
    {
        private const string DBAccountLoanSummary = "dbAccountLoanSummary";

        private const string DBCreditOfficerLoanSummary = "dbCreditOfficeLoanSummary";

        private const string DBManagerLoanSummary = "spDbManagerSummaryByBranch";


        public async Task<Pager<AccountantLoanSummary>> GetAccountantLoanSummaries(int? branchId, DateTime? dueDate, CancellationToken cancellation = default)
        {
            using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

            var result = await connection.QueryAsync<AccountantLoanSummary>(DBAccountLoanSummary, new { BranchId = branchId, DueDate = dueDate });

            var counts = result.Count();

            return PagerExtensions.Create(result.ToList(), counts);
        }


        public async Task<AccountSummary> GetAccountantSummaries(int? branchId, DateTime? dueDate, CancellationToken cancellation = default)
        {
            using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

            var loanTask = connection.QueryAsync<AccountantLoanSummary>(DBAccountLoanSummary, new { BranchId = branchId, DueDate = dueDate });

            var dbCoRequestSummaryTask = connection.QueryAsync<LoanRequestSummary>(@"SET NOCOUNT ON;
                                                                SELECT
                                                                    s.Id,
                                                                    s.Status,
                                                                    ISNULL(r.TotalRequests, 0) AS TotalRequests
                                                                FROM st_loanRequestStatus s
                                                                LEFT JOIN
                                                                (
                                                                    SELECT
                                                                        statusId,
                                                                        COUNT(*) AS TotalRequests
                                                                    FROM VLoanRequests
                                                                    WHERE statusId BETWEEN 1 AND 12 AND BranchId = @BranchId
                                                                    GROUP BY statusId
                                                                ) r ON r.statusId = s.Id
                                                                WHERE s.Id BETWEEN 1 AND 12
                                                                ORDER BY s.Id;
                                                                ", new { BranchId = branchId });

            await Task.WhenAll(loanTask, dbCoRequestSummaryTask).WaitAsync(cancellation);

            return new AccountSummary(loanTask.Result.ToList(),dbCoRequestSummaryTask.Result.ToList());

        }



        public async Task<ErrorOr<int>> GetApprovedLoanRequest(int? branchId, CancellationToken cancellation = default)
        {

            using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

            string sql = "SELECT COUNT(*) FROM VLoanRequests WHERE BranchId=@BranchId AND StatusId=4;";

            var result = await connection.ExecuteScalarAsync<int>(sql, new { BranchId = branchId });

            return result;
        }


        public async Task<ErrorOr<NumerLoanRequest>> GetNumberLoanRequestByCo(int? branchId, string? UserId, CancellationToken cancellation = default)
        {

            using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

            string sql = @"SELECT COUNT(1)
                           FROM VLoanRequests WHERE BranchId=@BranchId 
                            AND StatusId IN (1,6) AND UserRequestId=@UserId;
                          ";
            var result = await connection.QueryFirstAsync<NumerLoanRequest>(sql, new { BranchId = branchId, UserId = UserId });

            return result;
        }

        public async Task<ErrorOr<NumerLoanRequest>> GetNumberDueCustomerByCo(int? branchId, string? UserId, CancellationToken cancellation = default)
        {

            using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

            string sql = @"SELECT COUNT(1)
                           FROM VLoanRequests WHERE BranchId=@BranchId 
                            AND StatusId IN (1,6) AND UserRequestId=@UserId;
                          ";
            var result = await connection.QueryFirstAsync<NumerLoanRequest>(sql, new { BranchId = branchId, UserId = UserId });

            return result;
        }



        public async Task<Pager<DbCoLoanSummary>> GetDbCreditOfficerSummaries(int? branchId, string? UserId,
        DateTime? dueDate, CancellationToken cancellation = default)

        {

            using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

            var result = await connection.QueryAsync<DbCoLoanSummary>(DBCreditOfficerLoanSummary, new { BranchId = branchId, UserId = UserId, DueDate = dueDate });

            if (result is null)
            {
                result = new List<DbCoLoanSummary>
                {
                    new DbCoLoanSummary
                    {
                        TotalCustomer=0,
                        CurrencyId=0,
                        TotalPrinciple=0,
                        TotalInterest=0
                    }
                };
            }

            var counts = result.Count();

            return PagerExtensions.Create(result.ToList(), counts);
        }



        public async Task<CoLoanSummary> GetCreditOfficerSummaries(int? branchId, string? UserId, DateTime? dueDate, CancellationToken cancellation = default)

        {

            using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

            var dbCoLoanSummaryTask = connection.QueryAsync<DbCoLoanSummary>(DBCreditOfficerLoanSummary, new { BranchId = branchId, UserId = UserId, DueDate = dueDate });

            var dbCoRequestSummaryTask = connection.QueryAsync<LoanRequestSummary>(@"SET NOCOUNT ON;
                                                                SELECT
                                                                    s.Id,
                                                                    s.Status,
                                                                    ISNULL(r.TotalRequests, 0) AS TotalRequests
                                                                FROM st_loanRequestStatus s
                                                                LEFT JOIN
                                                                (
                                                                    SELECT
                                                                        statusId,
                                                                        COUNT(*) AS TotalRequests
                                                                    FROM VLoanRequests
                                                                    WHERE statusId BETWEEN 1 AND 12 AND BranchId = @BranchId
                                                                    GROUP BY statusId
                                                                ) r ON r.statusId = s.Id
                                                                WHERE s.Id BETWEEN 1 AND 12
                                                                ORDER BY s.Id;
                                                                ", new { BranchId = branchId });

            await Task.WhenAll(dbCoLoanSummaryTask, dbCoRequestSummaryTask).WaitAsync(cancellation);

            return new CoLoanSummary(dbCoLoanSummaryTask.Result.ToList(),dbCoRequestSummaryTask.Result.ToList());

        }





        public async Task<ErrorOr<int>> GetNumberLoanRequestByManager(int? branchId, string? UserId, CancellationToken cancellation = default)
        {

            using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

            string sql = @"SELECT COUNT(1)
                           FROM VLoanRequests WHERE BranchId=@BranchId 
                           AND StatusId IN (2,3,8) AND AppovedUserId=@UserId;
                          ";
            var result = await connection.ExecuteScalarAsync<int>(sql, new { BranchId = branchId, UserId = UserId });

            return result;
        }

        public async Task<ErrorOr<DbManagerLoanSummary>> GetDbManagerLoanSummary(int? branchId, string? UserId, CancellationToken cancellation = default)
        {

            using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

            var result = await connection.QueryFirstOrDefaultAsync<DbManagerLoanSummary>(DBManagerLoanSummary, new { BranchId = branchId, UserId = UserId });

            return result ?? new();
        }





    }
}


//public async Task<ErrorOr<AccountantLoanSummary>> GetAccountantLoanSummary(int? branchId, DateTime? dueDate, CancellationToken cancellation = default)
//{
//    using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

//    var result = await connection.QueryFirstOrDefaultAsync<AccountantLoanSummary>(DBAccountLoanSummary, new { BranchId = branchId, DueDate = dueDate });

//    return result ?? new AccountantLoanSummary();
//}
