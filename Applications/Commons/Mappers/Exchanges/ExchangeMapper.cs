using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.ForiegnExchanges;
using Domains.Commons.Entities;
using Domains.Models.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Commons.Mappers.Exchanges
{
    public static class ExchangeMapper
    {
        public static ExchangeDtoEdit ToDtoEdit(this VttCurrencyExchanges entity)
        {
            return new ExchangeDtoEdit
            {
                Id = entity!.Id,
                TransactionId = entity?.TransactionsId,
                TransactionDate = entity?.TransactionDate,
                BuyAmount = entity?.BuyAmount,
                SellAmount = entity?.SellAmount,
                SellCurrencyId = entity?.SellCurrencyRefId ?? 0,
                BuyCurrencyId = entity?.BuyCurrencyRefId ?? 0,
                ExchangeRate = entity?.ExchangeRate,
                SpotRate = entity?.SpotRate,
                Description = entity?.Remark,
                IsBuy = entity!.IsBuy,
            };
        }

        public static ExchangeDtoEdit ToEdit(this ExchangeDtoEdit entity)
        {
            return new ExchangeDtoEdit
            {
                //TransactionId = entity?.TransactionsId,
                //TransactionDate = entity?.TransactionDate,
                //BuyAmount = entity?.BuyAmount,
                //SellAmount = entity?.SellAmount,
                //SellCurrencyId = entity?.SellCurrencyRefId ?? 0,
                //BuyCurrencyId = entity?.BuyCurrencyRefId ?? 0,
                //ExchangeRate = entity?.ExchangeRate,
                //SpotRate = entity?.SpotRate,
                //Remark = entity?.Remark
            };
        }



        public static ForeignExchange ToModel(this ForeignExchangeDtoCreate entity)
        {
            return new ForeignExchange
            {
                StaffRefId = entity?.StaffRefId,
                TransactionRefId = entity?.TranRefId,
                NameRefId = entity?.NameRefId,
                StaffAuthRefId = entity?.StaffAuthRefId,
                BankRefId = entity?.BankRefId,
                BranchRefId = entity?.BranchRefId,
                BuyCurrencyRefId = entity?.BuyCurrencyRefId,
                BuyAmount = entity?.BuyAmount,
                SellAmount = entity?.SellAmount,
                SellCurrencyRefId = entity?.SellCurrencyRefId,
                ExchangeRate = entity?.ExchangeRate,
                SpotRate = entity?.SpotRate,
                GainAndLostLedgerRefId = entity?.GainAndLostLedgerRefId,
                GainAndLostAmount = entity?.GainAndLostAmount,
                FXEPALedgerRefId = entity?.FxepaLedgerRefId,
                FXEPA_Amount = entity?.FXEPA_Amount,
                FXPALedgerRefId = entity?.FxpaLedgerRefId,
                FXPA_Amount = entity?.FXPA_Amount,
                Remark = entity?.Remark,
                SellLedgerRefId = entity?.SellLedgerRefId,
                BuyLedgerRefId = entity?.BuyLedgerRefId,
                IsBuy = entity!.IsBuy,
                LoanTransaction = new LoanTransaction
                {
                    TransactionsId = entity?.TranRefId ?? $"trx-{Guid.CreateVersion7()}",
                    UserRefId = entity?.StaffRefId,
                    BranchRefId = entity?.BranchRefId,
                    TransactionTypeName = entity?.Transaction?.TransactionTypeName,
                    TranTypeRefId = entity?.Transaction?.TranTypeRefId, // This should be set to the appropriate transaction type reference ID
                    TransactionDate = entity?.Transaction?.TransactionDate ?? DateTimeOffset.UtcNow,
                }
            };
        }

        public static ForeignExchange ToModel(this ForeignExchangeDtoUpdate entity)
        {
            return new ForeignExchange
            {
                Id = entity.Id,
                StaffRefId = entity?.StaffRefId,
                TransactionRefId = entity?.TranRefId,
                NameRefId = entity?.NameRefId,
                StaffAuthRefId = entity?.StaffAuthRefId,
                BankRefId = entity?.BankRefId,
                BranchRefId = entity?.BranchRefId,
                BuyCurrencyRefId = entity?.BuyCurrencyRefId,
                BuyAmount = entity?.BuyAmount,
                SellAmount = entity?.SellAmount,
                SellCurrencyRefId = entity?.SellCurrencyRefId,
                ExchangeRate = entity?.ExchangeRate,
                SpotRate = entity?.SpotRate,
                GainAndLostLedgerRefId = entity?.GainAndLostLedgerRefId,
                GainAndLostAmount = entity?.GainAndLostAmount,
                FXEPALedgerRefId = entity?.FXEPALedgerRefId,
                FXEPA_Amount = entity?.FXEPA_Amount,
                FXPALedgerRefId = entity?.FXPALedgerRefId,
                FXPA_Amount = entity?.FXPA_Amount,
                Remark = entity?.Remark,
                SellLedgerRefId = entity?.SellLedgerRefId,
                BuyLedgerRefId = entity?.BuyLedgerRefId,
                LoanTransaction = new LoanTransaction
                {
                    TransactionsId = entity?.TranRefId ?? $"trx-{Guid.CreateVersion7()}",
                    UserRefId = entity?.StaffRefId,
                    BranchRefId = entity?.BranchRefId,
                    TransactionTypeName = entity?.Transaction?.TransactionTypeName,
                    TranTypeRefId = entity?.Transaction?.TranTypeRefId, // This should be set to the appropriate transaction type reference ID
                    TransactionDate = entity?.Transaction?.TransactionDate ?? DateTimeOffset.UtcNow,
                }
            };
        }

    }
}
