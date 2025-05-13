using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Holidays
{
    public class LtHolidayConfig : IEntityTypeConfiguration<Holiday>
    {
        public void Configure(EntityTypeBuilder<Holiday> builder)
        {
            builder.ToTable("lt_holidays");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.HolidayDt).IsUnique();
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.HolidayDt).HasColumnType("DATE");
            builder.Property(x => x.Status).HasDefaultValue(true);
            builder.Property(x => x.Description).HasDefaultValue(null);
            builder.HasQueryFilter(x => x.Status == true);
        }
    }
}
