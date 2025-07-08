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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Infrastructures.Implements
{
    public sealed class LoanReportService(ILogger<LoanService> logger, IDapperConnectionFactory connectionFactory, IDbContextFactory<DataContext> contextFactory) : ILoanReportService
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
                    if (schedule is not null)
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




        public async Task<VLoanDisbursments?> PrintLoanScheduleAsync(int? branchId, string? loanId)
        {

            ArgumentNullException.ThrowIfNull(branchId, nameof(branchId));

            ArgumentNullException.ThrowIfNull(loanId, nameof(loanId));

            logger.LogInformation("Fetching loan schedule for BranchId: {BranchId}, LoanId: {LoanId}", branchId, loanId);

            var sql = @"WITH FilteredBalance AS (
                    SELECT 
                        s.LoanId,
                        s.BranchId,
                        s.OutStanding
                    FROM VLoanDisbursments s
                    WHERE 
                        s.LoanId=@LoanId AND 
                        s.BranchId=@BranchId
                ),
                TopSchedules AS (
                    SELECT 
                        s.*,
                        ROW_NUMBER() OVER (
                            PARTITION BY s.LoanId, s.BranchId 
                            ORDER BY s.PaymentDate ASC
                        ) AS RowNum
                    FROM VSchedules s
                    WHERE 
                        s.LoanId = @LoanId AND
                        s.BranchId = @BranchId AND
                        s.ScheduleStatusId IN (2, 4)
                )
                SELECT 
                    d.LoanId,
                    d.BranchId,
                    d.BranchName,
                    d.AltName,
                    d.Phone,
                    d.ContractNumber,
                    d.RegisterDate,
                    d.CurrencyId,  
                    d.PaidPrinciple,
                    fb.Outstanding Principle,
                    d.InterestRate,
                    d.PeriodName,
                    d.LastPaymentDate,
                    d.AgentName,
                    d.AgentPhone,
                    d.Circle,
                    d.Address, 
                        ts.LoanScheduleId,
                        ts.InstallmentNo,
                        ts.PaymentDate,
                        ts.TotalDay,    
                        ts.CurrencyId,
                        ts.Principle,
                        ts.Interest,
                        ts.CollectionFee,
                        ts.TotalPay,
                        ts.LoanOutstanding,
                        ts.ScheduleStatusId
                FROM VLoanDisbursments d
                LEFT JOIN FilteredBalance fb
                    ON d.LoanId = fb.LoanId AND d.BranchId = fb.BranchId
                LEFT JOIN TopSchedules ts
                    ON d.LoanId = ts.LoanId AND d.BranchId = ts.BranchId AND ts.RowNum <= 4
                WHERE 
                    d.LoanId = @LoanId AND 
                    d.BranchId = @BranchId
                ORDER BY ts.PaymentDate ASC;";


            using var db = connectionFactory.CreateConnection();

            var lookup = new Dictionary<string, VLoanDisbursments>();

            var result = await db.QueryAsync<VLoanDisbursments, VSchedule, VLoanDisbursments>(
                sql: sql,
                map: (disbursement, schedule) =>
                {
                    if (!lookup.TryGetValue(loanId, out var disb))
                    {
                        disb = disbursement;

                        disb.Schedules = new List<VSchedule>();

                        lookup[loanId!] = disb;
                    }
                    if (schedule is not null && schedule.LoanOutstanding >= 0)
                    {
                        disb.Schedules.Add(schedule);
                    }

                    return disb;
                },
                param: new { BranchId = branchId, LoanId = loanId },
                splitOn: "LoanScheduleId",
                commandType: CommandType.Text
            );

            logger.LogInformation("Fetched {Count} loan schedules for BranchId: {BranchId}, LoanId: {LoanId}", lookup.Count, branchId, loanId);

            return lookup.Values.FirstOrDefault();
        }



        public async Task<VLoanDisbursments?> GetLoanWithTopSchedulesAsync(string? loanId, int? branchId,CancellationToken cancellation=default)
        {

            await using var _context = await contextFactory.CreateDbContextAsync();

            // Get schedules that match filter
            var matchingSchedules = await _context.VSchedules
                .Where(s =>
                    s.LoanId == loanId &&
                    s.BranchId == branchId &&
                    (s.ScheduleStatusId == (int)ScheduleStatus.Unpaid || 
                     s.ScheduleStatusId == (int)ScheduleStatus.Partial))
                .OrderBy(s => s.PaymentDate)
                .ToListAsync();

            // Get balance from first matching schedule
            var balance = matchingSchedules.FirstOrDefault()?.LoanOutstanding;

            // Get top 4 schedules
            var topSchedules = matchingSchedules
                .Take(4)
                .ToList();

            // Get loan info
            var loan = await _context.VLoanDisbursements
                .Where(d => d.LoanId == loanId && d.BranchId == branchId)
                .Select(d => new VLoanDisbursments
                {
                    LoanId = d.LoanId,
                    BranchId = d.BranchId,
                    AltName = d.AltName,
                    Phone = d.Phone,
                    ContractNumber = d.ContractNumber,
                    RegisterDate = d.RegisterDate,
                    Principle = d.Principle,
                    PaidPrinciple = d.PaidPrinciple,
                    OutStanding = balance,
                    InterestRate = d.InterestRate,
                    PeriodName = d.PeriodName,
                    LastPaymentDate = d.LastPaymentDate,
                    AgentName = d.AgentName,
                    AgentPhone = d.AgentPhone,
                    Circle = d.Circle,
                    Schedules = topSchedules
                })
                .FirstOrDefaultAsync();

            return loan;
        }


    }
}
