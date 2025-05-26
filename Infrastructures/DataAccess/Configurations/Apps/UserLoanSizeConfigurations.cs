using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Apps
{
    public class UserLoanSizeConfigurations : IEntityTypeConfiguration<UserLoanSize> 
    {
        public void Configure(EntityTypeBuilder<UserLoanSize> builder)
        {
            builder.ToTable("lt_LoanSizes");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.UserRefId).HasMaxLength(250);
            builder.Property(p => p.MaxUsdLoanSize).HasPrecision(18,3);
            builder.Property(p => p.MaxUsdLoanSize).HasPrecision(18,3);
            builder.Property(p => p.MaxKhrLoanSize).HasPrecision(18, 3);
            builder.Property(p => p.MinKhrLoanSize).HasPrecision(18, 3);
            builder.Property(p => p.Description).HasMaxLength(200);
            builder.Property(p => p.IsActive).HasDefaultValue(1);
            builder.Property(p => p.StartDate).HasColumnType("DATE");
            builder.Property(p => p.EndDate).HasColumnType("DATE");
            builder.HasQueryFilter(p => p.IsActive == true);
        }
    }
}
