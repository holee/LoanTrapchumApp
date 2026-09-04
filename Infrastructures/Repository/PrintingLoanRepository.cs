using Applications.Commons.ExceptionHandlers;
using Applications.Features.Reports.Dtos;
using Applications.Interfaces;
using Dapper;
using Infrastructures.DataAccess;
using System.Data;
namespace Infrastructures.Repository;

public sealed class PrintingLoanRepository(IDapperConnectionFactory connectionFactory) : IPrintingLoanService
{

    public async Task<RptScheduleDetails?> PrintScheduleDetailsAsync(string? loanId, int? branchId, CancellationToken cancellationToken = default)
    {

        var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);


        var lookup = new Dictionary<string, RptScheduleDetails>();

        await connection.QueryAsync<RptScheduleDetails, RptScheduleDetailsLine, RptScheduleDetails>("PrintLoanSchedule",
            (loan, schedule) =>
            {
                if (!lookup.TryGetValue(loan.LoanId!, out var existing))
                {
                    existing = loan;
                    existing.Schedules = new List<RptScheduleDetailsLine>();

                    lookup.Add(existing.LoanId!, existing);
                }

                if (schedule is not null)
                {
                    existing.Schedules.Add(schedule);
                }

                return existing;
            },
            new { LoanId = loanId, BranchId = branchId },
            splitOn: "LoanScheduleId",
            commandType: CommandType.StoredProcedure
        );

        return lookup.Values.FirstOrDefault();
    }

    public async Task<RptScheduleDetails?> PrintScheduleDetailsAsync(string? loanId, int? branchId, int? scheduleGroup, CancellationToken cancellationToken = default)
    {
        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        using var multi = await connection.QueryMultipleAsync(
                "PrintLoanDetailsByGroup",
                new
                {
                    LoanId = loanId,
                    BranchId = branchId,
                    ScheduleGroup = scheduleGroup
                },
                commandType: CommandType.StoredProcedure);

        var loan = await multi.ReadFirstOrDefaultAsync<RptScheduleDetails>()
                 ?? throw new NotFoundException("No loan found.");

        loan.Schedules = (await multi.ReadAsync<RptScheduleDetailsLine>())
                                        .OrderBy(x => x.InstallmentNo)
                                        .ToList();

        loan.Schedules = BuildGroup(loan.Schedules);

        for (int i = 0; i < loan.Schedules.Count; i++)
        {
            loan.Schedules[i].RowNum = i + 1;
        }

        loan.ScheduleNum = scheduleGroup;

        return loan;
    }
  
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    /// <summary>
    /// Private methods
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private static List<RptScheduleDetailsLine> BuildGroup(IEnumerable<RptScheduleDetailsLine> source)
    {

        if (!source.Any()) return [];

        var group = source.ToList();

        var first = group.First();

        var last = group.Last();

        var opening = CreateOpeningRow(first);

        var closing = CreateClosingRow(last);

        group.Add(opening);

        group.Add(closing);

        return group.OrderBy(x => x.InstallmentNo).ToList();
    }
    private static RptScheduleDetailsLine CreateOpeningRow(RptScheduleDetailsLine first)
    {
        return new RptScheduleDetailsLine
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
    private static RptScheduleDetailsLine CreateClosingRow(RptScheduleDetailsLine last)
    {
        return new RptScheduleDetailsLine
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


//private static RptScheduleDetails DeepCopyWithoutSchedule(RptScheduleDetails source)
//{
//    return new RptScheduleDetails
//    {
//        CurrencyId = source.CurrencyId,
//        ContractNumber = source.ContractNumber,
//        BranchName = source.BranchName,
//        LoanAccount = source.LoanAccount,
//        AltName = source.AltName,
//        Phone = source.Phone,
//        Salutation = source.Salutation,
//        RegisterDate = source.RegisterDate,
//        CreditAgentKhmer = source.CreditAgentKhmer,
//        AgentPhone = source.AgentPhone,
//        FirstPaymentDate = source.FirstPaymentDate,
//        Principle = source.Principle,
//        InterestRate = source.InterestRate,
//        OutStanding = source.OutStanding,
//        LastPaymentDate = source.LastPaymentDate,
//        Address = source.Address,
//        Circle = source.Circle,
//        PeriodName = source.PeriodName,
//        ScheduleNum = source.ScheduleNum,
//        Schedules = []
//    };
//}




//public async Task<RptScheduleDetails?> PrintScheduleDetailsAsync(string? loanId, int? branchId, int? scheduleGroup, CancellationToken cancellationToken = default)
//{

//    var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

//    var multiResult = await connection.QueryMultipleAsync("PrintLoanDetailsByGroup",
//                     param: new { loanId, branchId, scheduleGroup },
//                      commandType: CommandType.StoredProcedure
//     );

//    var loan = await multiResult.ReadFirstOrDefaultAsync<RptScheduleDetails>();

//    if (loan is null)
//    {
//        throw new NotFoundException("No loan found.");
//    }

//    var schedules = (await multiResult.ReadAsync<RptScheduleDetailsLine>())
//                    .OrderBy(s => s.InstallmentNo)
//                    .ToList();

//    var loanDetails = new List<RptScheduleDetails>();

//    int installmentCounter = 1;

//    int scheduleNumCounter = 0;

//    foreach (var chunk in schedules.Chunk(4))
//    {
//        scheduleNumCounter++;

//        var response = DeepCopyWithoutSchedule(loan);

//        var groupedSchedules = BuildGroup(chunk);

//        for (int i = 0; i < groupedSchedules.Count; i++)
//        {
//            groupedSchedules[i].RowNum = i + 1;

//            groupedSchedules[i].InstallmentNo = installmentCounter++;
//        }

//        response.Schedules.AddRange(groupedSchedules);

//        response.ScheduleNum = scheduleNumCounter;

//        loanDetails.Add(response);
//    }

//    var selectedLoan = loanDetails.FirstOrDefault(ls => ls.ScheduleNum == scheduleGroup);

//    if ((selectedLoan is null))
//    {
//        throw new NotFoundException("No loan found.");
//    }

//    return selectedLoan;


//}

