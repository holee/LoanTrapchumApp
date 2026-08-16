using Domains.Models.Transaction;

namespace Applications.Commons.Mappers;

public static class JournalEntryMapper
{
    public static LoanTransaction MapTransactionCreate(this JournalTransactionDtoCreate dto)
    {
        return new LoanTransaction
        {

            TransactionsId = $"trx-{Guid.CreateVersion7()}",
            TransactionDate = DateTimeOffset.UtcNow,
            UserRefId = dto.UserRefId,
            TransactionTypeName = dto.TransactionTypeName,
            BranchRefId = dto.BranchRefId,
            TranTypeRefId = dto.TranTypeRefId,
            JournalEntry = dto.JournalEntry.MapJournalCreate(),
        };
    }

    public static LoanTransaction MapTransactionUpdate(this JournalTransactionDtoCreate dto)
    {
        return new LoanTransaction
        {

            TransactionsId = $"trx-{Guid.CreateVersion7()}",
            TransactionDate = DateTimeOffset.UtcNow,
            UserRefId = dto.UserRefId,
            TransactionTypeName = dto.TransactionTypeName,
            BranchRefId = dto.BranchRefId,
            TranTypeRefId = dto.TranTypeRefId,
            JournalEntry = dto.JournalEntry.MapJournalCreate(),
        };
    }

    public static JournalTransactionDtoCreate MapTransactionUpdate(this LoanTransaction source)
    {
        return new JournalTransactionDtoCreate
        {

            TransactionsId = source.TransactionsId,
            TransactionDate = source.TransactionDate,
            UserRefId = source.UserRefId,
            TransactionTypeName = source.TransactionTypeName,
            BranchRefId = source.BranchRefId,
            TranTypeRefId = source.TranTypeRefId,
            JournalEntry = new JournalEntryDtoCreate
            {
                JournalEntryId = source.JournalEntry!.JournalEntryId,
                BankRefId = source.JournalEntry!.BankRefId,
                BranchRefId = source.JournalEntry!.BranchRefId,
                CurrencyRefId = source.JournalEntry!.CurrencyRefId,
                JournalDesc = source.JournalEntry!.JournalDesc,
                JournalDate = source.JournalEntry!.JournalDate,
                //JournalLineCount = source.JournalEntry!.JournalLineCount!.Value,
                IsAdjustment = source.JournalEntry!.IsAdjustment!.Value,
                JournalType = source.JournalEntry!.JournalType,
                TotalCr = source.JournalEntry!.TotalCr,
                TotalDr = source.JournalEntry!.TotalDr,
                ReferrenceNum = source.JournalEntry!.ReferrenceNum,
                JournalEntryLines = source.JournalEntry!.JournalEntryLines!.Select(x => new JournalLineDtoCreate
                {
                    BaseAmount = x.BaseAmount,
                    Credit = x.Credit,
                    Debit = x.Debit,
                    JournalLineDesc = x.JournalLineDesc,
                    LedgerRefId = x.LedgerRefId,
                    LedgerName = x.LedgerName,
                    EntityNameRefId = x.EntityNameRefId,
                    EntityName = x.EntityName,
                    LineSeq = x.LineSeq!.Value
                }).ToList()
            }
        };
    }

    public static JournalEntryDtoUpdate MapToJournalEntryDtoUpdate(this LoanTransaction source)
    {
        var journalEntry = source.JournalEntry;

        return new JournalEntryDtoUpdate
        {
            JournalId = journalEntry?.JournalEntryId,
            JournalType = journalEntry?.JournalType,
            JournalDate = journalEntry?.JournalDate,
            IsAdjustment = journalEntry?.IsAdjustment ?? false,
            ReferrenceNum = journalEntry?.ReferrenceNum,
            JournalDesc = journalEntry?.JournalDesc ?? string.Empty,
            CurrencyRefId = journalEntry?.CurrencyRefId,
            TotalDr = journalEntry?.TotalDr,
            TotalCr = journalEntry?.TotalCr,
            JournalEntryLines = journalEntry?.JournalEntryLines?.Select(x => new JournalLineDtoUpdate
            {
                JournalLineId = (int)x.JournalLineId,
                JournalRefId = x.JournalRefId,
                EntityNameRefId = x.EntityNameRefId,
                EntityName = x.EntityName,
                LedgerRefId = x.LedgerRefId,
                LedgerName = x.LedgerName,
                Amount = x.Amount,
                Debit = x.Debit,
                Credit = x.Credit,
                ExchRate = x.ExchangeRate,
                BaseAmount = x.BaseAmount,
                JournalLineDesc = x.JournalLineDesc
            }).ToList() ?? new List<JournalLineDtoUpdate>(),
            LoanTransaction = new JournalTransactionDtoUpdate
            {
                TranId = source.TransactionsId,
                TranTypeRefId = source.TranTypeRefId,
                TransactionName = source.TransactionTypeName ?? string.Empty,
                UserRefId = source.UserRefId,
                CurrencyRefId = journalEntry?.CurrencyRefId
            }
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
            JournalEntryLines = dto.JournalEntryLines?.Select(x => x.MapJournalLineCreate()).ToList()
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
