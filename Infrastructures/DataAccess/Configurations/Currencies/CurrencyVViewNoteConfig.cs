using Applications.Features.Currencies.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Currencies;

public class CurrencyVViewNoteConfig : IEntityTypeConfiguration<CurrencyNoteViewDto>
{
    public void Configure(EntityTypeBuilder<CurrencyNoteViewDto> builder)
    {
        builder.ToView("vwCurrencyNotes");
        builder.HasNoKey();
        builder.Property(p => p.CurrencyValue).HasPrecision(18, 2);
        builder.Property(p => p.CurrencyName).HasMaxLength(100);
        builder.HasQueryFilter(x => x.IsActive == true);
    }
}
