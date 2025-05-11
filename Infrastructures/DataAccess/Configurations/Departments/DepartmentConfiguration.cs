using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Departments
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("lt_departments");
            builder.HasKey(dep => dep.DepartmentId);
            builder.Property(dep => dep.DepartmentId).ValueGeneratedOnAdd();
            builder.Property(p => p.DepartmentName).HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(100);
            builder.HasQueryFilter(x=>x.IsActive==true);
        }
    }
}
