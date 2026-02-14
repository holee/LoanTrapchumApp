using Applications.Features.Journals.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Journals;

public class JournalEntryLineDtoConfiguration : IBaseEntityTypeConfigurations<JournalEntryLineDto>
{
    public void Configure(EntityTypeBuilder<JournalEntryLineDto> builder)
    {
        builder.ToView("VJournalEntryLines");
        builder.HasNoKey();
        builder.Property(x => x.Debit).HasPrecision(18, 2);
        builder.Property(x => x.Credit).HasPrecision(18, 2);
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.BaseAmount).HasPrecision(18, 2);
        builder.Property(x => x.ExchangeRate).HasPrecision(18, 2);
        builder.Property(x => x.JournalLineDesc).HasMaxLength(300);
    }
}