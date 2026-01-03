using Applications.Features.Currencies.Requests;
using Applications.Features.Currencies.Responses;
using Domains.Commons.Entities;
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
                TransactionId = entity?.TransactionsId,
                TransactionDate = entity?.TransactionDate,
                BuyAmount = entity?.BuyAmount,
                SellAmount = entity?.SellAmount,
                SellCurrencyId = entity?.SellCurrencyRefId ?? 0,
                BuyCurrencyId = entity?.BuyCurrencyRefId ?? 0,
                ExchangeRate = entity?.ExchangeRate,
                SpotRate = entity?.SpotRate,
                Remark = entity?.Remark
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
    }
}
