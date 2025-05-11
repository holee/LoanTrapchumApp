using Applications.Responses.ViewModels.Journals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Journals;

public class VJournalConfiguration : IEntityTypeConfiguration<JournalEntryVm>
{
    void IEntityTypeConfiguration<JournalEntryVm>.Configure(EntityTypeBuilder<JournalEntryVm> builder)
    {
        builder.ToView("VJournals");
        builder.HasNoKey();
        builder.Property(x => x.TotalDr).HasPrecision(18, 2);
        builder.Property(x => x.TotalCr).HasPrecision(18, 2);
        builder.Property(x => x.Description).HasMaxLength(300);
    }
}
