using Applications.Responses.ViewModels.Branches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Branches;

public class VBranchConfig : IEntityTypeConfiguration<VBranchResponse>
{
    public void Configure(EntityTypeBuilder<VBranchResponse> builder)
    {
        builder.ToView("VBranches");
        builder.HasNoKey();
        builder.HasQueryFilter(x=>x.IsActive==true);
    }
}
