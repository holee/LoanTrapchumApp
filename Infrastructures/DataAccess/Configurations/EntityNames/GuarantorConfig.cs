using Domains.Models.Lists;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.EntityNames
{
    public class GuarantorConfig : IEntityTypeConfiguration<LoanGuarantorLink>
    {
        public void Configure(EntityTypeBuilder<LoanGuarantorLink> builder)
        {
            builder.ToTable("lt_loanGuarantors");
            builder.HasKey(x => new { x.Id });
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDt).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.ModifiedDt).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.EntityNameRefId).HasMaxLength(250);
            builder.Property(x => x.LoanRequestRefId).HasMaxLength(250);
            builder.HasOne(x => x.Guarantor)
                            .WithMany(x=>x.GuarantorsLink)
                            .HasForeignKey(fk => fk.EntityNameRefId);

            builder.HasOne(x => x.LoanRequest)
                            .WithMany(x => x.GuarantorsLink)
                            .HasForeignKey(fk => fk.LoanRequestRefId);
        }
    }
}
