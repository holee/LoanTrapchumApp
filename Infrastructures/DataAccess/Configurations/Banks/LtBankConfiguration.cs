using Domains.Models.Currencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Banks
{
    public class LtBankConfiguration : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            builder.ToTable("lt_banks");
            builder.HasKey(pk => pk.BankId);
            builder.Property(x => x.BankId).ValueGeneratedOnAdd();
            builder.Property(x => x.BankName).HasMaxLength(100);
            builder.Property(x => x.BankCode).HasMaxLength(20);
            builder.Property(x => x.AltName).HasMaxLength(100);
            builder.Property(x => x.WebSite).HasMaxLength(200);
            builder.Property(x => x.Email).HasMaxLength(200);
            builder.Property(x => x.AltPhone).HasMaxLength(30);
            builder.Property(x => x.Country).HasMaxLength(100);
            builder.Property(x => x.ProvinceName).HasMaxLength(100);
            builder.Property(x => x.DistrictName).HasMaxLength(100);
            builder.Property(x => x.CommuneName).HasMaxLength(100);
            builder.Property(x => x.VillageName).HasMaxLength(100);
            builder.Property(x => x.IsClosed).HasDefaultValue(false);
            builder.Property(x => x.IsHead).HasDefaultValue(true);
            builder.Property(x => x.Remark).HasMaxLength(300);

        }
    }
}
