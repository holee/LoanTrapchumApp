//using Infrastructures.DataAccess;
//using Microsoft.EntityFrameworkCore;

//public class VLoanSchedulesQuery
//{
//    private readonly ApplicationDbContext _context;

//    public VLoanSchedulesQuery(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public IQueryable<VLoanScheduleDto> GetLoanSchedules()
//    {
//        var loanSchedulePayments = _context.Set<LoanPaymentLine>()
//            .GroupBy(rl => new 
//            { 
//                rl.LoanRefId, 
//                rl.BranchRefId, 
//                rl.InstallmentNo, 
//                rl.LoanScheduleRefId 
//            })
//            .Select(g => new
//            {
//                g.Key.LoanRefId,
//                g.Key.BranchRefId,
//                g.Key.InstallmentNo,
//                g.Key.LoanScheduleRefId,
//                PaidPrinciple = g.Sum(x => x.PaidPrinciple),
//                PaidInterest = g.Sum(x => x.PaidInterest),
//                PaidCollectionFee = g.Sum(x => x.PaidCollectionFee),
//                PenaltyAmount = g.Sum(x => x.PenaltyAmount)
//            });

//        var query = from s in _context.Set<LoanSchedule>()
//                   join st in _context.Set<LoanScheduleStatus>() 
//                       on s.ScheduleStatusRefId equals st.LoanScheduleStatusId
//                   join d in _context.Set<LoanDisbursement>() 
//                       on s.LoanRefId equals d.LoanId
//                   join e in _context.Set<CustomerList>() 
//                       on d.CustomerRefId equals e.CustomerId
//                   join c in _context.Set<EmployeeList>() 
//                       on d.CreditAgentRefId equals c.EmployeeId
//                   join pay in loanSchedulePayments 
//                       on new { s.LoanRefId, s.BranchId, LoanScheduleId = s.LoanScheduleId } 
//                       equals new { pay.LoanRefId, pay.BranchRefId, LoanScheduleId = pay.LoanScheduleRefId } 
//                       into payments
//                   from p in payments.DefaultIfEmpty()
//                   select new VLoanScheduleDto
//                   {
//                       LoanScheduleId = s.LoanScheduleId,
//                       BranchId = d.BranchRefId,
//                       CreditAgentId = d.CreditAgentRefId,
//                       CreditAgentNameKh = c.AltName,
//                       CreditAgentNameEn = $"{c.FirstName} {c.LastName}",
//                       CurrencyId = d.CurrencyRefId,
//                       LoanId = s.LoanRefId,
//                       CustomerId = e.CustomerId,
//                       FullName = $"{e.FirstName} {e.LastName}",
//                       AltName = e.AltName,
//                       InstallmentNo = s.InstallmentNo,
//                       PaymentDate = s.PaymentDate,
//                       TotalDay = s.TotalDay ?? 0,
//                       Principle = s.Principal ?? 0,
//                       Interest = s.Interest ?? 0,
//                       CollectionFee = s.CollectionFee ?? 0,
//                       PenaltyRate = d.PenaltyRate ?? 0,
//                       TotalPay = s.TotalPay ?? 0,
//                       LoanOutstanding = s.LoanOutstanding ?? 0,
//                       PaidPrinciple = p.PaidPrinciple ?? 0,
//                       PaidInterest = p.PaidInterest ?? 0,
//                       PaidCollectionFee = p.PaidCollectionFee ?? 0,
//                       IsLate = EF.Functions.DateDiffDay(s.PaymentDate, DateTime.Now) > 0,
//                       DayArrea = Math.Max(0, EF.Functions.DateDiffDay(s.PaymentDate, DateTime.Now) ?? 0),
//                       PenaltyAmount = s.ScheduleStatusRefId == 1 ? 0 : 
//                           ((((s.Interest / 100m) / 28m) * s.TotalPay) * 2m) * 
//                           Math.Max(0, EF.Functions.DateDiffDay(s.PaymentDate, DateTime.Now) ?? 0),
//                       ScheduleStatusId = s.ScheduleStatusRefId,
//                       ScheduleStatus = st.LoanScheduleStatusCode
//                   };

//        return query;
//    }
//}

//public class VLoanScheduleDto
//{
//    public int LoanScheduleId { get; set; }
//    public int BranchId { get; set; }
//    public int CreditAgentId { get; set; }
//    public string CreditAgentNameKh { get; set; }
//    public string CreditAgentNameEn { get; set; }
//    public int CurrencyId { get; set; }
//    public int LoanId { get; set; }
//    public int CustomerId { get; set; }
//    public string FullName { get; set; }
//    public string AltName { get; set; }
//    public int InstallmentNo { get; set; }
//    public DateTime PaymentDate { get; set; }
//    public int TotalDay { get; set; }
//    public decimal Principle { get; set; }
//    public decimal Interest { get; set; }
//    public decimal CollectionFee { get; set; }
//    public decimal PenaltyRate { get; set; }
//    public decimal TotalPay { get; set; }
//    public decimal LoanOutstanding { get; set; }
//    public decimal PaidPrinciple { get; set; }
//    public decimal PaidInterest { get; set; }
//    public decimal PaidCollectionFee { get; set; }
//    public bool IsLate { get; set; }
//    public int DayArrea { get; set; }
//    public decimal PenaltyAmount { get; set; }
//    public decimal PaidAmount => PaidPrinciple + PaidInterest + PaidCollectionFee;
//    public decimal LeftInterestAmount => Interest - PaidInterest;
//    public decimal LeftCollectionFeeAmount => CollectionFee - PaidCollectionFee;
//    public decimal LeftPrinciple => Principle - PaidPrinciple;
//    public decimal LeftAmount => TotalPay - (PaidPrinciple + PaidInterest + PaidCollectionFee);
//    public int ScheduleStatusId { get; set; }
//    public string ScheduleStatus { get; set; }
//}
