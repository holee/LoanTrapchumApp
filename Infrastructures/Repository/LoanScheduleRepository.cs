using Applications.Features.Loans.Responses;
using Applications.Interfaces;
using Dapper;
using ErrorOr;
using Infrastructures.Constants;
using Infrastructures.DataAccess;
using System.Data;

namespace Infrastructures.Repository
{
    public sealed class LoanScheduleRepository : IScheduleService
    {
        private readonly IDbConnection db;
        public LoanScheduleRepository(IDapperConnectionFactory dapperConnectionFactory)
        {
            db = dapperConnectionFactory.CreateConnection();
        }

        public async Task<ErrorOr<IEnumerable<LoanResponse>>> GetLoanDisbursments(int? branchId, string? loanId, CancellationToken cancellation = default)
        {

            var parameters = new { branchId, loanId };

            var queries = await db.QueryMultipleAsync(SqlConstant.GetLoanResponse, parameters);

            var loanQuery = await queries.ReadFirstOrDefaultAsync<LoanResponse>();

            if (loanQuery is null)
            {
                return Error.NotFound("No Loan Found.");
            }

            var scheduleQueries = (await queries.ReadAsync<ScheduleResponse>()).ToList();


            var groupedSchedules = scheduleQueries
                                            .GroupBy(x => x.ScheduleNum)
                                            .OrderBy(g => g.Key);

            var loanSchedules = new List<LoanResponse>();



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

            return loanSchedules;

        }


        public async Task<ErrorOr<IEnumerable<LoanResponse>>> GetLoanWithSchedules2(int? branchId, string? loanId, CancellationToken cancellation = default)
        {

            var parameters = new { branchId, loanId };

            var loanDictionary = new Dictionary<string, LoanResponse>();

            var result = await db.QueryAsync<LoanResponse, ScheduleResponse, LoanResponse>(
                "spGetLoanWithScedules",
                (loan, schedule) =>
                {
                    if (!loanDictionary.TryGetValue(loan.LoanId!, out var currentLoan))
                    {
                        currentLoan = loan;

                        currentLoan.Schedules = [];

                        loanDictionary.Add(currentLoan.LoanId!, currentLoan);

                    }

                    if (schedule != null)
                    {
                        currentLoan.Schedules.Add(schedule);
                    }

                    return currentLoan;
                },
                param: parameters,
                splitOn: "LoanScheduleId",
                commandType: CommandType.StoredProcedure
            );

            var loanQuery = loanDictionary.Values.FirstOrDefault();

            if (loanQuery is null)
            {
                return ErrorOr.Error.NotFound("No Loan Found.");
            }

            var scheduleQueries = loanQuery.Schedules;

            var groups = scheduleQueries.Chunk(4)
                                .Select((chunk, index) => new
                                {
                                    Index = index,
                                    Group = BuildGroup(chunk)
                                })
                                .ToDictionary(x => x.Index, x => x.Group);

            var loanSchedules = new List<LoanResponse>();

            int groupNum = 1;

            foreach (var group in groups.OrderBy(g => g.Key))
            {
                var response = DeepCopyWithoutSchedule(loanQuery);

                int? scheduleNum = 1;

                for (int i = 0; i < group.Value.Count; i++)
                {
                    var schedule = group.Value[i];

                    schedule.RowNum = i + 1;

                    response.Schedules.Add(schedule);
                }
                response.ScheduleNum = scheduleNum;

                loanSchedules.Add(response);

                groupNum++;
            }

            return loanSchedules;
        }


        public async Task<ErrorOr<IEnumerable<LoanResponse>>> GetLoanWithSchedules(int? branchId, string? loanId, CancellationToken cancellation = default)
        {
            var parameters = new { branchId, loanId };

            using var multiResult = await db.QueryMultipleAsync(SqlConstant.GetLoanResponse, parameters);

            var loan = await multiResult.ReadFirstOrDefaultAsync<LoanResponse>();

            if (loan is null)
            {
                return Error.NotFound("No Loan Found.");
            }

            var schedules = (await multiResult.ReadAsync<ScheduleResponse>())
                            .OrderBy(s => s.InstallmentNo) // ensure correct order first
                            .ToList();

            var loanSchedules = new List<LoanResponse>();

            int installmentCounter = 1;

            int scheduleNumCounter = 0;

            foreach (var chunk in schedules.Chunk(4))
            {
                scheduleNumCounter++;

                var response = DeepCopyWithoutSchedule(loan);

                var groupedSchedules = BuildGroup(chunk);

                for (int i = 0; i < groupedSchedules.Count; i++)
                {
                    groupedSchedules[i].RowNum = i + 1;

                    groupedSchedules[i].InstallmentNo = installmentCounter++;
                }

                response.Schedules.AddRange(groupedSchedules);

                response.ScheduleNum = scheduleNumCounter;

                loanSchedules.Add(response);
            }

            return loanSchedules;
        }



        public async Task<ErrorOr<LoanResponse>> GetLoanWithSchedules(int? branchId, string? loanId, int? scheduleNum, CancellationToken cancellation = default)
        {
            var parameters = new { branchId, loanId, scheduleNum };

            using var multiResult = await db.QueryMultipleAsync(SqlConstant.GetLoanResponse, parameters);

            var loan = await multiResult.ReadFirstOrDefaultAsync<LoanResponse>();

            if (loan is null)
            {
                return Error.NotFound("No Loan Found.");
            }

            var schedules = (await multiResult.ReadAsync<ScheduleResponse>())
                            .OrderBy(s => s.InstallmentNo) // ensure correct order first
                            .ToList();

            var loanSchedules = new List<LoanResponse>();

            int installmentCounter = 1;

            int scheduleNumCounter = 0;

            foreach (var chunk in schedules.Chunk(4))
            {
                scheduleNumCounter++;

                var response = DeepCopyWithoutSchedule(loan);

                var groupedSchedules = BuildGroup(chunk);

                for (int i = 0; i < groupedSchedules.Count; i++)
                {
                    groupedSchedules[i].RowNum = i + 1;

                    groupedSchedules[i].InstallmentNo = installmentCounter++;
                }

                response.Schedules.AddRange(groupedSchedules);

                response.ScheduleNum = scheduleNumCounter;

                loanSchedules.Add(response);
            }

            var selectedLoan = loanSchedules
                                    .FirstOrDefault(ls => ls.ScheduleNum == scheduleNum);

            if ((selectedLoan is null))
            {
                return Error.NotFound("No Loan Found.");
            }

            return selectedLoan;
        }






        public async Task<ErrorOr<LoanResponse>> CreatePdf(int? branchId, string? loanId, CancellationToken cancellation = default)
        {
            var parameters = new { branchId, loanId };

            var loanDictionary = new Dictionary<string, LoanResponse>();

            var result = await db.QueryAsync<LoanResponse, ScheduleResponse, LoanResponse>(
                "spGetLoanWithScedules",
                (loan, schedule) =>
                {
                    if (!loanDictionary.TryGetValue(loan.LoanId!, out var currentLoan))
                    {
                        currentLoan = loan;

                        currentLoan.Schedules = [];

                        loanDictionary.Add(currentLoan.LoanId!, currentLoan);

                    }

                    if (schedule != null)
                    {
                        currentLoan.Schedules.Add(schedule);
                    }

                    return currentLoan;
                },
                param: parameters,
                splitOn: "LoanScheduleId",
                commandType: CommandType.StoredProcedure
            );

            var loanQuery = loanDictionary.Values.FirstOrDefault();

            if (loanQuery is not null)
                return loanQuery;

            return Error.NotFound("No Schedule Found.");

        }


        public async Task<ErrorOr<LoanResponse>> CreatdPdf(int? branchId, string? loanId, int? scheduleNum, CancellationToken cancellation = default)
        {

            var parameters = new { branchId, loanId, scheduleNum };

            var multiResult = await db.QueryMultipleAsync(SqlConstant.GetLoanResponseByScheduleNum, parameters);

            var loanInfo = await multiResult.ReadFirstOrDefaultAsync<LoanResponse>();

            if (loanInfo is null)
            {
                return Error.NotFound("No Loan Found.");
            }

            var schedules = (await multiResult.ReadAsync<ScheduleResponse>()).ToList();

            if (!schedules.Any())
            {
                return Error.NotFound("No Schedules Found.");
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

            return response;
        }


        private static LoanResponse DeepCopyWithoutSchedule(LoanResponse source)
        {
            return new LoanResponse
            {
                CurrencyId = source.CurrencyId,
                ContractNumber = source.ContractNumber,
                BranchName = source.BranchName,
                LoanAccount = source.LoanAccount,
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
                ScheduleNum = source.ScheduleNum,
                Schedules = []
            };
        }


        private static List<ScheduleResponse> BuildGroup(IEnumerable<ScheduleResponse> source)
        {

            if (!source.Any()) return [];

            var group = source.ToList();

            var first = group.First();

            var last = group.Last();

            var opening = CreateOpeningRow(first);

            var closing = CreateClosingRow(last);

            group.Add(opening);

            group.Add(closing);

            return group
                .OrderBy(x => x.InstallmentNo)
                .ToList();
        }

        private static ScheduleResponse CreateOpeningRow(ScheduleResponse first)
        {
            return new ScheduleResponse
            {
                InstallmentNo = 0,
                PaymentDate = null,
                TotalDay = null,
                Principle = null,
                Interest = null,
                CollectionFee = null,
                TotalPay = null,
                LoanOutstanding = first.LoanOutstanding + first.Principle
            };
        }

        private static ScheduleResponse CreateClosingRow(ScheduleResponse last)
        {
            return new ScheduleResponse
            {
                InstallmentNo = last.InstallmentNo + 1,
                PaymentDate = last.PaymentDate,
                TotalDay = null,
                Principle = last.LoanOutstanding,
                Interest = 0,
                TotalFee = 0,
                CollectionFee = null,
                TotalPay = last.LoanOutstanding,
                LoanOutstanding = 0
            };
        }

    }
}



//public async Task<ErrorOr.ErrorOr<IEnumerable<LoanResponse>>> GetLoanWithSchedules2(int? branchId, string? loanId, CancellationToken cancellation = default)
//{

//    var parameters = new { branchId, loanId };

//    var loanDictionary = new Dictionary<string, LoanResponse>();

//    var result = await db.QueryAsync<LoanResponse, ScheduleResponse, LoanResponse>(
//        "spGetLoanWithScedules",
//        (loan, schedule) =>
//        {
//            if (!loanDictionary.TryGetValue(loan.LoanId, out var currentLoan))
//            {
//                currentLoan = loan;

//                currentLoan.Schedules = [];

//                loanDictionary.Add(currentLoan.LoanId, currentLoan);

//            }

//            if (schedule != null)
//            {
//                currentLoan.Schedules.Add(schedule);
//            }

//            return currentLoan;
//        },
//        param: parameters,
//        splitOn: "LoanScheduleId",
//        commandType: CommandType.StoredProcedure
//    );

//    var loanQuery = loanDictionary.Values.FirstOrDefault();

//    //var multiResult = await db.QueryMultipleAsync(SqlConstant.GetLoanResponse, parameters);

//    //var loanQuery = await multiResult.ReadFirstOrDefaultAsync<LoanResponse>();

//    if (loanQuery is null)
//    {
//        return ErrorOr.Error.NotFound("No Loan Found.");
//    }

//    var scheduleQueries = loanQuery.Schedules;    //(await multiResult.ReadAsync<ScheduleResponse>()).ToList();

//    var groups = scheduleQueries.Chunk(4)
//                        .Select((chunk, index) => new
//                        {
//                            Index = index,
//                            Group = BuildGroup(chunk)
//                        })
//                        .ToDictionary(x => x.Index, x => x.Group);

//    var loanSchedules = new List<LoanResponse>();

//    int groupNum = 1;

//    foreach (var group in groups.OrderBy(g => g.Key))
//    {
//        var response = DeepCopyWithoutSchedule(loanQuery);

//        int? scheduleNum = 1;

//        for (int i = 0; i < group.Value.Count; i++)
//        {
//            var schedule = group.Value[i];

//            schedule.RowNum = i + 1;

//            response.Schedules.Add(schedule);
//        }
//        response.ScheduleNum = scheduleNum;

//        loanSchedules.Add(response);

//        groupNum++;
//    }

//    return loanSchedules;
//}




//public async Task<Result<IEnumerable<LoanScheduleResponse>>> GetLoanWithSchedules(int? branchId, string? loanId, CancellationToken cancellation = default)
//{

//    var parameters = new { branchId, loanId };

//    var multiResult = await db.QueryMultipleAsync(SqlConstant.GetLoanResponse, parameters);

//    var loanQuery = await multiResult.ReadFirstOrDefaultAsync<LoanScheduleResponse>();

//    if (loanQuery is null)
//    {
//        return Result.Failure<IEnumerable<LoanScheduleResponse>>(ErrorResponse.NotFound("No Loan Found."));
//    }

//    var scheduleQueries = (await multiResult.ReadAsync<ScheduleResponse>()).ToList();

//    var groups = new Dictionary<int, List<ScheduleResponse>>();

//    foreach (var schedule in scheduleQueries)
//    {
//        int key = schedule.ScheduleNum ?? 1;

//        if (!groups.ContainsKey(key)) groups[key] = new List<ScheduleResponse>();

//        groups[key].Add(schedule);
//    }

//    var loanSchedules = new List<LoanScheduleResponse>();

//    int groupNum = 1;

//    foreach (var group in groups.OrderBy(g => g.Key))
//    {
//        var response = DeepCopyWithoutSchedule(loanQuery);

//        decimal? firstPrinciple = null;

//        decimal? firstOutstanding = null;

//        int? scheduleNum = 1;

//        for (int i = 0; i < group.Value.Count; i++)
//        {
//            var schedule = group.Value[i];

//            schedule.RowNum = i + 1;

//            if (i == 0)
//            {
//                firstPrinciple = schedule.Principle;

//                firstOutstanding = schedule.LoanOutstanding;

//                scheduleNum = schedule.ScheduleNum;
//            }

//            response.Schedules.Add(schedule);
//        }

//        response.OutStanding = firstPrinciple + firstOutstanding;

//        response.ScheduleNum = scheduleNum;

//        loanSchedules.Add(response);

//        groupNum++;
//    }

//    return Result.Success<IEnumerable<LoanScheduleResponse>>(loanSchedules);
//}
