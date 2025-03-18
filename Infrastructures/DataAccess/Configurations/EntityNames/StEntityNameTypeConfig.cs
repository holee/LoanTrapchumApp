using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.EntityNames
{
    public class StEntityNameTypeConfig : IEntityTypeConfiguration<EntityNameType>
    {
        public void Configure(EntityTypeBuilder<EntityNameType> builder)
        {
            builder.ToTable("st_types");
            builder.HasKey(x => x.TypeId);
            builder.Property(x => x.TypeName).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.TypeOf).HasMaxLength(50).IsRequired(false);


            builder.HasData(new EntityNameType[]
            {
                new EntityNameType
                {
                    TypeId=1,
                    TypeName="Employee",
                    TypeOf="Employee"
                },
                new EntityNameType
                {
                    TypeId=2,
                    TypeName="Customer",
                    TypeOf="Customer"
                },
                new EntityNameType
                {
                    TypeId=3,
                    TypeName="Vendor",
                    TypeOf="Vendor"
                },
                new EntityNameType
                {
                    TypeId=4,
                    TypeName="Other Name",
                    TypeOf="Other Name"
                }
            });
        }
    }
}
