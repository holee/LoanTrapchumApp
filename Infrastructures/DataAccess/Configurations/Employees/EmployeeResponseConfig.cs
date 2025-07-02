using Applications.Responses.ViewModels.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Employees
{
    public class EmployeeResponseConfig : IBaseEntityTypeConfigurations<VEmployeeList>
    {
        public void Configure(EntityTypeBuilder<VEmployeeList> builder)
        {
            builder.ToTable("VEmployeeList");
            builder.HasKey(x => x.EmployeeId);
            builder.HasQueryFilter(x => x.IsActive == true);



            builder.HasMany(x => x.LoanDisbursments)
                .WithOne()
                .HasForeignKey(x => x.CreditAgentRefId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.HasMany(x => x.LoanRequests)
                .WithOne()
                .HasForeignKey(x => x.CreditAgentId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
