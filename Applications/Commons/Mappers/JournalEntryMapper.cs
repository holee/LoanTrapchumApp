using Applications.Requests.Transactions.Journals;
using Domains.Models.Journals;
using Domains.Models.Transaction;

namespace Applications.Commons.Mappers;

public static class JournalEntryMapper
{
    public static LoanTransaction MapTransactionCreate(this JournalTransactionDtoCreate dto)
    {
        return new LoanTransaction
        {
            
            TransactionsId = $"trx-{Guid.CreateVersion7()}",
            TransactionDate = DateTime.Now,
            UserRefId = dto.UserRefId,
            TransactionTypeName = dto.TransactionTypeName,
            BranchRefId = dto.BranchRefId,
            TranTypeRefId = dto.TranTypeRefId,
            JournalEntry=dto.JournalEntry.MapJournalCreate(),
        };
    }

    public static JournalEntry MapJournalCreate(this JournalEntryDtoCreate dto)
    {
        return new JournalEntry
        {
            JournalEntryId = $"jrn-{Guid.CreateVersion7()}",
            BankRefId = dto.BankRefId,
            BranchRefId = dto.BranchRefId,
            CurrencyRefId = dto.CurrencyRefId,
            JournalDesc = dto.JournalDesc,
            JournalDate = dto.JournalDate,
            JournalLineCount = dto.JournalLineCount,
            IsAdjustment = dto.IsAdjustment,
            JournalType = dto.JournalType,
            TotalCr = dto.TotalCr,
            TotalDr = dto.TotalDr,
            ReferrenceNum = dto.ReferrenceNum,
            JournalEntryLines=dto.JournalEntryLines?.Select(x=>x.MapJournalLineCreate()).ToList()
        };
    }


    public static JournalEntryLine MapJournalLineCreate(this JournalLineDtoCreate dto)
    {
        return new JournalEntryLine
        {
             
            BaseAmount = dto.BaseAmount,
            Credit = dto.Credit,
            Debit = dto.Debit,
            JournalLineDesc = dto.JournalLineDesc,
            LedgerRefId = dto.LedgerRefId,
            LedgerName = dto.LedgerName,
            EntityNameRefId = dto.EntityNameRefId,
            EntityName = dto.EntityName,
            LineSeq = dto.LineSeq,
        };
    }



}
