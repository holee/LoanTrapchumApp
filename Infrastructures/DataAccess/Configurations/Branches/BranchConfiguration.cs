using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Branches;

public sealed  class BranchConfiguration : IBaseEntityTypeConfigurations<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("lt_branches");
        builder.HasKey(x => x.BranchId);
        builder.Property(x => x.BranchId).ValueGeneratedOnAdd();
        builder.Property(p => p.BranchName).HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(200);
        builder.Property(p => p.EditSeq).HasDefaultValue(0);
        builder.Property(p => p.IsActive).HasDefaultValue(true);
        builder.HasOne(x => x.Bank)
                  .WithMany(x => x.Branches)
                      .HasForeignKey(x => x.BankRefId);
    }

}