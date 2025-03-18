using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class StLoanPaymentLocationConfig : IEntityTypeConfiguration<LoanPaymentLocation>
    {
        public void Configure(EntityTypeBuilder<LoanPaymentLocation> builder)
        {
            builder.ToTable("st_loanPaymentLocations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Location).HasMaxLength(100);
        }
    }
}
