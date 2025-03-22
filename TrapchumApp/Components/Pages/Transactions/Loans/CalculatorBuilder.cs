using Domains.Models.Loans;
using TCLMS.Shared.Entities.LoanCalculators;

namespace TrapchumApp.Components.Pages.Transactions.Loans
{
    public sealed class CalculatorBuilder
    {
        int _currencyId;
        int _payEvery = 1;
        int _repaymentModeId;
        int _collectionFeeModeId = 1;
        int _installmentTypeId = 1;
        decimal _effectiveRate = 0;
        DateTime _firstPayment = DateTime.UtcNow;
        DateTime _startDate = DateTime.UtcNow;
        List<DateTime> Holidays { get; set; } = [];
        public CalculatorBuilder(int currencyId, int payEvery, int repaymentModeId,
            int collectionFeeModeId, int installmentTypeId, decimal effectiveRate,
            DateTime firstPayment, DateTime startDate)
        {
            _currencyId = currencyId;
            _payEvery = payEvery;
            _repaymentModeId = repaymentModeId;
            _collectionFeeModeId = collectionFeeModeId;
            _installmentTypeId = installmentTypeId;
            _effectiveRate = effectiveRate;
            _firstPayment = firstPayment;
            _startDate = startDate;
        }
        public void SetHolidays(List<DateTime> Holidays)
        {
            this.Holidays = Holidays;
        }

        public CalculatorBuilder SetHoliday(List<DateTime> Holidays)
        {
            this.Holidays = Holidays;
            return this;
        }
        public IEnumerable<ScheduleLine> Calculate(int? periodInMonth, decimal? rateInMonth, decimal? amount)
        {

            return _repaymentModeId switch
            {
                1 => CalBalloon(periodInMonth, rateInMonth, amount),
                2 => CalDecline(periodInMonth, rateInMonth, amount),
                3 => CalFlat(periodInMonth, rateInMonth, amount),
                4 => CalAnnurity(periodInMonth, rateInMonth, amount),
                _ => CalFlat(periodInMonth, rateInMonth, amount)
            };
        }


        private void SetHoliday(ref DateTime paymentDate)
        {
            foreach (var holiday in Holidays)
            {
                if (IsSunday(paymentDate) || IsHoliday(holiday, paymentDate))
                {
                    paymentDate = paymentDate.AddDays(1);
                }
            }

        }
        private IEnumerable<ScheduleLine> CalBalloon(int? periodInMonth, decimal? monthlyRate, decimal? amount)
        {
            var schedules = new List<ScheduleLine>();

            decimal _monthlyRate = monthlyRate ?? 0.00M;

            decimal _amount = amount ?? 0.00M;

            int _periodInMonth = periodInMonth ?? 0;

            int circles = _periodInMonth / _payEvery;

            var beginBalance = 0.00M;

            var lastBalance = 0.00M;

            var paymentDate = _firstPayment;

            beginBalance = lastBalance = _amount;

            for (int i = 1; i <= periodInMonth; i++)
            {
                beginBalance = lastBalance;
                var interestAmount = 0.00M;
                var totalPayAmount = 0.00M;
                var effAmount = 0.00M;
                var totalDays = 0;
                var principal = CalculatorExtensions.PaidPrincipal(_currencyId, _amount, circles);
                DateTime previusDate;
                if (i == 1)
                {
                    previusDate = _startDate;
                    totalDays = CalculatorExtensions.TotalDays(previusDate, _firstPayment);
                }
                if (i > 1)
                {
                    previusDate = schedules[i - 2].PaymentDate!.Value;
                    paymentDate = SetPaymentDate(previusDate); //StartDate.AddMonths(i);
                    SetHoliday(ref paymentDate);
                    totalDays = CalculatorExtensions.TotalDays(previusDate, paymentDate);
                }

                if (i % _payEvery == 0)
                {
                    if (_collectionFeeModeId == 1)
                    {
                        effAmount = CalculatorExtensions.CalcEffective(_currencyId, _amount, _effectiveRate, totalDays);
                    }
                    else
                    {
                        effAmount = CalculatorExtensions.CalcEffective(_currencyId, beginBalance, _effectiveRate, totalDays);
                    }

                    interestAmount = CalculatorExtensions.CalcInterest(_currencyId, beginBalance, _monthlyRate, totalDays);

                    totalPayAmount = CalculatorExtensions.TotalPay(principal, interestAmount, effAmount);

                    if (i < periodInMonth)
                    {
                        lastBalance = CalculatorExtensions.LastBalance(beginBalance, principal);
                    }
                    else
                    {
                        principal = beginBalance;
                        lastBalance = 0;
                    }

                }
                if (i % _payEvery != 0)
                {
                    principal = 0;
                    if (_collectionFeeModeId == 1)
                    {
                        effAmount = CalculatorExtensions.CalcEffective(_currencyId, _amount, _effectiveRate, totalDays);
                    }
                    else
                    {
                        effAmount = CalculatorExtensions.CalcEffective(_currencyId, beginBalance, _effectiveRate, totalDays);
                    }

                    interestAmount = CalculatorExtensions.CalcInterest(_currencyId, beginBalance, _monthlyRate, totalDays);

                    totalPayAmount = CalculatorExtensions.TotalPay(principal, interestAmount, effAmount);
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
                    OutStanding = lastBalance
                };

                schedules.Add(line);
            }

            return schedules;
        }
        private IEnumerable<ScheduleLine> CalFlat(int? periodInMonth, decimal? monthlyRate, decimal? amount)
        {
            List<ScheduleLine> scheduleLines = [];
            var principalPaid = CalculatorExtensions.PaidPrincipal(_currencyId, amount, periodInMonth);
            decimal beginBalance = 0, lastBalance = 0;
            DateTime paymentDate = _firstPayment;
            beginBalance = lastBalance = amount!.Value;
            var effAmount = 0M;
            DateTime previusDate;
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
                    previusDate = _startDate;
                    totalDays = CalculatorExtensions.TotalDays(previusDate, _firstPayment);
                }
                if (i > 1)
                {
                    previusDate = scheduleLines[i - 2].PaymentDate!.Value;
                    paymentDate = SetPaymentDate(previusDate);
                    SetHoliday(ref paymentDate);
                    totalDays = CalculatorExtensions.TotalDays(previusDate, paymentDate);
                }
                if (_collectionFeeModeId == 1)
                {
                    effAmount = CalculatorExtensions.CalcEffective(_currencyId, amount, _effectiveRate, totalDays);
                }
                if (_collectionFeeModeId == 2)
                {
                    effAmount = CalculatorExtensions.CalcEffective(_currencyId, beginBalance, _effectiveRate, totalDays);
                }

                interest = CalculatorExtensions.CalcInterest(_currencyId, amount.Value, monthlyRate!.Value, totalDays);

                totalPay = CalculatorExtensions.TotalPay(principalPaid, interest, effAmount);

                ScheduleLine line = new ScheduleLine()
                {
                    InstallmentNo = i,
                    PaymentDate = paymentDate,
                    TotalDay = totalDays,
                    Principal = principalPaid,
                    Interest = interest,
                    TotalPay = totalPay,
                    OutStanding = lastBalance,
                    CollectionFeeAmount = effAmount
                };
                scheduleLines.Add(line);
            }

            return scheduleLines;
        }
        private IEnumerable<ScheduleLine> CalAnnurity(int? periodInMonth, decimal? monthlyRate, decimal? amount)
        {
            List<ScheduleLine> loanScheduleLines = [];

            var principleAndInterestAmount = CalculatorExtensions.CalcPmt(_currencyId, periodInMonth, monthlyRate, -amount);

            decimal beginBalance = 0, lastBalance = 0;

            DateTime paymentDate = _firstPayment;

            beginBalance = lastBalance = amount!.Value;

            var effAmount = 0M;

            for (int i = 1; i <= periodInMonth; i++)
            {
                var totalPay = 0.00M;
                var interest = 0.00M;
                var totalDays = 0;
                var principal = 0.00M;
                beginBalance = lastBalance;
                DateTime previusDate;
                if (i == 1)
                {
                    previusDate = _startDate;
                    totalDays = CalculatorExtensions.TotalDays(previusDate, _firstPayment);
                }
                if (i > 1)
                {
                    previusDate = loanScheduleLines[i - 2].PaymentDate!.Value;

                    paymentDate = SetPaymentDate(previusDate);

                    SetHoliday(ref paymentDate);

                    totalDays = CalculatorExtensions.TotalDays(previusDate, paymentDate);

                }
                if (_collectionFeeModeId == 1)
                {
                    effAmount = CalculatorExtensions.CalcEffective(_currencyId, amount, _effectiveRate, totalDays);
                }
                if (_collectionFeeModeId == 2)
                {
                    effAmount = CalculatorExtensions.CalcEffective(_currencyId, beginBalance, _effectiveRate, totalDays);
                }
                interest = CalculatorExtensions.CalcInterest(_currencyId, beginBalance, monthlyRate!.Value, totalDays);

                totalPay = CalculatorExtensions.TotalPay(principleAndInterestAmount, effAmount);

                if (i < periodInMonth)
                {
                    principal = principleAndInterestAmount - interest;

                    lastBalance = CalculatorExtensions.LastBalance(beginBalance, principal);
                }
                if (i >= periodInMonth)
                {
                    principal = beginBalance;
                    lastBalance = 0;
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
                    CollectionFeeAmount = effAmount

                };

                loanScheduleLines.Add(line);
            }

            return loanScheduleLines;
        }
        private IEnumerable<ScheduleLine> CalDecline(int? periodInMonth, decimal? monthlyRate, decimal? amount)
        {
            List<ScheduleLine> loanScheduleLines = [];

            int period = periodInMonth ?? 0;

            decimal _amount = amount ?? 0.00M;

            decimal rate = monthlyRate ?? 0;

            var principalPaid = CalculatorExtensions.PaidPrincipal(_currencyId, _amount, period);

            var effAmount = 0M;

            decimal beginBalance = 0, lastBalance = 0;

            DateTime paymentDate = _firstPayment;

            beginBalance = lastBalance = _amount;

            for (int i = 1; i <= period; i++)
            {
                var totalPay = 0.00M;
                var interest = 0.00M;
                var totalDays = 0;
                beginBalance = lastBalance;
                DateTime previusDate;
                if (i == 1)
                {
                    previusDate = _startDate;

                    totalDays = CalculatorExtensions.TotalDays(previusDate, _firstPayment);
                }
                if (i > 1)
                {
                    previusDate = loanScheduleLines[i - 2].PaymentDate!.Value;
                    paymentDate = SetPaymentDate(previusDate); //StartDate.AddMonths(i);
                    SetHoliday(ref paymentDate);
                    totalDays = CalculatorExtensions.TotalDays(previusDate, paymentDate);
                }

                if (_collectionFeeModeId == 1)
                {
                    effAmount = CalculatorExtensions.CalcEffective(_currencyId, _amount, _effectiveRate, totalDays);
                }
                if (_collectionFeeModeId == 2)
                {
                    effAmount = CalculatorExtensions.CalcEffective(_currencyId, beginBalance, _effectiveRate, totalDays);
                }

                interest = CalculatorExtensions.CalcInterest(_currencyId, beginBalance, rate, totalDays);

                totalPay = CalculatorExtensions.TotalPay(principalPaid, interest, effAmount);

                if (i < period)
                {
                    lastBalance = CalculatorExtensions.LastBalance(beginBalance, principalPaid);
                }
                if (i >= period)
                {
                    principalPaid = beginBalance;
                    lastBalance = 0;
                }
                ScheduleLine line = new ScheduleLine()
                {
                    InstallmentNo = i,
                    PaymentDate = paymentDate,
                    Principal = principalPaid,
                    Interest = interest,
                    TotalDay = totalDays,
                    TotalPay = totalPay,
                    OutStanding = lastBalance,
                    CollectionFeeAmount = effAmount
                };
                loanScheduleLines.Add(line);
            }

            return loanScheduleLines;



        }
        private bool IsSunday(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Sunday;
        }
        private bool IsHoliday(DateTime holiday, DateTime paymentDate)
        {
            var days = (holiday - paymentDate).TotalDays;

            return days == 0;

        }
        private DateTime SetPaymentDate(DateTime date, int InstallmentTypeId = 1)
        {
            return InstallmentTypeId switch
            {
                1 => date.AddMonths(1),
                2 => date.AddDays(14),
                3 => date.AddDays(7),
                4 => date.AddDays(1),
                _ => DateTime.Now,
            };
        }
    }
}
