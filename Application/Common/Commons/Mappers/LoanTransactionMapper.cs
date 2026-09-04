using Application.Features.LoanPayments.Command.RecievePayment;
using Application.Features.LoanPayments.Requests;
using Application.Features.Transactions.LoanTransactions;
using Domains.Models.Loans;
using Domains.Models.Transaction;

namespace Application.Commons.Mappers;

public static class LoanTransactionMapper
{
    public static LoanTransaction ToLoanTransction(this LoanPaymentTransactionDtoCreate req, int? branchId)
    {
        if (req == null) return new();

        return new LoanTransaction
        {
            TransactionsId = req.TransactionsId,
            TranTypeRefId = req.TranTypeRefId,
            TransactionTypeName = req.TransactionTypeName,
            TransactionDate = req.TransactionDate,
            UserRefId = req.UserRefId,
            BranchRefId= branchId,
            LoanPayment = req.LoanPayment?.ToLoanPayment(req.BranchRefId),
            CashManagements = req.CashManagements!.ToCashManagements()
        };
    }


    public static LoanTransaction ToLoanTransction(this LoanPaymentTransactionDtoCreate req)
    {
        if (req == null) return new();

        return new LoanTransaction
        {
            TransactionsId = req.TransactionsId,
            TranTypeRefId = req.TranTypeRefId,
            TransactionTypeName = req.TransactionTypeName,
            TransactionDate = req.TransactionDate,
            UserRefId = req.UserRefId,
            BranchRefId = req.BranchRefId,
            LoanPayment = req.LoanPayment?.ToLoanPayment(req.BranchRefId),
            CashManagements = req.CashManagements!.ToCashManagements()
        };
    }

    public static LoanTransaction RecieveLoanTransction(this RecievePaymentCommand req)
    {
        return req.Request.ToLoanTransction(req.Request.BranchRefId);
    }

    public static LoanPayment ToLoanPayment(this LoanPaymentDtoCreate req)
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
            RecieveAmount= req.RecieveAmount,
            ChangeAmount = req.ChangeAmount,
            Principle = req.PaidPrinciple, //req.Principle
            Interest = req.PaidInterest,// req.Interest
            CollectionFeeAmount = req.PaidFeeAmount,// req.CollectionFeeAmount
            PenaltyAmount = req.PaidPenaltyAmount,// req.PenaltyAmount
            LoanRepaymentLines = req.LoanPaymentLines.Select(x => x.ToLoanPaymentLines(req.BranchRefId  )).ToList(),
        };
    }

    public static LoanPayment ToLoanPayment(this LoanPaymentDtoCreate req,int? branchId)
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
            RecieveAmount = req.RecieveAmount,
            ChangeAmount = req.ChangeAmount,
            PayDate = req.PayDate,
            Principle = req.PaidPrinciple, //req.Principle
            Interest = req.PaidInterest,// req.Interest
            CollectionFeeAmount = req.PaidFeeAmount,// req.CollectionFeeAmount
            PenaltyAmount = req.PaidPenaltyAmount,// req.PenaltyAmount
            LoanRepaymentLines = req.LoanPaymentLines.Select(x => x.ToLoanPaymentLines(req.BranchRefId)).ToList(),
        };
    }

    public static LoanPaymentLine ToLoanPaymentLines(this LoanPaymentLineDtoCreate x, int? branchId)
    {
        if (x == null) return new();

        return new LoanPaymentLine
        {
            LoanRefId = x.LoanRefId,
            LoanScheduleRefId = x.LoanScheduleRefId,
            InstallmentNo = x.InstallmentNo,
            PaidPrinciple = x.PaidPrinciple,
            PaidInterest = x.PaidInterest,
            BranchRefId = branchId,
            PaidCollectionFee = x.PaidCollectionFee,
            PenaltyAmount = x.PaidPenaltyAmount,
            OfficialReceiptNo = x.OfficialReceiptNo,
        };
    }

    public static List<CashManagement> ToCashManagements(this IEnumerable<TrxCashManagementDtoCreate> reg)
    {
        if (!reg.Any()) return new();

        return reg.Select(x => x.ToCashManagement()).ToList();
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
