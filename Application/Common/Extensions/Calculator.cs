using Domains.Models.Loans;

namespace Application.Extensions
{
    public static class Calculator
    {

        public static List<ScheduleLine> Calculate(int periodInMonth,decimal monthlyRate,decimal amount) 
        {
            List<ScheduleLine> loanScheduleLines = new List<ScheduleLine>(); 
            var principalPaid=Math.Round(amount / periodInMonth,2);
            var totalPay = 0.00M;
            var interest = 0.00M;
            decimal beginingBalance=0,lastBalance=0;
            DateTime paymentDate=DateTime.Now;
            beginingBalance = lastBalance = amount;
            for(int i = 1; i <= periodInMonth; i++)
            {
                beginingBalance = lastBalance;
                interest = beginingBalance * (monthlyRate / 100);
                totalPay = interest + principalPaid;
                if (i < periodInMonth)
                {
                    lastBalance = beginingBalance - principalPaid;
                }
                if (i == periodInMonth)
                {
                    principalPaid = beginingBalance;
                    lastBalance = 0;

                }
                paymentDate =DateTime.Now.AddMonths(i);
                ScheduleLine line = new ScheduleLine()
                {
                    InstallmentNo = i,
                    PaymentDate = paymentDate,
                    Principal=principalPaid,
                    Interest=interest,
                    TotalPay=totalPay,
                    OutStanding=lastBalance
                };
                loanScheduleLines.Add(line);
            }

            return loanScheduleLines;
        }
    }
}
