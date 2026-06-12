using Applications.Features.LoanPayments.Requests;

namespace Applications.Calculations.Extensions;

public static class PaymentDtoCreateExtensions
{
    public static PaymentCreate ToPaymentCreate(this PaymentDtoCreate create)
    {
        return new PaymentCreate
        {
            OriginalPaidAmount = create.TotalPaymentNoPenalty,
            OriginalPenaltyAmount = create.PenaltyAmountAmount,
            TotalAmountNoPenalty = create.TotalPaymentNoPenalty,
            TotalAmountWithPenalty = create.TotalPaymentWithPenalty,
            PenaltyAmount = create.PenaltyAmountAmount,
            RecieveKhrAmount = 0,
            RecieveAmount = 0,
            ChangeKhrAmount = 0,
            ChangeAmount = 0,
        };
    }

    public static PaymentCreate ToPaymentCreate(this PaymentDtoCreate create, int currencyId)
    {
        return new PaymentCreate
        {
            OriginalPaidAmount = create.TotalPaymentNoPenalty,
            OriginalPenaltyAmount = create.PenaltyAmountAmount,
            TotalAmountNoPenalty = create.TotalPaymentNoPenalty,
            TotalAmountWithPenalty = create.TotalPaymentWithPenalty,
            PenaltyAmount = create.PenaltyAmountAmount,
            RecieveKhrAmount = currencyId == 1 ? create.TotalPaymentWithPenalty : 0.00M,
            RecieveAmount = currencyId == 2 ? create.TotalPaymentWithPenalty : 0.00M,
            ChangeKhrAmount = 0,
            ChangeAmount = 0,
            LoanRepaymentLines = []
        };
    }

    public static PaymentCreate ToPaymentCreate(this List<LoanPaymentLineDtoCreate> lines, int? currencyId)
    {
        return new PaymentCreate
        {
            OriginalPaidAmount = lines.TotalAmountNoPenalty(),
            OriginalPenaltyAmount = lines.TotalPaidPenalty(),
            TotalAmountNoPenalty = lines.TotalAmountNoPenalty(),
            TotalAmountWithPenalty = lines.TotalAmountWithPenalty(),
            PenaltyAmount = lines.TotalPaidPenalty(),
            RecieveKhrAmount = currencyId == 1 ? lines.TotalAmountWithPenalty() : 0.00M,
            RecieveAmount = currencyId == 2 ? lines.TotalAmountWithPenalty() : 0.00M,
            ChangeKhrAmount = 0,
            ChangeAmount = 0,
            LoanRepaymentLines = lines
        };


    }

}


//public static List<LoanScheduleDtoCreate> CreateShcedules(IEnumerable<ScheduleLine> schedules, long creditAgentId)
//{
//    return schedules.Select(x => new LoanScheduleDtoCreate
//    {
//        InstallmentNo = x.InstallmentNo,
//        PaymentDate = x.PaymentDate,
//        Principal = x.Principal,
//        Interest = x.Interest,
//        CollectionFee = x.CollectionFeeAmount,
//        LoanOutstanding = x.OutStanding,
//        TotalDay = x.TotalDay,
//        TotalPay = x.TotalPay,
//        AgentRefId = creditAgentId,
//        ScheduleStatusRefId = 2
//    }).ToList();
//}
//public static List<LoanScheduleDtoCreate> CreateShcedules(IEnumerable<ScheduleLine> schedules)
//{
//    return schedules.Select(x => new LoanScheduleDtoCreate
//    {
//        InstallmentNo = x.InstallmentNo,
//        PaymentDate = x.PaymentDate,
//        Principal = x.Principal,
//        Interest = x.Interest,
//        CollectionFee = x.CollectionFeeAmount,
//        LoanOutstanding = x.OutStanding,
//        TotalDay = x.TotalDay,
//        TotalPay = x.TotalPay,
//        AgentRefId = 0,
//        ScheduleStatusRefId = 2
//    }).ToList();
//}
//public static LoanCaloulator Build(int paymentMode, int payEvery, decimal effectiveRate,
//    int collectionFeeMode, int day, int currencyId, int installmentTypeId,
//    DateTime firstPayment, DateTime startDate)
//{
//    return paymentMode switch
//    {
//        1 => new Balloon(payEvery, effectiveRate, collectionFeeMode, day, currencyId, installmentTypeId, firstPayment, startDate),
//        2 => new Decline(payEvery, effectiveRate, collectionFeeMode, day, currencyId, installmentTypeId, firstPayment, startDate),
//        3 => new Flat(payEvery, effectiveRate, collectionFeeMode, day, currencyId, installmentTypeId, firstPayment, startDate),
//        4 => new Annuarity(payEvery, effectiveRate, collectionFeeMode, day, currencyId, installmentTypeId, firstPayment, startDate),
//        _ => new Flat(payEvery, effectiveRate, collectionFeeMode, day, currencyId, installmentTypeId, firstPayment, startDate),
//    };
//}

//public static LoanCaloulator Create(int? paymentMode,
//    int? payEvery,
//    decimal? effectiveRate,
//    int? collectionFeeMode,
//    int? day,
//    int? currencyId,
//    int? installmentTypeId,
//    DateTime? firstPayment,
//    DateTime? startDate)
//{

//    int _paymentMode = paymentMode ?? 1;
//    int _payEvery = payEvery ?? 1;
//    decimal _effectiveRate = effectiveRate ?? 0;
//    int _collectionFeeMode = collectionFeeMode ?? 1;
//    int _day = day ?? 28;
//    int _currencyId = currencyId ?? 1;
//    int _installmentTypeId = installmentTypeId ?? 2;
//    DateTime _firstPayment = firstPayment ?? DateTime.UtcNow;
//    DateTime _startDate = startDate ?? DateTime.UtcNow;
//    return paymentMode switch
//    {
//        1 => new Balloon(_payEvery, _effectiveRate, _collectionFeeMode, _day, _currencyId, _installmentTypeId, _firstPayment, _startDate),
//        2 => new Decline(_payEvery, _effectiveRate, _collectionFeeMode, _day, _currencyId, _installmentTypeId, _firstPayment, _startDate),
//        3 => new Flat(_payEvery, _effectiveRate, _collectionFeeMode, _day, _currencyId, _installmentTypeId, _firstPayment, _startDate),
//        4 => new Annuarity(_payEvery, _effectiveRate, _collectionFeeMode, _day, _currencyId, _installmentTypeId, _firstPayment, _startDate),
//        _ => new Flat(_payEvery, _effectiveRate, _collectionFeeMode, _day, _currencyId, _installmentTypeId, _firstPayment, _startDate),
//    };
//}



