using Applications.Responses.ViewModels.Journals;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Bases;
using Infrastructures.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EntityNameListConfiguration : IBaseEntityTypeConfigurations<NameListVm>
{
    public void Configure(EntityTypeBuilder<NameListVm> builder)
    {
        builder.ToView("VNameList");
        builder.HasNoKey();
        builder.HasQueryFilter(x => x.IsActive == true);
    }

}

