using Applications.Enumerations;
using Applications.Extensions.DateTimes;
using Applications.Features.Holidays.Dtos;
using Applications.Features.Loans.Dtos.Requests;
using Domains.Enumerations;
using Domains.Models.Loans;

namespace Applications.Calculations;


public sealed class LoanCalculatorBuiler
{
    public LoanCalculatorBuiler() { }
    public LoanCalculatorBuiler(int? currencyId,
           int? payEvery,
           int? repaymentModeId,
           int? collectionFeeModeId,
           int? installmentTypeId,
           AdjustmentOption scheduleMode,
           decimal? collectionFeeRate,
           DateTime? firstPayment,
           DateTime? startDate,
           IEnumerable<HolidayDto> holidays)
    {
        CurrencyId = currencyId;
        PayEvery = payEvery;
        RepaymentModeId = repaymentModeId;
        CollectionFeeModeId = collectionFeeModeId;
        InstallmentTypeId = installmentTypeId;
        ScheduleMode = scheduleMode;
        CollectionFeeRate = collectionFeeRate;
        FirstPayment = firstPayment;
        StartDate = startDate;
        Holidays = holidays;
    }

    public int? CurrencyId { get; set; } = (int)CurrenciesType.Khr;

    public int? PayEvery { get; set; } = 1;

    public int? RepaymentModeId { get; set; } = (int)RepaymentOption.Declined;

    public int? CollectionFeeModeId { get; set; } = (int)CollectionFeeOption.IRR;

    public int? InstallmentTypeId { get; set; } = (int)InstallmentOption.Monthly;

    public AdjustmentOption ScheduleMode { get; set; } = AdjustmentOption.After;

    public decimal? CollectionFeeRate { get; set; } = 0;

    public DateTime? FirstPayment { get; set; } = DateTime.Now;

    public DateTime? StartDate { get; set; } = DateTime.UtcNow;

    public IEnumerable<HolidayDto> Holidays { get; set; } = [];

    public static LoanCalculatorBuiler Create(LoanCalculatorBuiler req)
    {
        return new LoanCalculatorBuiler(
            req.CurrencyId ?? (int)CurrenciesType.Khr,
            req.PayEvery ?? 1,
            req.RepaymentModeId ?? (int)RepaymentOption.Declined,
            req.CollectionFeeModeId ?? (int)CollectionFeeOption.IRR,
            req.InstallmentTypeId ?? (int)InstallmentOption.Monthly,
            req.ScheduleMode,
            req.CollectionFeeRate ?? 0,
            req.FirstPayment ?? DateTime.UtcNow,
            req.StartDate ?? DateTime.UtcNow,
            req.Holidays ?? []
        );
    }

}


public sealed class LoanBuilder
{

    decimal? _effectiveBalance = 0;

    private LoanDtoCreate _calculateBuiler;

    public LoanBuilder(LoanDtoCreate req)
    {
        _calculateBuiler = req;
    }

    public LoanBuilder(Action<LoanDtoCreate> action)
    {
        var param = new LoanDtoCreate();

        action.Invoke(param);

        _calculateBuiler = param;
    }

    public IEnumerable<ScheduleLine> CreateSchedules(LoanDtoCreate req)
    {
        return _calculateBuiler.RepaymentModeRefId switch
        {
            (int)RepaymentOption.Balloon => Balloon(req),

            (int)RepaymentOption.Declined => Decline(req),

            (int)RepaymentOption.IRR => Flat(req),

            (int)RepaymentOption.Annurity => Annurity(req),

            _ => Flat(req)
        };
    }

    public IEnumerable<ScheduleLine> CreateSchedules(LoanDtoCreate req, LoanRepaymentModes mode)
    {
        return mode switch
        {
            LoanRepaymentModes.Balloon => Balloon(req),

            LoanRepaymentModes.Decline => Decline(req),

            LoanRepaymentModes.IRR => Flat(req),

            LoanRepaymentModes.Annuity => Annurity(req),

            _ => Flat(req)
        };
    }




    private void SetHoliday(ref DateTime? paymentDate)
    {
        if (paymentDate == null) return; // No date to process

        paymentDate = FindValidDate(paymentDate);
    }

    private DateTimeOffset FindValidDate(DateTimeOffset date)
    {
        // Check if date is a Sunday or holiday
        bool isInvalid = IsSunday(date) || IsHoliday(date);

        // For Skip mode, return the original date regardless of validity
        if (_calculateBuiler.ScheduleMode == AdjustmentOption.Skip)
        {
            return date;
        }

        // Base case: Date is valid (not a Sunday or holiday)
        if (!isInvalid)
        {
            return date;
        }

        // Recursive case: Adjust date based on schedule mode
        DateTimeOffset newDate = _calculateBuiler.ScheduleMode switch
        {
            AdjustmentOption.Before => date.AddDays(-1),

            AdjustmentOption.After => date.AddDays(1),

            _ => throw new InvalidOperationException("Invalid schedule mode")
        };

        // Recursively check the new date
        return FindValidDate(newDate);
    }


    private DateTimeOffset? FindValidDate(DateTimeOffset? date)
    {
        // Check if date is a Sunday or holiday
        bool isInvalid = IsSunday(date) || IsHoliday(date);

        // For Skip mode, return the original date regardless of validity
        if (_calculateBuiler.ScheduleMode == AdjustmentOption.Skip)
        {
            return date;
        }

        // Base case: Date is valid (not a Sunday or holiday)
        if (!isInvalid)
        {
            return date;
        }

        // Recursive case: Adjust date based on schedule mode
        DateTimeOffset? newDate = _calculateBuiler.ScheduleMode switch
        {
            AdjustmentOption.Before => date!.Value.AddDays(-1),

            AdjustmentOption.After => date!.Value.AddDays(1),

            _ => throw new InvalidOperationException("Invalid schedule mode")
        };

        // Recursively check the new date
        return FindValidDate(newDate);
    }



    private DateTime? FindValidDate(DateTime? date)
    {
        // Check if date is a Sunday or holiday
        bool isInvalid = IsSunday(date) || IsHoliday(date);

        // For Skip mode, return the original date regardless of validity
        if (_calculateBuiler.ScheduleMode == AdjustmentOption.Skip)
        {
            return date;
        }

        // Base case: Date is valid (not a Sunday or holiday)
        if (!isInvalid)
        {
            return date;
        }

        // Recursive case: Adjust date based on schedule mode
        DateTime? newDate = _calculateBuiler.ScheduleMode switch
        {
            AdjustmentOption.Before => date!.Value.AddDays(-1),

            AdjustmentOption.After => date!.Value.AddDays(1),

            _ => throw new InvalidOperationException("Invalid schedule mode")
        };

        // Recursively check the new date
        return FindValidDate(newDate);
    }


    private bool IsSunday(DateTimeOffset date)
    {
        return date.DayOfWeek == DayOfWeek.Sunday;
    }

    private bool IsHoliday(DateTimeOffset date)
    {
        foreach (var holiday in _calculateBuiler.Holidays)
        {
            if (holiday.HolidayDt.HasValue &&
                holiday.HolidayDt.Value.Date == date.DateTime.Date &&
                holiday.Status == true) // Only active holidays
            {
                return true;
            }
        }
        return false;
    }


    private IEnumerable<ScheduleLine> Balloon(CalculatorBase req)
    {
        var schedules = new List<ScheduleLine>();

        decimal _monthlyRate = req.InterestRate ?? 0.00M;

        decimal _amount = req.Principle ?? 0.00M;

        int? _periodInMonth = req.Installments ?? 0;

        int? circles = _periodInMonth / _calculateBuiler.PayEvery; //_payEvery;

        var beginBalance = 0.00M;

        var lastBalance = 0.00M;

        var paymentDate = _calculateBuiler.FirstPaymentDate; //_firstPayment;

        beginBalance = lastBalance = _amount;

        _effectiveBalance = req.Principle ?? 0.00M;

        int scheduleNum = 1;

        for (int i = 1; i <= _periodInMonth; i++)
        {
            beginBalance = lastBalance;

            var interestAmount = 0.00M;

            var totalPayAmount = 0.00M;

            var effAmount = 0.00M;

            var totalDays = 0;

            var principal = CalculatorExtensions.PaidPrincipal(_calculateBuiler.CurrencyRefId, _amount, circles);

            DateTime? previusDate;

            if (i == 1)
            {
                previusDate = _calculateBuiler.StartDate;//_startDate;

                totalDays = CalculatorExtensions.TotalDays(previusDate, _calculateBuiler.FirstPaymentDate);
            }
            if (i > 1)
            {
                previusDate = schedules[i - 2].PaymentDate;

                paymentDate = SetPaymentDate(previusDate);

                SetHoliday(ref paymentDate);

                totalDays = CalculatorExtensions.TotalDays(previusDate, paymentDate);
            }

            if (i % _calculateBuiler.PayEvery == 0)
            {
                if (_calculateBuiler.CollectionFeeModeId == (int)EffectiveEnum.IRR)
                {
                    effAmount = CalculatorExtensions.CollectionFee(_calculateBuiler.CurrencyRefId, _amount, _calculateBuiler.CollectionFeeRate, totalDays);
                }
                else
                {
                    effAmount = CalculatorExtensions.CollectionFee(_calculateBuiler.CurrencyRefId, beginBalance, _calculateBuiler.CollectionFeeRate, totalDays);
                }

                interestAmount = CalculatorExtensions.Interest(_calculateBuiler.CurrencyRefId, beginBalance, _monthlyRate, totalDays);

                totalPayAmount = CalculatorExtensions.TotalPay(principal, interestAmount, effAmount);

                if (i < _periodInMonth)
                {
                    lastBalance = CalculatorExtensions.LastBalance(beginBalance, principal);
                }
                else
                {
                    principal = beginBalance;
                    lastBalance = 0;
                }

            }
            if (i % _calculateBuiler.PayEvery != 0)
            {
                principal = 0;

                if (_calculateBuiler.CollectionFeeModeId == (int)EffectiveEnum.IRR)
                {
                    if (i % 5 == 0)
                    {
                        _effectiveBalance = lastBalance;
                    }

                    effAmount = CalculatorExtensions.CollectionFee(_calculateBuiler.CurrencyRefId, _effectiveBalance, _calculateBuiler.CollectionFeeRate, totalDays);
                }
                else
                {
                    effAmount = CalculatorExtensions.CollectionFee(_calculateBuiler.CurrencyRefId, beginBalance, _calculateBuiler.CollectionFeeRate, totalDays);
                }

                interestAmount = CalculatorExtensions.Interest(_calculateBuiler.CurrencyRefId, beginBalance, _monthlyRate, totalDays);

                totalPayAmount = CalculatorExtensions.TotalPay(principal, interestAmount, effAmount);
            }


            if (i % 5 == 0)
            {
                scheduleNum = scheduleNum + 1;
            }

            ScheduleLine line = new ScheduleLine()
            {
                InstallmentNo = i,
                TotalDay = totalDays,
                PaymentDate = paymentDate,
                Principal = principal,
                Interest = interestAmount,
                CollectionFeeAmount = effAmount,
                TotalPay = totalPayAmount,
                OutStanding = lastBalance,
                BranchId = req.BranchId,
                ScheduleNum = scheduleNum,
                CreditAgentRefId = req.CreditAgentRefId,
                RepaymentModeRefId = req.RepaymentModeRefId,
            };

            schedules.Add(line);
        }

        return schedules;
    }
    private IEnumerable<ScheduleLine> Flat(CalculatorBase req)
    {
        List<ScheduleLine> scheduleLines = [];

        var amount = req.Principle ?? 0;

        var periodInMonth = req.Installments ?? 12;

        var monthlyRate = req.InterestRate ?? 0.00M;

        var principalPaid = CalculatorExtensions.PaidPrincipal(_calculateBuiler.CurrencyRefId, amount, periodInMonth);

        decimal beginBalance = 0, lastBalance = 0;

        DateTime? paymentDate = _calculateBuiler.FirstPaymentDate; //_firstPayment;

        beginBalance = lastBalance = amount;

        var effAmount = 0M;

        DateTime? previusDate;

        _effectiveBalance = req.Principle ?? 0.00M;

        int scheduleNum = 1;


        for (int i = 1; i <= periodInMonth; i++)
        {
            beginBalance = lastBalance;

            var totalDays = 0;

            var totalPay = 0.00M;

            var interest = 0.00M;

            if (i < periodInMonth)
            {
                lastBalance = CalculatorExtensions.LastBalance(beginBalance, principalPaid);
            }
            if (i >= periodInMonth)
            {
                principalPaid = beginBalance;

                lastBalance = 0;
            }
            if (i == 1)
            {
                previusDate = _calculateBuiler.StartDate;// _startDate;

                totalDays = CalculatorExtensions.TotalDays(previusDate, _calculateBuiler.FirstPaymentDate);
            }
            if (i > 1)
            {
                previusDate = scheduleLines[i - 2].PaymentDate!.Value;

                paymentDate = SetPaymentDate(previusDate);

                SetHoliday(ref paymentDate);

                totalDays = CalculatorExtensions.TotalDays(previusDate, paymentDate);
            }
            if (_calculateBuiler.CollectionFeeModeId == (int)EffectiveEnum.IRR)
            {

                if (i % 5 == 0)
                {
                    _effectiveBalance = lastBalance;
                }

                effAmount = CalculatorExtensions.CollectionFee(_calculateBuiler.CurrencyRefId, _effectiveBalance, _calculateBuiler.CollectionFeeRate, totalDays);
            }
            if (_calculateBuiler.CollectionFeeModeId == (int)EffectiveEnum.Declined)
            {
                effAmount = CalculatorExtensions.CollectionFee(_calculateBuiler.CurrencyRefId, beginBalance, _calculateBuiler.CollectionFeeRate, totalDays);
            }

            interest = CalculatorExtensions.Interest(_calculateBuiler.CurrencyRefId, amount, monthlyRate, totalDays);

            totalPay = CalculatorExtensions.TotalPay(principalPaid, interest, effAmount);


            if (i % 5 == 0)
            {
                scheduleNum = scheduleNum + 1;
            }


            ScheduleLine line = new ScheduleLine()
            {
                InstallmentNo = i,
                PaymentDate = paymentDate,
                TotalDay = totalDays,
                Principal = principalPaid,
                Interest = interest,
                TotalPay = totalPay,
                OutStanding = lastBalance,
                CollectionFeeAmount = effAmount,
                BranchId = req.BranchId,
                ScheduleNum = scheduleNum,
                CreditAgentRefId = req.CreditAgentRefId,
                RepaymentModeRefId = req.RepaymentModeRefId,
            };
            scheduleLines.Add(line);
        }

        return scheduleLines;
    }
    private IEnumerable<ScheduleLine> Annurity(CalculatorBase req)
    {
        List<ScheduleLine> loanScheduleLines = [];

        var principleAndInterestAmount = CalculatorExtensions.CalcPmt(_calculateBuiler.CurrencyRefId, req.Installments, req.InterestRate, -req.Principle);

        decimal beginBalance = 0, lastBalance = 0;

        DateTime? paymentDate = _calculateBuiler.FirstPaymentDate; //_firstPayment;

        beginBalance = lastBalance = req.Principle!.Value;

        _effectiveBalance = req.Principle ?? 0.00M;

        var _collectionFeeAmount = 0M;

        int scheduleNum = 1;

        for (int i = 1; i <= req.Installments; i++)
        {
            var totalPay = 0.00M;

            var interest = 0.00M;

            var totalDays = 0;

            var principal = 0.00M;

            beginBalance = lastBalance;

            DateTime? previusDate;

            if (i == 1)
            {

                previusDate = _calculateBuiler.StartDate;//._startDate;

                totalDays = CalculatorExtensions.TotalDays(previusDate, _calculateBuiler.FirstPaymentDate);
            }
            if (i > 1)
            {
                previusDate = loanScheduleLines[i - 2].PaymentDate!.Value;

                paymentDate = SetPaymentDate(previusDate);

                SetHoliday(ref paymentDate);

                totalDays = CalculatorExtensions.TotalDays(previusDate, paymentDate);

            }
            if (_calculateBuiler.CollectionFeeModeId == (int)EffectiveEnum.IRR)
            {

                if (i % 5 == 0)
                {
                    _effectiveBalance = lastBalance;
                }

                _collectionFeeAmount = CalculatorExtensions.CollectionFee(_calculateBuiler.CurrencyRefId, _effectiveBalance, _calculateBuiler.CollectionFeeRate, totalDays);

            }
            if (_calculateBuiler.CollectionFeeModeId == (int)EffectiveEnum.Declined)
            {
                _collectionFeeAmount = CalculatorExtensions.CollectionFee(_calculateBuiler.CurrencyRefId, beginBalance, _calculateBuiler.CollectionFeeRate, totalDays);
            }
            interest = CalculatorExtensions.Interest(_calculateBuiler.CurrencyRefId, beginBalance, req.InterestRate, totalDays);

            totalPay = CalculatorExtensions.TotalPay(principleAndInterestAmount, _collectionFeeAmount);

            if (i < req.Installments)
            {
                principal = principleAndInterestAmount - interest;

                lastBalance = CalculatorExtensions.LastBalance(beginBalance, principal);
            }
            if (i >= req.Installments)
            {
                principal = beginBalance;
                lastBalance = 0;
            }

            if (i % 5 == 0)
            {
                scheduleNum = scheduleNum + 1;
            }

            ScheduleLine line = new ScheduleLine()
            {
                InstallmentNo = i,
                PaymentDate = paymentDate,
                TotalDay = totalDays,
                Principal = principal,
                Interest = interest,
                TotalPay = totalPay,
                OutStanding = lastBalance,
                ScheduleNum = scheduleNum,
                CollectionFeeAmount = _collectionFeeAmount,
                BranchId = req.BranchId,
                CreditAgentRefId = req.CreditAgentRefId,
                RepaymentModeRefId = req.RepaymentModeRefId,

            };

            loanScheduleLines.Add(line);
        }

        return loanScheduleLines;
    }


    private IEnumerable<ScheduleLine> Decline(CalculatorBase req)
    {
        List<ScheduleLine> loanScheduleLines = [];

        int period = req.Installments ?? 0;

        decimal loanAmount = req.Principle ?? 0.00M;

        decimal rate = req.InterestRate ?? 0;

        var principalPaid = CalculatorExtensions.PaidPrincipal(_calculateBuiler.CurrencyRefId, loanAmount, period);

        var collectionFeeAmount = 0M;

        decimal beginBalance = 0, lastBalance = 0;

        DateTime? paymentDate = _calculateBuiler.FirstPaymentDate;// _firstPayment;

        beginBalance = lastBalance = loanAmount;

        _effectiveBalance = req.Principle ?? 0.00M;

        int scheduleNum = 1;

        for (int i = 1; i <= period; i++)
        {
            var totalPay = 0.00M;

            var interest = 0.00M;

            var totalDays = 0;

            beginBalance = lastBalance;

            DateTime? previusDate;

            if (i == 1)
            {
                previusDate = _calculateBuiler.StartDate; //_startDate;

                totalDays = CalculatorExtensions.TotalDays(previusDate, _calculateBuiler.FirstPaymentDate);
            }
            if (i > 1)
            {
                previusDate = loanScheduleLines[i - 2].PaymentDate!.Value;

                paymentDate = SetPaymentDate(previusDate); //StartDate.AddMonths(i);

                SetHoliday(ref paymentDate);

                totalDays = CalculatorExtensions.TotalDays(previusDate, paymentDate);
            }

            if (_calculateBuiler.CollectionFeeModeId == (int)EffectiveEnum.IRR)
            {

                if (i % 5 == 0)
                {
                    _effectiveBalance = lastBalance;
                }

                collectionFeeAmount = CalculatorExtensions.CollectionFee(_calculateBuiler.CurrencyRefId, _effectiveBalance, _calculateBuiler.CollectionFeeRate, totalDays);
            }

            if (_calculateBuiler.CollectionFeeModeId == (int)EffectiveEnum.Declined)
            {
                collectionFeeAmount = CalculatorExtensions.CollectionFee(_calculateBuiler.CurrencyRefId, beginBalance, _calculateBuiler.CollectionFeeRate, totalDays);
            }

            interest = CalculatorExtensions.Interest(_calculateBuiler.CurrencyRefId, beginBalance, rate, totalDays);

            totalPay = CalculatorExtensions.TotalPay(principalPaid, interest, collectionFeeAmount);

            if (i < period)
            {
                lastBalance = CalculatorExtensions.LastBalance(beginBalance, principalPaid);
            }
            if (i >= period)
            {
                principalPaid = beginBalance;

                lastBalance = 0;
            }


            if (i % 5 == 0)
            {
                scheduleNum = scheduleNum + 1;
            }

            ScheduleLine line = new ScheduleLine
            {
                InstallmentNo = i,
                PaymentDate = paymentDate,
                Principal = principalPaid,
                Interest = interest,
                TotalDay = totalDays,
                TotalPay = totalPay,
                OutStanding = lastBalance,
                CollectionFeeAmount = collectionFeeAmount,
                BranchId = req.BranchId,
                ScheduleNum = scheduleNum,
                CreditAgentRefId = req.CreditAgentRefId,
                RepaymentModeRefId = req.RepaymentModeRefId,
            };

            loanScheduleLines.Add(line);
        }

        return loanScheduleLines;



    }



    /// <summary>
    /// Private fields
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>

    private bool IsSunday(DateTimeOffset? date)
    {
        var value = date ?? DateTimeOffset.Now;

        return value.DayOfWeek == DayOfWeek.Sunday;
    }

    private bool IsSunday(DateTime? date)
    {
        var value = date ?? DateTime.Now;

        return value.DayOfWeek == DayOfWeek.Sunday;
    }

    private bool IsHoliday(DateTimeOffset? paymentDate)
    {
        var value = paymentDate.DayNum();

        return _calculateBuiler.Holidays.Any(x => x.HolidayDt.HasValue && x.HolidayDt == paymentDate && x.HolidayNum == value);
    }

    private bool IsHoliday(DateTime? paymentDate)
    {
        var value = paymentDate.DayNum();

        return _calculateBuiler.Holidays.Any(x => x.HolidayDt.HasValue && x.HolidayDt == paymentDate && x.HolidayNum == value);

    }

    private DateTime? SetPaymentDate(DateTime? date)
    {
        return _calculateBuiler.InstallmentTypeId switch
        {
            1 => date!.Value.AddMonths(1),
            2 => date!.Value.AddDays(14),
            3 => date!.Value.AddDays(7),
            4 => date!.Value.AddDays(1),
            _ => DateTime.Now,
        };
    }

}

#region commentcode
//private void SetHolidays(List<HolidayVm> holidays)
//{
//    this.holidays = holidays;
//}
//private void SetSchedulePayment(AdjustmentOption mode)
//{
//    _sechuleMode = mode;
//}

//private bool IsHoliday(DateTimeOffset? holiday, DateTimeOffset? paymentDate)
//{
//    var first = int.Parse(holiday!.Value.ToString("yyyyMMdd"));

//    var second = int.Parse(paymentDate!.Value.ToString("yyyyMMdd"));

//    return first == second;

//}
//public IEnumerable<ScheduleLine> CreateSchedules(LoanDtoCreate req, List<DateTimeOffset> holidays)
//{
//    return _installmentTypeId switch
//    {
//        1 => Balloon(req),

//        2 => Decline(req),

//        3 => Flat(req),

//        4 => Annurity(req),

//        _ => Flat(req)
//    };
//}

//public IEnumerable<ScheduleLine> CreateSchedules(LoanDtoCreate req, RepaymentModes mode, List<DateTimeOffset> holidays)
//{

//    return mode switch
//    {
//        RepaymentModes.Balloon => Balloon(req),

//        RepaymentModes.Declined => Decline(req),

//        RepaymentModes.IRR => Flat(req),

//        RepaymentModes.Annurity => Annurity(req),

//        _ => Flat(req)
//    };
//}


//case DateAdjustment.NearestBefore:
//    if (!IsHolidayOrSunday(date)) return date;
//    var back = date;
//    while (IsHolidayOrSunday(back))
//        back = back.AddDays(-1);
//    return back;

//case DateAdjustment.NearestAfter:
//    if (!IsHolidayOrSunday(date)) return date;
//    var forward = date;
//    while (IsHolidayOrSunday(forward))
//        forward = forward.AddDays(1);
//    return forward;

//case DateAdjustment.NextWeekday:
//    do
//    {
//        date = date.AddDays(1);
//    } while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
//    return date;

//case DateAdjustment.PreviousWeekday:
//    do
//    {
//        date = date.AddDays(-1);
//    } while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
//    return date;


//private DateTimeOffset? SetPaymentDate(DateTimeOffset? date, int InstallmentTypeId)
//{
//    return InstallmentTypeId switch
//    {
//        1 => date!.Value.AddMonths(1),
//        2 => date!.Value.AddDays(14),
//        3 => date!.Value.AddDays(7),
//        4 => date!.Value.AddDays(1),
//        _ => DateTimeOffset.Now,
//    };
//}

//public LoanBuilder(int? currencyId, int? payEvery, int? repaymentModeId, int? collectionFeeModeId, int? installmentTypeId, decimal? effectiveRate, DateTimeOffset? firstPayment = null, DateTimeOffset? startDate = null)
//{
//    _currencyId = currencyId;

//    _payEvery = payEvery;

//    _repaymentModeId = repaymentModeId;

//    _collectionFeeModeId = collectionFeeModeId;

//    _installmentTypeId = installmentTypeId;

//    _collectionFeeRate = effectiveRate;

//    _firstPayment = firstPayment ?? DateTimeOffset.Now;

//    _startDate = startDate ?? DateTimeOffset.Now;
//}

//private DateTimeOffset? SetPaymentDate(DateTimeOffset? date, InstallmentTypes type)
//{
//    return type switch
//    {
//        InstallmentTypes.Monthly => date!.Value.AddMonths(1),
//        InstallmentTypes.BiWeekly => date!.Value.AddDays(14),
//        InstallmentTypes.Weekly => date!.Value.AddDays(7),
//        InstallmentTypes.Daily => date!.Value.AddDays(1),
//        _ => DateTimeOffset.Now,
//    };
//}
//private IEnumerable<ScheduleLine> Balloon(int? periodInMonth, decimal? monthlyRate, decimal? amount)
//{
//    var schedules = new List<ScheduleLine>();

//    decimal _monthlyRate = monthlyRate ?? 0.00M;

//    decimal _amount = amount ?? 0.00M;

//    int? _periodInMonth = periodInMonth ?? 0;

//    int? circles = _periodInMonth / _payEvery;

//    var beginBalance = 0.00M;

//    var lastBalance = 0.00M;

//    var paymentDate = _firstPayment;

//    beginBalance = lastBalance = _amount;

//    for (int i = 1; i <= periodInMonth; i++)
//    {
//        beginBalance = lastBalance;

//        var interestAmount = 0.00M;

//        var totalPayAmount = 0.00M;

//        var effAmount = 0.00M;

//        var totalDays = 0;

//        var principal = CalculatorExtensions.PaidPrincipal(_currencyId, _amount, circles);

//        DateTimeOffset? previusDate;

//        if (i == 1)
//        {
//            previusDate = _startDate;

//            totalDays = CalculatorExtensions.TotalDays(previusDate, _firstPayment);
//        }
//        if (i > 1)
//        {
//            previusDate = schedules[i - 2].PaymentDate!.Value;

//            paymentDate = SetPaymentDate(previusDate); //StartDate.AddMonths(i);

//            SetHoliday(ref paymentDate);

//            totalDays = CalculatorExtensions.TotalDays(previusDate, paymentDate);
//        }

//        if (i % _payEvery == 0)
//        {
//            if (_collectionFeeModeId == 1)
//            {
//                effAmount = CalculatorExtensions.CalcCollectionFee(_currencyId, _amount, _collectionFeeRate, totalDays);
//            }
//            else
//            {
//                effAmount = CalculatorExtensions.CalcCollectionFee(_currencyId, beginBalance, _collectionFeeRate, totalDays);
//            }

//            interestAmount = CalculatorExtensions.CalcInterest(_currencyId, beginBalance, _monthlyRate, totalDays);

//            totalPayAmount = CalculatorExtensions.TotalPay(principal, interestAmount, effAmount);

//            if (i < periodInMonth)
//            {
//                lastBalance = CalculatorExtensions.CalcLastBalance(beginBalance, principal);
//            }
//            else
//            {
//                principal = beginBalance;

//                lastBalance = 0;
//            }

//        }
//        if (i % _payEvery != 0)
//        {
//            principal = 0;

//            if (_collectionFeeModeId == 1)
//            {
//                effAmount = CalculatorExtensions.CalcCollectionFee(_currencyId, _amount, _collectionFeeRate, totalDays);
//            }
//            else
//            {
//                effAmount = CalculatorExtensions.CalcCollectionFee(_currencyId, beginBalance, _collectionFeeRate, totalDays);
//            }

//            interestAmount = CalculatorExtensions.CalcInterest(_currencyId, beginBalance, _monthlyRate, totalDays);

//            totalPayAmount = CalculatorExtensions.TotalPay(principal, interestAmount, effAmount);
//        }

//        ScheduleLine line = new ScheduleLine()
//        {
//            InstallmentNo = i,
//            TotalDay = totalDays,
//            PaymentDate = paymentDate,
//            Principal = principal,
//            Interest = interestAmount,
//            CollectionFeeAmount = effAmount,
//            TotalPay = totalPayAmount,
//            OutStanding = lastBalance
//        };

//        schedules.Add(line);
//    }

//    return schedules;
//}
//private IEnumerable<ScheduleLine> Flat(int? periodInMonth, decimal? monthlyRate, decimal? amount)
//{
//    List<ScheduleLine> scheduleLines = [];
//    var principalPaid = CalculatorExtensions.PaidPrincipal(_currencyId, amount, periodInMonth);
//    decimal beginBalance = 0, lastBalance = 0;
//    DateTimeOffset? paymentDate = _firstPayment;
//    beginBalance = lastBalance = amount!.Value;
//    var effAmount = 0M;
//    DateTimeOffset? previusDate;
//    for (int i = 1; i <= periodInMonth; i++)
//    {
//        beginBalance = lastBalance;
//        var totalDays = 0;
//        var totalPay = 0.00M;
//        var interest = 0.00M;
//        if (i < periodInMonth)
//        {
//            lastBalance = CalculatorExtensions.CalcLastBalance(beginBalance, principalPaid);
//        }
//        if (i >= periodInMonth)
//        {
//            principalPaid = beginBalance;
//            lastBalance = 0;
//        }
//        if (i == 1)
//        {
//            previusDate = _startDate;
//            totalDays = CalculatorExtensions.TotalDays(previusDate, _firstPayment);
//        }
//        if (i > 1)
//        {
//            previusDate = scheduleLines[i - 2].PaymentDate!.Value;
//            paymentDate = SetPaymentDate(previusDate);
//            SetHoliday(ref paymentDate);
//            totalDays = CalculatorExtensions.TotalDays(previusDate, paymentDate);
//        }
//        if (_collectionFeeModeId == 1)
//        {
//            effAmount = CalculatorExtensions.CalcCollectionFee(_currencyId, amount, _collectionFeeRate, totalDays);
//        }
//        if (_collectionFeeModeId == 2)
//        {
//            effAmount = CalculatorExtensions.CalcCollectionFee(_currencyId, beginBalance, _collectionFeeRate, totalDays);
//        }

//        interest = CalculatorExtensions.CalcInterest(_currencyId, amount.Value, monthlyRate!.Value, totalDays);

//        totalPay = CalculatorExtensions.TotalPay(principalPaid, interest, effAmount);

//        ScheduleLine line = new ScheduleLine()
//        {
//            InstallmentNo = i,
//            PaymentDate = paymentDate,
//            TotalDay = totalDays,
//            Principal = principalPaid,
//            Interest = interest,
//            TotalPay = totalPay,
//            OutStanding = lastBalance,
//            CollectionFeeAmount = effAmount
//        };
//        scheduleLines.Add(line);
//    }

//    return scheduleLines;
//}
//private IEnumerable<ScheduleLine> Annurity(int? periodInMonth, decimal? monthlyRate, decimal? amount)
//{
//    List<ScheduleLine> loanScheduleLines = [];

//    var principleAndInterestAmount = CalculatorExtensions.CalcPmt(_currencyId, periodInMonth, monthlyRate, -amount);

//    decimal beginBalance = 0, lastBalance = 0;

//    DateTimeOffset? paymentDate = _firstPayment;

//    beginBalance = lastBalance = amount!.Value;

//    var effAmount = 0M;

//    for (int i = 1; i <= periodInMonth; i++)
//    {
//        var totalPay = 0.00M;

//        var interest = 0.00M;

//        var totalDays = 0;

//        var principal = 0.00M;

//        beginBalance = lastBalance;

//        DateTimeOffset? previusDate;
//        if (i == 1)
//        {

//            previusDate = _startDate;

//            totalDays = CalculatorExtensions.TotalDays(previusDate, _firstPayment);
//        }
//        if (i > 1)
//        {
//            previusDate = loanScheduleLines[i - 2].PaymentDate!.Value;

//            paymentDate = SetPaymentDate(previusDate);

//            SetHoliday(ref paymentDate);

//            totalDays = CalculatorExtensions.TotalDays(previusDate, paymentDate);

//        }
//        if (_collectionFeeModeId == 1)
//        {
//            effAmount = CalculatorExtensions.CalcCollectionFee(_currencyId, amount, _collectionFeeRate, totalDays);
//        }
//        if (_collectionFeeModeId == 2)
//        {
//            effAmount = CalculatorExtensions.CalcCollectionFee(_currencyId, beginBalance, _collectionFeeRate, totalDays);
//        }
//        interest = CalculatorExtensions.CalcInterest(_currencyId, beginBalance, monthlyRate!.Value, totalDays);

//        totalPay = CalculatorExtensions.TotalPay(principleAndInterestAmount, effAmount);

//        if (i < periodInMonth)
//        {
//            principal = principleAndInterestAmount - interest;

//            lastBalance = CalculatorExtensions.CalcLastBalance(beginBalance, principal);
//        }
//        if (i >= periodInMonth)
//        {
//            principal = beginBalance;
//            lastBalance = 0;
//        }

//        ScheduleLine line = new ScheduleLine()
//        {
//            InstallmentNo = i,
//            PaymentDate = paymentDate,
//            TotalDay = totalDays,
//            Principal = principal,
//            Interest = interest,
//            TotalPay = totalPay,
//            OutStanding = lastBalance,
//            CollectionFeeAmount = effAmount

//        };

//        loanScheduleLines.Add(line);
//    }

//    return loanScheduleLines;
//}
//private IEnumerable<ScheduleLine> Decline(int? periodInMonth, decimal? monthlyRate, decimal? amount)
//{
//    List<ScheduleLine> loanScheduleLines = [];

//    int period = periodInMonth ?? 0;

//    decimal _amount = amount ?? 0.00M;

//    decimal rate = monthlyRate ?? 0;

//    var principalPaid = CalculatorExtensions.PaidPrincipal(_currencyId, _amount, period);

//    var effAmount = 0M;

//    decimal beginBalance = 0, lastBalance = 0;

//    DateTimeOffset? paymentDate = _firstPayment;

//    beginBalance = lastBalance = _amount;

//    for (int i = 1; i <= period; i++)
//    {
//        var totalPay = 0.00M;
//        var interest = 0.00M;
//        var totalDays = 0;
//        beginBalance = lastBalance;
//        DateTimeOffset? previusDate;
//        if (i == 1)
//        {
//            previusDate = _startDate;

//            totalDays = CalculatorExtensions.TotalDays(previusDate, _firstPayment);
//        }
//        if (i > 1)
//        {
//            previusDate = loanScheduleLines[i - 2].PaymentDate!.Value;

//            paymentDate = SetPaymentDate(previusDate); //StartDate.AddMonths(i);

//            SetHoliday(ref paymentDate);

//            totalDays = CalculatorExtensions.TotalDays(previusDate, paymentDate);
//        }

//        if (_collectionFeeModeId == 1)
//        {
//            effAmount = CalculatorExtensions.CalcCollectionFee(_currencyId, _amount, _collectionFeeRate, totalDays);
//        }
//        if (_collectionFeeModeId == 2)
//        {
//            effAmount = CalculatorExtensions.CalcCollectionFee(_currencyId, beginBalance, _collectionFeeRate, totalDays);
//        }

//        interest = CalculatorExtensions.CalcInterest(_currencyId, beginBalance, rate, totalDays);

//        totalPay = CalculatorExtensions.TotalPay(principalPaid, interest, effAmount);

//        if (i < period)
//        {
//            lastBalance = CalculatorExtensions.CalcLastBalance(beginBalance, principalPaid);
//        }
//        if (i >= period)
//        {
//            principalPaid = beginBalance;
//            lastBalance = 0;
//        }
//        ScheduleLine line = new ScheduleLine()
//        {
//            InstallmentNo = i,
//            PaymentDate = paymentDate,
//            Principal = principalPaid,
//            Interest = interest,
//            TotalDay = totalDays,
//            TotalPay = totalPay,
//            OutStanding = lastBalance,
//            CollectionFeeAmount = effAmount
//        };
//        loanScheduleLines.Add(line);
//    }

//    return loanScheduleLines;



//}


//private IEnumerable<ScheduleLine> CreateSchedules(int? periodInMonth, decimal? rateInMonth, decimal? amount)
//{

//    return _repaymentModeId switch
//    {
//        1 => Balloon(periodInMonth, rateInMonth, amount),

//        2 => Decline(periodInMonth, rateInMonth, amount),

//        3 => Flat(periodInMonth, rateInMonth, amount),

//        4 => Annurity(periodInMonth, rateInMonth, amount),

//        _ => Flat(periodInMonth, rateInMonth, amount)
//    };
//}

//private IEnumerable<ScheduleLine> CreateSchedules(LoanBuilderBase param)
//{

//    return _repaymentModeId switch
//    {
//        1 => Balloon(param.Period, param.RateInMonth, param.Amount),

//        2 => Decline(param.Period, param.RateInMonth, param.Amount),

//        3 => Flat(param.Period, param.RateInMonth, param.Amount),

//        4 => Annurity(param.Period, param.RateInMonth, param.Amount),

//        _ => Flat(param.Period, param.RateInMonth, param.Amount)
//    };
//}


//private static bool IsHolidayOrSunday(DateTime date, List<DateTime> holidays)
//{
//    return date.DayOfWeek == DayOfWeek.Sunday || holidays.Contains(date.Date);
//}

//private static DateTime? AdjustDate(DateTime date, List<DateTime> holidays, AdjustmentOption adjustment)
//{
//    bool IsHolidayOrSunday(DateTime d) =>
//        d.DayOfWeek == DayOfWeek.Sunday || holidays.Contains(d.Date);

//    switch (adjustment)
//    {
//        case AdjustmentOption.Skip:
//            return date;
//        case AdjustmentOption.Before:
//            while (IsHolidayOrSunday(date))
//                date = date.AddDays(-1);
//            return date;
//        case AdjustmentOption.After:
//            while (IsHolidayOrSunday(date))
//                date = date.AddDays(1);
//            return date;
//        default:
//            throw new ArgumentOutOfRangeException(nameof(adjustment));
//    }
//}

//private void SetHoliday(ref DateTimeOffset? paymentDate)
//{
//    if (paymentDate == null) return; // No date to process

//    paymentDate = FindValidDate(paymentDate);
//}
//private DateTimeOffset? SetPaymentDate(DateTimeOffset? date)
//{
//    return _installmentTypeId switch
//    {
//        1 => date!.Value.AddMonths(1),
//        2 => date!.Value.AddDays(14),
//        3 => date!.Value.AddDays(7),
//        4 => date!.Value.AddDays(1),
//        _ => DateTimeOffset.Now,
//    };
//}

#endregion