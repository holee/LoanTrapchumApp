using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.EntityNames
{
    public class GuarantorConfig : IEntityTypeConfiguration<GuarantorLink>
    {
        public void Configure(EntityTypeBuilder<GuarantorLink> builder)
        {
            builder.ToTable("lt_loanGuarantors");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDt).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.ModifiedDt).HasDefaultValueSql("GETDATE()");


            builder.HasOne(x => x.Guarantor)
                            .WithMany(x => x.GuarantorsLink)
                            .HasForeignKey(fk => fk.EntityNameRefId);

            builder.HasOne(x => x.LoanRequest)
                            .WithMany(x => x.GuarantorsLink)
                            .HasForeignKey(fk => fk.LoanRequestRefId);
        }
    }
}
