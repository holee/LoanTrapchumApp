using Applications.Features.Journals.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Journals;

public class JournalEntryDtoConfiguration : IBaseEntityTypeConfigurations<JournalEntryDto>
{
    public void Configure(EntityTypeBuilder<JournalEntryDto> builder)
    {
        builder.ToView("VJournals");
        builder.HasNoKey();
        builder.Property(x => x.TotalDr).HasPrecision(18, 2);
        builder.Property(x => x.TotalCr).HasPrecision(18, 2);
        builder.Property(x => x.TotalBaseDr).HasPrecision(18, 2);
        builder.Property(x => x.TotalBaseCr).HasPrecision(18, 2);
        builder.Property(x => x.ExchangeRate).HasPrecision(18, 2);
        builder.Property(x => x.Description).HasMaxLength(300);
    }
}
