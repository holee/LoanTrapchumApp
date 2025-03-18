using Domains.Models.Collaterals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals
{
    public class StCollateralPropertyTypeConfig : IEntityTypeConfiguration<LoanPropertyType>
    {
        public void Configure(EntityTypeBuilder<LoanPropertyType> builder)
        {

            builder.ToTable("st_collateralPropertyTypes");
            builder.HasKey(x => x.Id);

            builder.HasData(new LoanPropertyType[]
            {
                new LoanPropertyType
                {
                    Id=1 ,PropertyName="Soft Title"
                },
                new LoanPropertyType
                {
                    Id=2 ,PropertyName="Hard Title"
                },
                new LoanPropertyType
                {
                    Id=3 ,PropertyName="Other"
                },
                new LoanPropertyType
                {
                    Id=4 ,PropertyName="Agreement"
                },
                new LoanPropertyType
                {
                    Id=5 ,PropertyName="Birth Certificate"
                },
                new LoanPropertyType
                {
                    Id=6 ,PropertyName="Family Book"
                },
                new LoanPropertyType
                {
                    Id=7 ,PropertyName="ID Card"
                },
                new LoanPropertyType
                {
                    Id=8 ,PropertyName="Moto Registration"
                },
                new LoanPropertyType
                {
                    Id=9 ,PropertyName="Resident Book"
                },
                new LoanPropertyType
                {
                    Id=10 ,PropertyName="Car Registration"
                },
                new LoanPropertyType
                {
                    Id=11 ,PropertyName="Land Title"
                }

            });
        }
    }
}
