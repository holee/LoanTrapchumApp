using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.SystemSettings
{
    public class OtherTypeConfig : IEntityTypeConfiguration<OtherType>
    {
        public void Configure(EntityTypeBuilder<OtherType> builder)
        {
            builder.ToTable("st_OtherTypes");
            builder.HasKey(x => x.OtherTypeId);
            builder.Property(x => x.OtherTypeId).ValueGeneratedOnAdd();
        }
    }
}
