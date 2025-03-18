using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.LoanDisbursements
{
    public class LoanClarifiedStaffConfig : IEntityTypeConfiguration<LoanClarifyOfStaff>
    {
        public void Configure(EntityTypeBuilder<LoanClarifyOfStaff> builder)
        {
            builder.ToTable("st_LoanClassifiedstaffs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Description).HasMaxLength(50);

            builder.HasData(new LoanClarifyOfStaff[]
            {
                new LoanClarifyOfStaff
                {
                    Id=1,
                    Description="Manager"
                },
                new LoanClarifyOfStaff
                {
                    Id=2,
                    Description="Sub Manager"
                },
                new LoanClarifyOfStaff
                {
                    Id=3,
                    Description="Credit Assistant"
                },
                new LoanClarifyOfStaff
                {
                    Id=4,
                    Description="Credit Agent"
                },
            });
        }
    }
}