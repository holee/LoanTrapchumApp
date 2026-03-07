using Applications.Responses.ViewModels.Holidays;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Holidays;

public class VHolidayConfig : IEntityTypeConfiguration<HolidayVm>
{
    public void Configure(EntityTypeBuilder<HolidayVm> builder)
    {
        builder.ToView("VHolidaysVm");
        builder.HasKey(x => new { x.Id, x.HolidayDt });
        builder.Property(x => x.HolidayDt).HasColumnType("DATE");
        builder.Property(x => x.Status).HasDefaultValue(true);
        builder.Property(x => x.Description).HasDefaultValue(null);
        builder.HasQueryFilter(nameof(HolidayVm.Status), x => x.Status == true);
        builder.HasQueryFilter(nameof(HolidayVm.HolidayDt), x => x.HolidayDt >= DateTime.Today);
    }
}