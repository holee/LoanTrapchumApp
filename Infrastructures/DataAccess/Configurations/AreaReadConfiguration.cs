using Applications.Features.Addresses.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations;

public sealed class AreaReadConfiguration : IBaseEntityTypeConfigurations<AreaDto>
{
    public void Configure(EntityTypeBuilder<AreaDto> builder)
    {
        builder.ToView("vAreas");
    }
}
