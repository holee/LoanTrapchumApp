using Applications.Responses.ViewModels.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Employees
{
    public class EmployeeCreditAgentConfiguration : IBaseEntityTypeConfigurations<VEmployeeCreditAgent> 
    {
        public void Configure(EntityTypeBuilder<VEmployeeCreditAgent> builder)
        {
            builder.ToView("VEmployeeCreateCreditAgents");
            builder.HasNoKey();
            builder.HasQueryFilter(x => x.IsActive);
        }
    }
}
