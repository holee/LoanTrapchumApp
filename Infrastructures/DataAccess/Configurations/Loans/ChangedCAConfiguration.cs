using Domains.Models.Lists.CreditAgents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class ChangedCAConfiguration : IEntityTypeConfiguration<LoanChangedCa>
    {
        public void Configure(EntityTypeBuilder<LoanChangedCa> builder)
        {
            builder.ToTable("lt_LoanChangeCA");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.MonetaDate).HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.WindowDate).HasDefaultValueSql("GETDATE()");
        }
    }
}
