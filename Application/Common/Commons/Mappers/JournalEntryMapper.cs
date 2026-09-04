using Domains.Models.Transaction;

namespace Application.Commons.Mappers;

public static class JournalEntryMapper
{
    public static LoanTransaction MapJournalTransactionCreate(this JournalTransactionDtoCreate dto)
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

    public static LoanTransaction MapJournalTransactionUpdate(this JournalTransactionDtoUpdate dto)
    {
        return new LoanTransaction
        {

            TransactionsId = $"trx-{Guid.CreateVersion7()}",
            TransactionDate = DateTimeOffset.UtcNow,
            UserRefId = dto.UserRefId,
            TransactionTypeName = dto.TransactionTypeName,
            BranchRefId = dto.BranchRefId,
            TranTypeRefId = dto.TranTypeRefId,
            JournalEntry = new JournalEntry
            {

            },
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

    public static JournalTransactionDtoUpdate MapTransactionUpdate(this LoanTransaction source)
    {
        return new JournalTransactionDtoUpdate
        {

            TransactionsId = source.TransactionsId,
            TransactionDate = source.TransactionDate,
            UserRefId = source.UserRefId,
            TransactionTypeName = source.TransactionTypeName,
            BranchRefId = source.BranchRefId,
            TranTypeRefId = source.TranTypeRefId,
            JournalEntry = new JournalEntryDtoUpdate
            {
                JournalEntryId = source.JournalEntry!.JournalEntryId,
                BankRefId = source.JournalEntry!.BankRefId,
                BranchRefId = source.JournalEntry!.BranchRefId,
                CurrencyRefId = source.JournalEntry!.CurrencyRefId,
                JournalDesc = source.JournalEntry!.JournalDesc,
                JournalDate = source.JournalEntry!.JournalDate,
                IsAdjustment = source.JournalEntry!.IsAdjustment!.Value,
                JournalType = source.JournalEntry!.JournalType,
                ReferrenceNum = source.JournalEntry!.ReferrenceNum,
                JournalEntryLines = source.JournalEntry!.JournalEntryLines!.Select(x => new JournalLineDtoUpdate
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

    public static JournalTransactionDtoUpdate MapToJournalEntryDtoUpdate(this LoanTransaction source)
    {
        var journalEntry = source.JournalEntry;

        return new JournalTransactionDtoUpdate
        {
            TransactionsId = source.TransactionsId,
            TranTypeRefId = source.TranTypeRefId,
            TransactionName = source.TransactionTypeName ?? string.Empty,
            UserRefId = source.UserRefId,
            CurrencyRefId = journalEntry?.CurrencyRefId,
            JournalEntry = new JournalEntryDtoUpdate
            {
                JournalEntryId = journalEntry?.JournalEntryId,
                BankRefId = journalEntry?.BankRefId,
                BranchRefId = journalEntry?.BranchRefId,
                CurrencyRefId = journalEntry?.CurrencyRefId,
                JournalDesc = journalEntry?.JournalDesc ?? string.Empty,
                JournalDate = journalEntry?.JournalDate,
                IsAdjustment = journalEntry?.IsAdjustment ?? false,
                JournalType = journalEntry?.JournalType,
                ReferrenceNum = journalEntry?.ReferrenceNum,
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
                }).ToList() ?? new List<JournalLineDtoUpdate>()
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

    public static JournalEntry MapJournalUpdate(this JournalEntryDtoUpdate dto)
    {
        return new JournalEntry
        {
            JournalEntryId = dto.JournalId,
            BankRefId = dto.BankRefId,
            BranchRefId = dto.BranchRefId,
            CurrencyRefId = dto.CurrencyRefId,
            JournalDesc = dto.JournalDesc ?? string.Empty,
            JournalDate = dto.JournalDate,
            JournalLineCount = dto.JournalLineCount,
            IsAdjustment = dto.IsAdjustment,
            JournalType = dto.JournalType ?? string.Empty,
            TotalCr = dto.TotalCr,
            TotalDr = dto.TotalDr,
            ReferrenceNum = dto.ReferrenceNum,
            JournalEntryLines = dto.JournalEntryLines?.Select(x => x.MapJournalLineUpate()).ToList()
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
            LedgerName = dto.LedgerName ?? string.Empty,
            EntityNameRefId = dto.EntityNameRefId,
            EntityName = dto.EntityName,
            LineSeq = dto.LineSeq,
        };
    }

    public static JournalEntryLine MapJournalLineUpate(this JournalLineDtoUpdate dto)
    {
        return new JournalEntryLine
        {

            BaseAmount = dto.BaseAmount,
            Credit = dto.Credit,
            Debit = dto.Debit,
            JournalLineDesc = dto.JournalLineDesc,
            LedgerRefId = dto.LedgerRefId,
            LedgerName = dto.LedgerName ?? string.Empty,
            EntityNameRefId = dto.EntityNameRefId,
            EntityName = dto.EntityName,
            LineSeq = dto.LineSeq,
        };
    }

}
