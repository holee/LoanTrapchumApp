using Applications.Extensions.Identity.Enumerations;
using Domains.Models.Identity;
using Infrastructures.Constants.Schemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Auths;

public class PermissionsConfigurations : IBaseEntityTypeConfigurations<Permissions>
{
    public void Configure(EntityTypeBuilder<Permissions> builder)
    {
        builder.ToTable(Tables.Permisions, SchemaNames.Security);
        builder.HasKey(x => x.Id);  
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Group).HasMaxLength(250);
        builder.Property(x => x.Description).HasMaxLength(250);
        builder.Property(x => x.Action).HasMaxLength(250);
        builder.Property(x=>x.IsBasic).HasDefaultValue(true);
    }
}