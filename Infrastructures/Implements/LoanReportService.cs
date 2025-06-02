using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Responses.ViewModels.Reports;
using Dapper;
using Infrastructures.Constants.Procedures;
using Infrastructures.DataAccess;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Infrastructures.Implements
{
    public sealed class LoanReportService(ILogger<LoanService> logger,IDapperConnectionFactory connectionFactory) : ILoanReportService
    {


        public async Task<DisbursmentsRpt?> GetLoanScheduleAsync(int? branchId, string? loanId)
        {

            ArgumentNullException.ThrowIfNull(branchId, nameof(branchId));

            ArgumentNullException.ThrowIfNull(loanId, nameof(loanId));

            logger.LogInformation("Fetching loan schedule for BranchId: {BranchId}, LoanId: {LoanId}", branchId, loanId);

            using var db = connectionFactory.CreateConnection();

            var lookup = new Dictionary<string, DisbursmentsRpt>();

            var result = await db.QueryAsync<DisbursmentsRpt, ScheduleRpt, DisbursmentsRpt>(
                sql: "rptGetLoanScheduleById",
                map: (disbursement, schedule) =>
                {
                    if (!lookup.TryGetValue(loanId!, out var disb))
                    {
                        disb = disbursement;
                        disb.Schedules = new List<ScheduleRpt>();
                        lookup[loanId!] = disb;
                    }
                    if(schedule is not null)
                    {
                        disb.Schedules!.Add(schedule);
                    }   
                   
                    return disb;
                },
                param: new { BranchId = branchId, LoanId = loanId },
                splitOn: "InstallmentNo", 
                commandType: CommandType.StoredProcedure
            );

            logger.LogInformation("Fetched {Count} loan schedules for BranchId: {BranchId}, LoanId: {LoanId}", lookup.Count, branchId, loanId);

            return lookup.Values.FirstOrDefault();
        }





    
    }
}
