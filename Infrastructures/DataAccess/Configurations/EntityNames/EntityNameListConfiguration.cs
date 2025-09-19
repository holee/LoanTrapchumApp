using Applications.Responses.ViewModels.OtherNames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.EntityNames;

public class EntityNameListConfiguration : IBaseEntityTypeConfigurations<NameListVm>
{
    public void Configure(EntityTypeBuilder<NameListVm> builder)
    {
        builder.ToView("VNameList");
        builder.HasNoKey();
        builder.HasQueryFilter(x => x.IsActive == true);
    }

}