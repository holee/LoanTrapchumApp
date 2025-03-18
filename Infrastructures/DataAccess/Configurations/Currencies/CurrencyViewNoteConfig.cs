using Applications.Responses.ViewModels.Currencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Currencies
{
    public class CurrencyViewNoteConfig : IEntityTypeConfiguration<VCurrencyNotes>
    {
        public void Configure(EntityTypeBuilder<VCurrencyNotes> builder)
        {
            builder.ToView("VCurrencyNotes");
            builder.HasNoKey();
            builder.Property(p => p.CurrencyValue).HasPrecision(18,2);
            builder.Property(p => p.CurrencyName).HasMaxLength(100);
            builder.HasQueryFilter(x=>x.IsActive==true);
        }
    }
}
