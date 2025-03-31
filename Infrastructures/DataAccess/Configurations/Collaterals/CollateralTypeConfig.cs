using Domains.Models.Collaterals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals
{
    public class CollateralTypeConfig : IEntityTypeConfiguration<CollateralType>
    {
        public void Configure(EntityTypeBuilder<CollateralType> builder)
        {
            builder.ToTable("st_collateralTypes");
            builder.HasKey(x => x.CollateralTypeId);
            builder.HasData(new CollateralType[]
            {
                new CollateralType
                {
                    CollateralTypeId=1,
                    CollateralTypeCode="Property",
                    Description="Property, Land, House, etc"
                },
                new CollateralType
                {
                    CollateralTypeId=2,
                    CollateralTypeCode="FD",
                    Description="Fixed Deposit"
                },
                new CollateralType
                {
                    CollateralTypeId=3,
                    CollateralTypeCode="Personal",
                    Description="Personal Guarantee"
                },
                new CollateralType
                {
                    CollateralTypeId=4,
                    CollateralTypeCode="Cheque",
                    Description="Cheque"
                },
                new CollateralType
                {
                    CollateralTypeId=5,
                    CollateralTypeCode="RemovableProperty",
                    Description="RemovableProperty, Job"
                }

            });
        }
    }
}
