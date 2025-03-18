using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class StLoanTermConfiguration : IEntityTypeConfiguration<LoanTermType>
    {
        public void Configure(EntityTypeBuilder<LoanTermType> builder)
        {

            builder.ToTable("st_loanTerms");
            builder.HasKey(x => x.LoanTermId);
            builder.HasData(new LoanTermType[]
            {
                new LoanTermType
                {
                    LoanTermId=1,
                    LoanTermCode="Annurity",
                    Description="Anuality"
                },
                new LoanTermType
                {
                    LoanTermId=2,
                    LoanTermCode="Balloon",
                    Description="Balloon"
                },
                new LoanTermType
                {
                    LoanTermId=3,
                    LoanTermCode="Decline",
                    Description="Decline"
                },
                new LoanTermType
                {
                    LoanTermId=4,
                    LoanTermCode="Flat",
                    Description="Flat"
                },
            });
        }
    }
}
