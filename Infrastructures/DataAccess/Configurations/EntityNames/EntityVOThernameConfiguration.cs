using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.EntityNames;

public class EntityVOThernameConfiguration : IBaseEntityTypeConfigurations<VOtherNames>
{
    public void Configure(EntityTypeBuilder<VOtherNames> builder)
    {
        builder.ToView("VNames");
        builder.HasNoKey();
        builder.HasQueryFilter(x => x.EntType == EntType.OtherName.ToString());
    }

}