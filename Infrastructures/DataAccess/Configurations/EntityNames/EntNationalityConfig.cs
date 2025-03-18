using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.EntityNames
{
    public class EntNationalityConfig : IEntityTypeConfiguration<Nationality>
    {
        public void Configure(EntityTypeBuilder<Nationality> builder)
        {
            builder.ToTable("st_nationalities");
            builder.HasKey(x => x.NationalityId);
            builder.Property(x => x.NationalityId).ValueGeneratedOnAdd();
            builder.Property(prop => prop.NationalityCode).HasMaxLength(50);
            builder.Property(prop => prop.NationalityName).HasMaxLength(100);
        }
    }
}
