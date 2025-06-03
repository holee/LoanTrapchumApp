using Applications.Responses.ViewModels.Addresses;
using Domains.Models.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Addresses;

public class AddrVillageConfiguration : IEntityTypeConfiguration<Village>
{
    public void Configure(EntityTypeBuilder<Village> builder)
    {
        builder.ToTable("lt_addrVillages");
        builder.HasKey(x => x.VillageId);
        builder.Property(x => x.VillageId).ValueGeneratedOnAdd();
        builder.Property(p => p.CommuneRefId);
        builder.Property(p => p.VilInKhmer).HasMaxLength(200);
        builder.Property(p => p.VillDescription).HasMaxLength(200);
    }
}
public class VVillageConfiguration : IEntityTypeConfiguration<VVillages>
{
    public void Configure(EntityTypeBuilder<VVillages> builder)
    {
        builder.ToView("VAddrVillages");
        builder.HasNoKey();
    }
}