using Domains.Models.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Currencies
{
    public class StCashStatusConfig : IEntityTypeConfiguration<CashStatus>
    {
        public void Configure(EntityTypeBuilder<CashStatus> builder)
        {
            builder.ToTable("st_cashStatus");
            builder.HasKey(k => k.CashStatusId);
            builder.Property(x => x.CashStatusId).ValueGeneratedOnAdd();
            builder.Property(x => x.Code).HasMaxLength(250);
            builder.Property(x => x.Description);
        }
    }
}
