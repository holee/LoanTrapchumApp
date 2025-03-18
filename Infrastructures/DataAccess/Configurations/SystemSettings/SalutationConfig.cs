using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.SystemSettings
{
    public class SalutationConfig : IEntityTypeConfiguration<Salutation>
    {
        public void Configure(EntityTypeBuilder<Salutation> builder)
        {
            builder.ToTable("lt_salutations");
            builder.HasKey(k => k.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).HasMaxLength(50);
            builder.HasIndex(x => x.Title).IsUnique();
        }
    }
}
