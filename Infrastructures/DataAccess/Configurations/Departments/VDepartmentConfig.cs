using Applications.Responses.ViewModels.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Departments;

public class VDepartmentConfig : IEntityTypeConfiguration<VDepartment>
{
    public void Configure(EntityTypeBuilder<VDepartment> builder)
    {
        builder.ToView("VDepartments");
        builder.HasNoKey();
        builder.HasQueryFilter(x=>x.IsActive==true);
    }
}
