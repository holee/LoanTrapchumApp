using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.SystemSettings
{
    public class StDayNameConfig : IEntityTypeConfiguration<DayName>
    {
        public void Configure(EntityTypeBuilder<DayName> builder)
        {
            builder.ToTable("st_daynames");
            builder.HasKey(k => k.DayInNumber);
            builder.Property(x => x.DayInNumber).ValueGeneratedOnAdd();
            builder.Property(x => x.DayEng).HasMaxLength(100);
            builder.Property(x => x.DayKhmer).HasMaxLength(100);

            ///Seed Data

            builder.HasData(new DayName[]
            {
                new DayName
                {
                    DayInNumber=1,
                    DayKhmer="អាទិត្យ",
                    DayEng="Sun"
                },
                new DayName
                {
                    DayInNumber=2,
                    DayKhmer="ច័ន្ទ",
                    DayEng="Mon"
                },
                new DayName
                {
                    DayInNumber=3,
                    DayKhmer="អង្គារ",
                    DayEng="Tue"
                },
                new DayName
                {
                    DayInNumber=4,
                    DayKhmer="ពុធ",
                    DayEng="Wed"
                },
                new DayName
                {
                    DayInNumber=5,
                    DayKhmer="ព្រហ",
                    DayEng="Thu"
                },
                new DayName
                {
                    DayInNumber=6,
                    DayKhmer="សុក្រ",
                    DayEng="Fri"
                },
                new DayName
                {
                    DayInNumber=7,
                    DayKhmer="សៅរ៍",
                    DayEng="Sat"
                }
            });
        }
    }
}
