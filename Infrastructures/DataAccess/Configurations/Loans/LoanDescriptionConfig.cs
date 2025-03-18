using Domains.Models.Loans;
using Infrastructures.DataAccess.Conversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class LoanDescriptionConfig : IEntityTypeConfiguration<LoanDescriptions>
    {
        public void Configure(EntityTypeBuilder<LoanDescriptions> builder)
        {
            builder.ToTable("lt_loanDescriptions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                    .ValueGeneratedOnAdd();
            builder.Property(x => x.Memo)
                    .HasMaxLength(400);
            builder.Property(x => x.CreatedDt)
                    .HasColumnName("CreatedDT")
                    .HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.LoanRequestRefId)
                 .HasConversion(ConversionExts.GuidConversion);
        }
    }
}
