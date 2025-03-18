using Domains.Models.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.CashManagements
{
    public class TTCashStatusConfig : IEntityTypeConfiguration<CashStatus>
    {
        public void Configure(EntityTypeBuilder<CashStatus> builder)
        {
            builder.ToTable("st_cashStatus");
            builder.HasKey(k => k.CashStatusId);
            builder.Property(p => p.Code).HasMaxLength(10);
            builder.Property(p => p.Description).HasMaxLength(200);
        }
    }
}
