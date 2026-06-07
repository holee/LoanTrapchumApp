using Applications.Features.Currencies.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructures.DataAccess.Configurations.Currencies
{
    public sealed class VttCurrencyExchangesConfig : IBaseEntityTypeConfigurations<VttCurrencyExchanges>
    {
        public void Configure(EntityTypeBuilder<VttCurrencyExchanges> builder)
        {
            builder.ToView("VTTExchangeTransactions");
            builder.HasNoKey();
            builder.Property(x => x.SellAmount).HasPrecision(18, 2);
            builder.Property(x => x.BuyAmount).HasPrecision(18, 2);
            builder.Property(x => x.ExchangeRate).HasPrecision(18, 2);
            builder.Property(x => x.SpotRate).HasPrecision(18, 2);
            builder.Property(x => x.TransactionDate).HasColumnType("datetimeoffset(7)");
        }
    }
}
