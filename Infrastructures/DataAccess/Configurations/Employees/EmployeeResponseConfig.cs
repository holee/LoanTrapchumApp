using Applications.Responses.ViewModels.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Employees
{
    public class EmployeeResponseConfig : IEntityTypeConfiguration<VEmployees>
    {
        public void Configure(EntityTypeBuilder<VEmployees> builder)
        {
            builder.ToTable("VEmployeeList");
            builder.HasNoKey();
            builder.HasQueryFilter(x=>x.IsActive == true);
        }

    }
}
