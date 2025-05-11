using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class LtLoanCategoryConfig : IEntityTypeConfiguration<LoanCategory>
    {
        public void Configure(EntityTypeBuilder<LoanCategory> builder)
        {
            builder.ToTable("lt_loanCategories");
            builder.HasKey(x => x.LoanCategoryId);
            builder.Property(prop => prop.LoanCategoryId).ValueGeneratedOnAdd();
            builder.Property(prop => prop.LoanCategoryCode).HasMaxLength(50);
            builder.Property(prop => prop.DesInKhmer).HasMaxLength(50);
            builder.Property(prop => prop.Description).HasMaxLength(50);
            builder.Property(prop => prop.Code).HasMaxLength(50);
            builder.Property(prop => prop.EditSeq).HasDefaultValue(0);
            builder.Property(prop => prop.IsActive).HasDefaultValue(true);
            builder.Property(prop => prop.CreatedDt).HasDefaultValueSql("GETDATE()");
            builder.Property(prop => prop.ModifiedDt).HasDefaultValueSql("GETDATE()");

            //builder.HasIndex(p=>p.LoancategoryCode).IsUnique(true);
        }
    }
}
