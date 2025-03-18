using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.LoanDisbursements
{
    public class LoanInstallmentTypeConfigurate : IEntityTypeConfiguration<LoanInstallmentType>
    {
        public void Configure(EntityTypeBuilder<LoanInstallmentType> builder)
        {
            builder.ToTable("st_LoanInstallmentTypes");
            builder.HasKey(x => x.InstallmentTypeId);
            builder.Property(x => x.InstallmentTypeCode).HasMaxLength(20).IsRequired(false);
            builder.Property(x => x.Description).HasMaxLength(100).IsRequired(false);
            builder.HasData(Create(1, "Monthly", "Pay every month"));
            builder.HasData(Create(2, "2 Weekly", "Pay every 2 weeks"));
            builder.HasData(Create(3, "Weekly", "Pay every week"));
            builder.HasData(Create(4, "Daily", "Pay everyday"));
        }

        private LoanInstallmentType Create(int id, string code, string description)
        {
            return new LoanInstallmentType
            {
                InstallmentTypeId = id,
                InstallmentTypeCode = code,
                Description = description
            };
        }


    }
}
