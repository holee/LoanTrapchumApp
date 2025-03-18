using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.SystemSettings
{
    public class StMonthNameConfig : IEntityTypeConfiguration<MonthName>
    {
        public void Configure(EntityTypeBuilder<MonthName> builder)
        {
            builder.ToTable("st_monthnames");
            builder.HasKey(x => x.MonthId);
            builder.Property(x => x.MonthId).ValueGeneratedOnAdd();
            builder.Property(prop => prop.MonthId).ValueGeneratedOnAdd();
            builder.Property(prop => prop.MonthNames).HasMaxLength(100);
            builder.Property(prop => prop.MonthEng).HasMaxLength(100);
        }
    }
}
