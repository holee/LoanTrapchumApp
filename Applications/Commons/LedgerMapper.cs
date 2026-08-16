using Applications.Features.Ledgers.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Commons
{
    public static class LedgerMapper
    {
        public static Ledger MapFromLedgerDtoCreate(this LedgerCreateDto? source)
        {
            return new Ledger
            {
                LedgerName = source?.LedgerName,
                LedgerCode = source?.LedgerCode,
                BankNumber = source?.BankNumber,
                IsMainGl = source?.IsMainGl ?? false,
                IsConsolidate = source?.IsConsolidate ?? false,
                Description = source?.Description,
                ParentRefId = source?.ParentRefId,
                LedgerTypeRefId = source?.LedgerTypeRefId ?? 0,
                IsActive = source?.IsActive ?? true,
                CurrencyRefId = source?.CurrencyRefId ?? 0,
            };
        }

        public static Ledger MapFromLedgerDtoUpdate(this LedgerUpdateDto? source)
        {
            return new Ledger
            {
                LedgerId = source?.LedgerId ?? 0,
                LedgerName = source?.LedgerName,
                LedgerCode = source?.LedgerCode,
                BankNumber = source?.BankNumber,
                IsMainGl = source?.IsMainGl ?? false,
                IsConsolidate = source?.IsConsolidate ?? false,
                Description = source?.Description,
                ParentRefId = source?.ParentRefId,
                LedgerTypeRefId = source?.LedgerTypeRefId ?? 0,
                IsActive = source?.IsActive ?? true,
                CurrencyRefId = source?.CurrencyRefId ?? 0,
            };
        }


        public static VLedger MapToVLedger(this Ledger source)
        {
            return new VLedger
            {
                LedgerId = source.LedgerId,
                LedgerName = source.LedgerName,
                LedgerCode = source.LedgerCode,
                BankNumber = source.BankNumber,
                IsMainGl = source.IsMainGl,
                IsConsolidate = source.IsConsolidate,
                Description = source.Description,
                ParentRefId = source.ParentRefId,
                LedgerTypeId = source.LedgerTypeRefId,
                IsActive = source.IsActive,
                CurrencyId = source.CurrencyRefId,
            };
        }


    }
}
