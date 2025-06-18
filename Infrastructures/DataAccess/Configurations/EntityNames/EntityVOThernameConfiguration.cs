using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Bases;
using Infrastructures.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EntityVOThernameConfiguration : IBaseEntityTypeConfigurations<VOtherNames>
{
    public void Configure(EntityTypeBuilder<VOtherNames> builder)
    {
        builder.ToView("VNames");
        builder.HasNoKey();
        builder.HasQueryFilter(x => x.EntType == EntType.OtherName.ToString());
    }

}

