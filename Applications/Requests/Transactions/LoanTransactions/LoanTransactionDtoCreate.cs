using Applications.Calculations;
using Applications.Requests.Loans.Payments;
using Domains.Models.Loans;
using Domains.Models.Transaction;

namespace Applications.Requests.Transactions.LoanTransactions;

public sealed class LoanTransactionDtoCreate
{
    public string TransactionsId { get; set; } = string.Empty;
    public int? TranTypeRefId { get; set; }
    public string TransactionTypeName { get; set; } = string.Empty;
    public DateTime? TransactionDate { get; set; } = DateTime.Now;
    public string? UserRefId { get; set; }
    public LoanRepaymentDtoCreate? LoanPayment { get; set; }
    public ICollection<TrxCashManagementDtoCreate>? CashManagements { get; set; } = [];
}

public static class LoanTransactionDtoCreateÉxtsions
{
    public static LoanTransaction ToLoanTransction(this LoanTransactionDtoCreate req)
    {
        if (req == null) return new();

        return new LoanTransaction
        {
            TransactionsId = req.TransactionsId,
            TranTypeRefId = req.TranTypeRefId,
            TransactionTypeName = req.TransactionTypeName,
            TransactionDate = req.TransactionDate,
            UserRefId = req.UserRefId,
            LoanPayment = req.LoanPayment?.ToLoanPayment(),
            CashManagements = req.CashManagements!.ToCashManagement()
        };
    }

    public static LoanPayment ToLoanPayment(this LoanRepaymentDtoCreate req)
    {
        if (req == null) return new();

        return new LoanPayment
        {
            TransactionRefId = req.TransactionRefId,
            LoanRefId = req.LoanRefId,
            CreditAgentRefId = req.CreditAgentRefId,
            UserRefId = req.UserRefId,
            BranchRefId = req.BranchRefId,
            CurrencyRefId = req.CurrencyRefId,
            OfficialReceiptNo = req.OfficialReceiptNo,
            PayDate = req.PayDate,
            Principle = req.Principle,
            Interest = req.Interest,
            CollectionFeeAmount = req.CollectionFeeAmount,
            PenaltyAmount = req.PenaltyAmount,
            SubTotal = req.SubTotal,
            LoanRepaymentLines = req.LoanRepaymentLines.Select(x => new LoanPaymentLine
            {
                LoanRefId = x.LoanRefId,
                InstallmentNo = x.InstallmentNo,
                PaidPrinciple = x.PaidPrinciple,
                PaidInterest = x.PaidInterest,
                PaidCollectionFee = x.PaidCollectionFee,
                PenaltyAmount = x.PenaltyAmount,
                SubTotal = x.SubTotal
            }).ToList(),
        };
    }

    public static LoanPaymentLine ToLoanPaymentLines(this LoanRepaymentLineDtoCreate x)
    {
        if (x == null) return new();

        return  new LoanPaymentLine
        {
            LoanRefId = x.LoanRefId,
            InstallmentNo = x.InstallmentNo,
            PaidPrinciple = x.PaidPrinciple,
            PaidInterest = x.PaidInterest,
            PaidCollectionFee = x.PaidCollectionFee,
            PenaltyAmount = x.PenaltyAmount,
            SubTotal = x.SubTotal
        };
    }

    public static List<CashManagement> ToCashManagement(this IEnumerable<TrxCashManagementDtoCreate> reg)
    {
        if (!reg.Any()) return new();

        return reg.Select(x => new CashManagement
        {
            Id = x.Id!,
            BankRefId = x.BankRefId,
            StaffRefId = x.StaffRefId,
            CurrencyRefId = x.CurrencyRefId,
            BranchRefId = x.BranchRefId,
            TransactionRefId = x.TranRefId,
            Amount = x.Amount,
            Balance = x.Balance,
            CashStatusRefId = x.CashStatusRefId,
            TransactionDate = x.TransactionDate,
            BusinessDate = x.BusinessDate,
            Reject = x.Reject,
            Receive = x.Receive,
            Remark = x.Remark
        }).ToList();
    }

    public static CashManagement ToCashManagement(this TrxCashManagementDtoCreate? x)
    {
        if (x is null) return new();

        return new CashManagement
        {
            Id = x.Id!,
            BankRefId = x.BankRefId,
            StaffRefId = x.StaffRefId,
            CurrencyRefId = x.CurrencyRefId,
            BranchRefId = x.BranchRefId,
            TransactionRefId = x.TranRefId,
            Amount = x.Amount,
            Balance = x.Balance,
            CashStatusRefId = x.CashStatusRefId,
            TransactionDate = x.TransactionDate,
            BusinessDate = x.BusinessDate,
            Reject = x.Reject,
            Receive = x.Receive,
            Remark = x.Remark
        };
    }

}
