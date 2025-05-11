using Domains.Models.Transaction;
using Infrastructures.DataAccess.Conversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.CashManagements
{
    public class ForeignExchangeConfig : IEntityTypeConfiguration<ForeignExchange>
    {
        public void Configure(EntityTypeBuilder<ForeignExchange> builder)
        {
            builder.ToTable("tt_ForeignExchanges");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.SellAmount).HasPrecision(18,2);
            builder.Property(p => p.BuyAmount).HasPrecision(18,2);
            builder.Property(p => p.FXEPA_Amount).HasPrecision(18, 2);
            builder.Property(p => p.FXPA_Amount).HasPrecision(18, 2);
            builder.Property(p => p.GainAndLostAmount).HasPrecision(18, 2);
            builder.Property(p => p.SpotRate).HasPrecision(18, 2);
            builder.Property(p => p.Narration).HasMaxLength(400);
            builder.Property(p => p.ExchangeRate).HasPrecision(18,2);
            builder.Property(p => p.Remark).HasColumnType("NVARCHAR(400)");
            builder.Property(p => p.TransactionRefId).HasMaxLength(250);
        }
    }
}
