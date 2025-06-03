using Applications.Responses.ViewModels.Addresses;
using Domains.Models.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Addresses;

public class AddrCommuneConfiguration : IEntityTypeConfiguration<Commune>
{
    public void Configure(EntityTypeBuilder<Commune> builder)
    {
        builder.ToTable("lt_addrCommunes");
        builder.HasKey(x => x.CommuneId);
        builder.Property(x => x.CommuneId).ValueGeneratedNever();
        builder.Property(x => x.DistrictRefId);
        builder.Property(x => x.ComInKhmer).HasMaxLength(100);
        builder.Property(x => x.ComDescription).HasMaxLength(100);
    }
}
public class VCommuneConfiguration : IEntityTypeConfiguration<VCommunes>
{
    public void Configure(EntityTypeBuilder<VCommunes> builder)
    {
        builder.ToView("VAddrCommune");
        builder.HasNoKey();
    }
}
