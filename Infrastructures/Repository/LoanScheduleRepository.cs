using Applications.Features.Loans.Responses;
using Applications.Interfaces;
using Applications.Responses;
using Dapper;
using Infrastructures.Constants;
using Infrastructures.DataAccess;
using System.Data;

namespace Infrastructures.Repository
{
    public sealed class LoanScheduleRepository : ILoanScheduleRepo
    {
        private readonly IDbConnection db;
        public LoanScheduleRepository(IDapperConnectionFactory dapperConnectionFactory)
        {
            db = dapperConnectionFactory.CreateConnection();
        }

        public async Task<Result<IEnumerable<LoanScheduleResponse>>> GetLoanDisbursments(int? branchId, string? loanId, CancellationToken cancellation = default)
        {

            var parameters = new { branchId, loanId };

            var queries = await db.QueryMultipleAsync(SqlConstant.GetLoanResponse, parameters);

            var loanQuery = await queries.ReadFirstOrDefaultAsync<LoanScheduleResponse>();

            if (loanQuery is null)
            {
                return Result.Failure<IEnumerable<LoanScheduleResponse>>(ErrorResponse.NotFound("No Loan Found."));
            }

            var scheduleQueries = (await queries.ReadAsync<ScheduleResponse>()).ToList();


            var groupedSchedules = scheduleQueries
                                            .GroupBy(x => x.ScheduleNum)
                                            .OrderBy(g => g.Key);

            var loanSchedules = new List<LoanScheduleResponse>();



            foreach (var scheduleGroup in groupedSchedules)
            {
                var response = DeepCopyWithoutSchedule(loanQuery);

                var schedules = scheduleGroup.ToArray();

                var first = schedules.FirstOrDefault();

                response.OutStanding = first?.Principle + first?.LoanOutstanding;

                int scheduleRowNum = 1;

                foreach (var schedule in schedules)
                {
                    schedule.RowNum = scheduleRowNum++;
                }


                response.Schedules.AddRange(schedules);

                loanSchedules.Add(response);
            }

            return Result.Success<IEnumerable<LoanScheduleResponse>>(loanSchedules);

        }



        public async Task<Result<IEnumerable<LoanScheduleResponse>>> GetLoanDisbursments2(int? branchId, string? loanId, CancellationToken cancellation = default)
        {

            var parameters = new { branchId, loanId };

            var multiResult = await db.QueryMultipleAsync(SqlConstant.GetLoanResponse, parameters);

            var loanQuery = await multiResult.ReadFirstOrDefaultAsync<LoanScheduleResponse>();

            if (loanQuery is null)
            {
                return Result.Failure<IEnumerable<LoanScheduleResponse>>(ErrorResponse.NotFound("No Loan Found."));
            }

            var scheduleQueries = (await multiResult.ReadAsync<ScheduleResponse>()).ToList();

            var grouped = new Dictionary<int, List<ScheduleResponse>>();

            foreach (var schedule in scheduleQueries)
            {
                int key = schedule.ScheduleNum ?? 1;

                if (!grouped.ContainsKey(key)) grouped[key] = new List<ScheduleResponse>();

                grouped[key].Add(schedule);
            }

            var loanSchedules = new List<LoanScheduleResponse>();

            int groupNum = 1;

            foreach (var group in grouped.OrderBy(g => g.Key))
            {
                var response = DeepCopyWithoutSchedule(loanQuery);

                decimal? firstPrinciple = null;

                decimal? firstOutstanding = null;

                int? scheduleNum = 1;

                for (int i = 0; i < group.Value.Count; i++)
                {
                    var schedule = group.Value[i];

                    schedule.RowNum = i + 1;

                    if (i == 0)
                    {
                        firstPrinciple = schedule.Principle;

                        firstOutstanding = schedule.LoanOutstanding;

                        scheduleNum = schedule.ScheduleNum;
                    }

                    response.Schedules.Add(schedule);
                }

                response.OutStanding = firstPrinciple + firstOutstanding;

                response.ScheduleNum = scheduleNum;

                loanSchedules.Add(response);

                groupNum++;
            }

            return Result.Success<IEnumerable<LoanScheduleResponse>>(loanSchedules);
        }


        public async Task<Result<LoanScheduleResponse>> CreatdPdf(int? branchId, string? loanId, int? scheduleNum, CancellationToken cancellation = default)
        {

            var parameters = new { branchId, loanId, scheduleNum };

            var multiResult = await db.QueryMultipleAsync(SqlConstant.GetLoanResponseByScheduleNum, parameters);

            var loanInfo = await multiResult.ReadFirstOrDefaultAsync<LoanScheduleResponse>();

            if (loanInfo is null)
            {
                return Result.Failure<LoanScheduleResponse>(ErrorResponse.NotFound("No Loan Found."));
            }

            var schedules = (await multiResult.ReadAsync<ScheduleResponse>()).ToList();

            if (!schedules.Any())
            {
                return Result.Failure<LoanScheduleResponse>(ErrorResponse.NotFound("No Schedules Found."));
            }

            // Reindex ScheduleNum starting from 1
            for (int i = 0; i < schedules.Count; i++)
            {
                schedules[i].ScheduleNum = i + 1;
            }

            var firstSchedule = schedules.First();

            var response = DeepCopyWithoutSchedule(loanInfo);

            response.Schedules.AddRange(schedules);

            response.OutStanding = (firstSchedule.Principle ?? 0) + (firstSchedule.LoanOutstanding ?? 0);

            response.ScheduleNum = firstSchedule.ScheduleNum;

            return Result.Success(response);
        }


        private static LoanScheduleResponse DeepCopyWithoutSchedule(LoanScheduleResponse source)
        {
            return new LoanScheduleResponse
            {
                CurrencyId = source.CurrencyId,
                ContractNumber = source.ContractNumber,
                BranchName = source.BranchName,
                AltName = source.AltName,
                Phone = source.Phone,
                Salutation = source.Salutation,
                RegisterDate = source.RegisterDate,
                CreditAgentKhmer = source.CreditAgentKhmer,
                AgentPhone = source.AgentPhone,
                FirstPaymentDate = source.FirstPaymentDate,
                Principle = source.Principle,
                InterestRate = source.InterestRate,
                OutStanding = source.OutStanding,
                LastPaymentDate = source.LastPaymentDate,
                Address = source.Address,
                Circle = source.Circle,
                PeriodName = source.PeriodName,
                Schedules = []
            };
        }

    }
}
