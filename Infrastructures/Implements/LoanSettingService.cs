using Applications.Interfaces.Loans;
using Applications.Responses.Auths;
using Applications.Responses.ViewModels.Auths;
using Domains.Models.Loans;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public sealed class LoanSettingService : ILoanSettingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDapperConnectionFactory _db;
        public LoanSettingService(ApplicationDbContext context, IDapperConnectionFactory db)
        {
            _context = context;
            _db = db;
        }
        public async Task<IEnumerable<LoanSubTypeStatus>> GetSubSubTypeStatus(int? subTypeRefId = null, string? q = null)
        {
            IQueryable<LoanSubTypeStatus> queries = _context.LoanSubTypeNoAcc;

            if (subTypeRefId.HasValue && subTypeRefId > 0)
            {
                queries = queries.Where(x => x.LoanTypeStatusRefId == subTypeRefId.Value);
            }
            if (!string.IsNullOrEmpty(q))
            {
                queries = queries.Where(x => x.Description != null && x.Description.Contains(q));
            }

            return await queries.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LoanTypeStatus>> GetSubTypeStatus(string? q = null)
        {
            IQueryable<LoanTypeStatus> queries = _context.LoanSubTypeStatuses;
            if (!string.IsNullOrEmpty(q))
            {
                queries = queries.Where(x => x.Description != null && x.Description.Contains(q));
            }

            return await queries.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LoanRepaymentMode>> GetRepaymentModes(string? q = null)
        {
            IQueryable<LoanRepaymentMode> query = _context.LoanRepaymentModes;

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.RepaymentModeCode != null && x.RepaymentModeCode.Contains(q) ||
                                  x.Description != null && x.Description.Contains(q));
            }

            return await query.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LoanInstallmentType>> GetInstallmentTypes(string? q = null)
        {
            IQueryable<LoanInstallmentType> query = _context.LoanInstallmentTypes;

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.InstallmentTypeCode != null && x.InstallmentTypeCode.Contains(q) ||
                                  x.Description != null && x.Description.Contains(q));
            }

            return await query.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LoanCategory>> GetLoanCategories(string? q = null)
        {
            IQueryable<LoanCategory> query = _context.LoanCategories.Where(x => x.IsActive == true);

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.LoancategoryCode != null && x.LoancategoryCode.Contains(q) ||
                                  x.Code != null && x.Code.Contains(q));
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<AprovePerson>> GetAprovedUserAsync(int? branchId, string? filter = null)
        {
            IQueryable<VUser> queries = _context.Database
                                                .SqlQuery<VUser>($"SELECT * FROM vUsers WHERE BranchId={branchId} AND IsActive=CAST(1 AS BIT)");

            if (!string.IsNullOrEmpty(filter))
            {
                queries = queries.Where(x => EF.Functions.Like(x.UserName!, $"%{filter}%") ||
                                             EF.Functions.Like(x.FirstName!, $"%{filter}%") ||
                                             EF.Functions.Like(x.LastName!, $"%{filter}%"));
            }

            var aprovedPersons = await queries
                                              .ProjectToType<AprovePerson>()
                                              .AsNoTracking()
                                              .ToListAsync();

            return aprovedPersons;

        }
    }

}



//public async Task<VLoan?> FindLoanScheduleById(long loanId)
//{
//    IQueryable<VLoan> queries = cxt.LoanDisbursments
//                                            .Select(x => new VLoan
//                                            {
//                                                Id = x.Id,
//                                                AgentRefId = x.CreditAgentRefId,
//                                                AgentName = $"{x.LoanCreditAgent!.Employee!.FirstName} {x.LoanCreditAgent!.Employee!.LastName}",
//                                                AgentPhone = $"{x.LoanCreditAgent!.Employee.Phone}",
//                                                CustomerId = x.CustomerRefId,
//                                                CustomerName = $"{x.Customer!.EntityName!.FirstName} {x.Customer!.EntityName!.LastName}",
//                                                CustomerPhone = x.Customer.EntityName!.Phone,
//                                                Address = $"{x.Customer.EntityName!.Address!.ProInKhmer} {x.Customer.EntityName!.Address!.ProInKhmer} {x.Customer.EntityName!.Address!.ProInKhmer} {x.Customer.EntityName!.Address!.ProInKhmer}",
//                                                AccountNumber = x.AccountNumber,
//                                                ContractId = x.ContractNumber,
//                                                StartDate = x.StartDate,
//                                                LastDate = DateTime.Now,
//                                                Amount = x.Principle,
//                                                Period = x.Installments,
//                                                AdminFee = x.AdminFee,
//                                                Circle = 1,
//                                                InterestRate = x.InterestRate,
//                                                Schedules = x.LoanSchedules!.Select(a => new VScheduleVM
//                                                {
//                                                    ScheduleId = a.LoanScheduleId,
//                                                    No = a.InstallmentNo,
//                                                    TotalDays = a.TotalDay,
//                                                    PaymentDate = a.PaymentDate,
//                                                    PaidPrincipal = a.Principal,
//                                                    PaidInterest = a.Interest,
//                                                    PaidAdminFee = a.CollectionFee,
//                                                    PaidAmount = a.TotalPay,
//                                                    OutStanding = a.LoanOutstanding
//                                                }).ToList(),

//                                            });

//    return await queries.AsNoTracking().FirstOrDefaultAsync(x => x.Id == loanId);
//}

//var collateralLineIds = model.CollateralLines!.Select(x => x.Id).ToList();

//var collateralLines = cxt.CollateralLines
//                            .Where(c => collateralLineIds.Contains(c.Id))
//                            .ToList();
//var guarantors = model.Guarantors!;
//var schedules = model.LoanSchedules;
//var disbur = ToDisbursment(model);
//disbur.CollateralLines!.AddRange(collateralLines);
//disbur.Guarantors!.AddRange(guarantors);
//disbur.LoanSchedules!.AddRange(schedules!);
//cxt.LoanDisbursments.Add(model);
//var rowAffect = await cxt.SaveChangesAsync();
//var updatedRow = cxt.Customers.Where(x => x.CustomerId == model.CustomerRefId)
//                 .ExecuteUpdate(p => p.SetProperty(x => x.IsLoanActive, true));

//var tranx = new LoanTransaction
//{
//    UserRefID = model.UserRefId,
//    TransactionTypeName = "Loan Disbursment",
//    TranTypeRefId = 19,
//    IsActive = true
//};
//var totalPrinciplePayment = model.Principle - model.AdminFeeAmount;
//var cashMgt = new CashManagement
//{
//    BankRefId = 11,
//    BranchRefId = model.BranchRefId,
//    Amount = totalPrinciplePayment,
//    CurrencyRefId = model.CurrencyRefId,
//    CashStatusRefId = 2,
//    Remark = "Cash goes through transaction",
//    StaffRefId = model.UserRefId,
//    Receive = "N",
//    Reject = "N",
//    TransactionDate = model.RegisterDate,
//    BusinessDate = model.RegisterDate,
//    Balance = 0
//};
//tranx.CashManagements!.Add(cashMgt);
//cxt.LoanTransaction.Add(tranx);
//await cxt.SaveChangesAsync();

//public async Task<long> CreateLoanAsync(LoanDisbursement disbursement)
//{
//    try
//    {
//        cxt.Database.BeginTransaction();
//        var insertedId = cxt.LoanDisbursments.Add(disbursement).Entity;
//        var rowAffect = await cxt.SaveChangesAsync();
//        var updatedRow = cxt.Customers.Where(x => x.CustomerId == disbursement.CustomerRefId)
//                         .ExecuteUpdate(p => p.SetProperty(x => x.IsLoanActive, true));
//        cxt.Database.CommitTransaction();
//        return disbursement.Id;
//    }
//    catch (Exception ex)
//    {

//        await Console.Out.WriteLineAsync(ex.Message);
//        cxt.Database.RollbackTransaction();
//        return default;

//    }

//}


//var existingLoan = await _context.VLoanDisbursements
//                                        .Where(x => x.Id == loanId && x.BranchId == branchId)
//                                        .AsNoTracking()
//                                        .FirstOrDefaultAsync();

//var existingSchedules = await _context.Database
//                           .SqlQuery<VSchedule>($"SELECT * FROM VSchedules v WHERE v.LoanId={loanId} AND v.BranchId={branchId}")
//                           .AsNoTracking()
//                           .ToListAsync();

//var existingPayments = await _context.Database
//                        .SqlQuery<VLoanRepayment>($"SELECT * FROM VLoanRepayment v WHERE v.LoanRefId={loanId};")
//                        .AsNoTracking()
//                        .ToListAsync();