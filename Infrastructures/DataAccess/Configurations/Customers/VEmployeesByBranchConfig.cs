using Applications.Responses.ViewModels.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Customers
{
    public class VEmployeesByBranchConfig : IEntityTypeConfiguration<VEmployeesByBranch>
    {
        public void Configure(EntityTypeBuilder<VEmployeesByBranch> builder)
        {
            builder.ToView("VEmployeesByBranch");
            builder.HasNoKey();
            builder.Property(x => x.FullName).HasMaxLength(50);
            builder.Property(x => x.AltName).HasMaxLength(50);
            builder.Property(x => x.Gender).HasMaxLength(20);
            builder.Property(x => x.BranchName).HasMaxLength(200);
        }
    }
   
}
