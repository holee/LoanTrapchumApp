using Application.Enumerations;
using Application.Features.Holidays.Dtos;
using Domains.Enumerations;
using Domains.Models.Loans;

namespace Application.Calculations;

public sealed class CalculatorSceduleService(IMediator mediator)
{

    private decimal _effectiveAmount = 0;

    private List<ScheduleLine>? _scheduleLines = null;

    private Dictionary<int, IEnumerable<ScheduleLine>>? _groupSchedules = null;

    private const int _groupSize = 5;

    private const int _numPerSchedule = 4;

    public IEnumerable<HolidayDto> Holidays { get; private set; } = [];

    private int groupKey = 0;
    public IEnumerable<ScheduleLine> Calculate(LoanDtoCreate? req)
    {

        if (req == null)
        {
            return [];
        }

        return req.RepaymentModeRefId switch
        {
            (int)RepaymentOption.Declined => Decline(req),

            (int)RepaymentOption.IRR => Flat(req),

            (int)RepaymentOption.Annurity => Annurity(req),

            _ => Balloon(req)
        };

    }

    private IEnumerable<ScheduleLine> Decline(LoanDtoCreate? req)
    {
        ArgumentNullException.ThrowIfNull(req, nameof(req));

        _ = GetHolidays(req.InstallmentNo);

        int period = req.Installments ?? 0;

        decimal originalAmount = req.Principle ?? 0M;

        decimal rate = req.InterestRate ?? 0M;

        decimal principalPerInstallment = originalAmount.PaidPrincipal(req.CurrencyId, period);

        decimal lastBalance = originalAmount;

        DateTime? paymentDate = req.FirstPaymentDate;

        DateTime? previousPaymentDate = req.StartDate;

        _effectiveAmount = originalAmount;

        InitializeGroups(req, originalAmount);

        for (int i = 1; i <= period; i++)
        {
            decimal beginBalance = lastBalance;

            if (i > 1)
            {
                previousPaymentDate = paymentDate;

                paymentDate = SetPaymentDate(req, paymentDate);
            }

            DateTime? adjustedPaymentDate = paymentDate;

            SetHoliday(req, ref adjustedPaymentDate);

            int totalDays = previousPaymentDate.TotalDays(adjustedPaymentDate);

            UpdateEffectiveAmount(req, i, beginBalance);

            decimal collectionFee = CalculateCollectionFee(req, _effectiveAmount, totalDays);

            decimal interest = beginBalance.Interest(req.CurrencyId, rate, totalDays);

            bool isLastInstallment = i == period;

            decimal principal = 0;

            if (isLastInstallment)
            {
                principal = originalAmount.LastScheduleAmount(req.CurrencyId, period, principalPerInstallment);
            }
            else
            {
                principal = principalPerInstallment;
            }

            lastBalance = isLastInstallment ? 0M : beginBalance.LastBalance(principal);

            int scheduleNum = ((i - 1) / _groupSize) + 1;

            _scheduleLines!.Add(BuildScheduleLine(req, i, adjustedPaymentDate, principal, interest, totalDays, collectionFee, lastBalance, scheduleNum));

            TryFlushGroup();
        }

        return FinalizeSchedules();
    }

    private IEnumerable<ScheduleLine> Flat(LoanDtoCreate? req)
    {
        ArgumentNullException.ThrowIfNull(req, nameof(req));

        _ = GetHolidays(req.InstallmentNo);

        decimal amount = req.Principle ?? 0M;

        int termInMonth = req.Installments ?? 12;

        decimal monthlyRate = req.InterestRate ?? 0M;

        decimal principalPaid = amount.PaidPrincipal(req.CurrencyId, termInMonth);

        decimal lastBalance = amount;

        _effectiveAmount = amount;

        DateTime? basePaymentDate = req.FirstPaymentDate;

        DateTime? previousBaseDate = req.StartDate;

        InitializeGroups(req, amount);

        for (int i = 1; i <= termInMonth; i++)
        {
            decimal beginBalance = lastBalance;

            if (i > 1)
            {
                previousBaseDate = basePaymentDate;

                basePaymentDate = SetPaymentDate(req, basePaymentDate);
            }

            DateTime? paymentDate = basePaymentDate;

            SetHoliday(req, ref paymentDate);

            int totalDays = previousBaseDate.TotalDays(paymentDate);

            if (i < termInMonth)
            {
                lastBalance = beginBalance.LastBalance(principalPaid);
            }
            else
            {
                principalPaid = beginBalance;

                lastBalance = 0;
            }

            UpdateEffectiveAmount(req, i, beginBalance);

            decimal collectionFee = CalculateCollectionFee(req, _effectiveAmount, totalDays);

            decimal interest = amount.Interest(req.CurrencyId, monthlyRate, totalDays);

            int scheduleNum = ((i - 1) / _groupSize) + 1;

            _scheduleLines!.Add(BuildScheduleLine(req, i, basePaymentDate, principalPaid, interest, totalDays, collectionFee, lastBalance, scheduleNum));

            TryFlushGroup();
        }

        return FinalizeSchedules();
    }

    private IEnumerable<ScheduleLine> Balloon(LoanDtoCreate? req)
    {
        ArgumentNullException.ThrowIfNull(req, nameof(req));
        _ = GetHolidays(req.InstallmentNo);

        decimal monthlyRate = req.InterestRate ?? 0.00M;

        decimal amount = req.Principle ?? 0.00M;

        int periodInMonth = req.Installments ?? 0;

        int? circles = periodInMonth / req.PayEvery;

        decimal lastBalance = amount;

        _effectiveAmount = amount;

        DateTime? basePaymentDate = req.FirstPaymentDate;

        DateTime? previousBaseDate = req.StartDate;

        InitializeGroups(req, amount);

        for (int i = 1; i <= periodInMonth; i++)
        {
            decimal beginBalance = lastBalance;

            if (i > 1)
            {
                previousBaseDate = basePaymentDate;

                basePaymentDate = SetPaymentDate(req, basePaymentDate);
            }

            DateTime? paymentDate = basePaymentDate;

            SetHoliday(req, ref paymentDate);

            int totalDays = CalculatorExtensions.TotalDays(previousBaseDate, paymentDate);

            decimal principal = (i % req.PayEvery == 0)
                ? amount.PaidPrincipal(req.CurrencyId, circles)
                : 0M;

            UpdateEffectiveAmount(req, i, beginBalance);

            decimal collectionFee = CalculateCollectionFee(req, _effectiveAmount, totalDays);

            decimal interestAmount = beginBalance.Interest(req.CurrencyId, monthlyRate, totalDays);

            if (i % req.PayEvery == 0)
            {
                if (i < periodInMonth)
                {
                    lastBalance = beginBalance.LastBalance(principal);
                }
                else
                {
                    principal = beginBalance;

                    lastBalance = 0;
                }
            }

            int scheduleNum = ((i - 1) / _groupSize) + 1;

            _scheduleLines!.Add(BuildScheduleLine(req, i, basePaymentDate, principal, interestAmount, totalDays, collectionFee, lastBalance, scheduleNum));

            TryFlushGroup();
        }

        return FinalizeSchedules();
    }

    private IEnumerable<ScheduleLine> Annurity(LoanDtoCreate? req)
    {

        ArgumentNullException.ThrowIfNull(req, nameof(req));

        _ = GetHolidays(req.InstallmentNo);

        decimal principleAndInterestAmount = req.Principle.CalcPmt(req.CurrencyId, req.Installments, req.InterestRate);

        decimal lastBalance = req.Principle!.Value;

        _effectiveAmount = req.Principle.Value;

        DateTime? basePaymentDate = req.FirstPaymentDate;

        DateTime? previousBaseDate = req.StartDate;

        InitializeGroups(req, lastBalance);

        for (int i = 1; i <= req.Installments; i++)
        {
            decimal beginBalance = lastBalance;

            if (i > 1)
            {
                previousBaseDate = basePaymentDate;

                basePaymentDate = SetPaymentDate(req, basePaymentDate);
            }

            DateTime? paymentDate = basePaymentDate;

            SetHoliday(req, ref paymentDate);

            int totalDays = previousBaseDate.TotalDays(paymentDate);

            UpdateEffectiveAmount(req, i, beginBalance);

            decimal collectionFee = CalculateCollectionFee(req, _effectiveAmount, totalDays);

            decimal interest = beginBalance.Interest(req.CurrencyId, req.InterestRate, totalDays);

            decimal principal;

            if (i < req.Installments)
            {
                principal = principleAndInterestAmount - interest;

                lastBalance = beginBalance.LastBalance(principal);
            }
            else
            {
                principal = beginBalance;

                lastBalance = 0;
            }

            int scheduleNum = ((i - 1) / _groupSize) + 1;

            _scheduleLines!.Add(BuildScheduleLine(req, i, paymentDate, principal, interest, totalDays, collectionFee, lastBalance, scheduleNum));

            TryFlushGroup();
        }

        return FinalizeSchedules();
    }

    private void InitializeGroups(LoanDtoCreate req, decimal openingBalance)
    {
        groupKey = 0;
        _scheduleLines = [];
        _groupSchedules = [];

        var header = BuildScheduleLine(req, installmentNo: 0, paymentDate: null,
            principal: 0, interest: 0, totalDays: 0,
            effectiveAmount: 0, outStanding: openingBalance, scheduleNum: 0);

        _scheduleLines.Add(header);
        _groupSchedules.Add(groupKey, _scheduleLines);
        _scheduleLines = [];
        groupKey++;
    }

    private void TryFlushGroup()
    {
        if (_scheduleLines!.Count == _numPerSchedule)
        {
            _groupSchedules!.Add(groupKey, _scheduleLines);

            groupKey++;

            _scheduleLines = [];
        }
    }

    private IEnumerable<ScheduleLine> FinalizeSchedules()
    {
        if (_scheduleLines?.Count > 0)
        {
            _groupSchedules!.Add(groupKey, _scheduleLines);

            groupKey++;

            _scheduleLines = [];
        }

        return _groupSchedules?.SelectMany(x => x.Value)
                               .OrderBy(x => x.InstallmentNo)
                               .ToHashSet() ?? [];
    }

    private void UpdateEffectiveAmount(LoanDtoCreate req, int installmentNo, decimal beginBalance)
    {


        if (req.CollectionFeeModeId == (int)EffectiveEnum.IRR)
        {
            int groupKey = ((installmentNo) / _groupSize) + 1;

            var lastBalances = _groupSchedules?.Where(x => x.Key == groupKey - 1)
                                                 .SelectMany(x => x.Value)
                                                 .FirstOrDefault()?.OutStanding;
            _effectiveAmount = lastBalances ?? 00;
        }
        else
        {
            _effectiveAmount = beginBalance;

        }

    }

    private decimal CalculateCollectionFee(LoanDtoCreate req, decimal beginBalance, int totalDays)
    {
        return (EffectiveEnum)req.CollectionFeeModeId! switch
        {
            EffectiveEnum.IRR => CalculatorExtensions.CollectionFee(req, _effectiveAmount, totalDays),

            EffectiveEnum.Declined => CalculatorExtensions.CollectionFee(req, beginBalance, totalDays),

            _ => 0M
        };
    }

    private ScheduleLine BuildScheduleLine(
        LoanDtoCreate req,
        int installmentNo,
        DateTime? paymentDate,
        decimal principal,
        decimal interest,
        int totalDays,
        decimal effectiveAmount,
        decimal outStanding,
        int scheduleNum) => new()
        {
            InstallmentNo = installmentNo,
            PaymentDate = paymentDate,
            Principal = principal,
            Interest = interest,
            TotalDay = totalDays,
            TotalPay = principal.TotalPay(interest, effectiveAmount),
            OutStanding = outStanding,
            CollectionFeeAmount = effectiveAmount,
            BranchId = req.BranchId,
            ScheduleNum = scheduleNum,
            CreditAgentRefId = installmentNo == 0 ? string.Empty : req.CreditAgentRefId,
            RepaymentModeRefId = installmentNo == 0 ? 0 : req.RepaymentModeRefId
        };
    private DateTime? SetPaymentDate(LoanDtoCreate req, DateTime? date)
    {
        if (date == null)
        {
            return null;
        }

        return req.InstallmentTypeId switch
        {
            1 => date.Value.AddMonths(1),

            2 => date.Value.AddDays(14),

            3 => date.Value.AddDays(7),

            4 => date.Value.AddDays(1),

            _ => date
        };
    }
    private void SetHoliday(LoanDtoCreate req, ref DateTime? paymentDate)
    {
        if (paymentDate == null)
        {
            return;
        }

        paymentDate = FindValidDate(req, paymentDate)!.Value.Date;
    }

    private DateTime? FindValidDate(LoanDtoCreate req, DateTime? date)
    {
        if (req.ScheduleMode == AdjustmentOption.Skip)
            return date;

        bool isInvalid = IsSunday(date) || IsHoliday(date);
        if (!isInvalid)
            return date;

        DateTime? nextDate = req.ScheduleMode switch
        {
            AdjustmentOption.Before => date!.Value.AddDays(-1),
            AdjustmentOption.After => date!.Value.AddDays(1),
            _ => throw new InvalidOperationException("Invalid schedule mode")
        };

        return FindValidDate(req, nextDate);
    }

    private static bool IsSunday(DateTime? date) => date.HasValue && date.Value.DayOfWeek == DayOfWeek.Sunday;

    private bool IsHoliday(DateTime? date)
    {
        if (date is null) return false;

        return Holidays.Any(h =>
            h.HolidayDt.HasValue &&
            h.HolidayDt.Value.Date == date.Value.Date &&
            h.Status == true);
    }

    public async Task GetHolidays(int? installment = 6)
    {

        ArgumentNullException.ThrowIfNullOrEmpty(nameof(installment));

        var year = (installment / 12) < 0 ? 1 : (installment / 12) + 1;

        var command = new GetActiveHolidaysQuery(year);

        var rsp = await mediator.Send(command);

        Holidays = rsp.Items;
    }

}


