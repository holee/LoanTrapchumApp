using Domains.Models.Collaterals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals
{
    public class CollateralLinkConfig : IEntityTypeConfiguration<CollateralLink>
    {
        public void Configure(EntityTypeBuilder<CollateralLink> builder)
        {
            builder.ToTable("lt_collateralLinks");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.CreatedDt).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.ModifiedDt).HasDefaultValueSql("GETDATE()");


            builder.HasOne(x => x.CollateralLine)
                .WithMany(x => x.CollateralLinesLink)
                .HasForeignKey(x => x.CollateralLineRefId);


            builder.HasOne(x => x.LoanRequest)
                .WithMany(x => x.CollateralLinesLink)
                .HasForeignKey(x => x.LoanRequestRefId);
        }

    }
}
