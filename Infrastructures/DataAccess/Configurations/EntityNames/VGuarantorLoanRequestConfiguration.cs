using Applications.Responses.ViewModels.Loans.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.EntityNames
{
    public class VGuarantorLoanRequestConfiguration : IBaseEntityTypeConfigurations<VLoanRequestGuarantor>
    {
        public void Configure(EntityTypeBuilder<VLoanRequestGuarantor> builder)
        {
            builder.ToView("VLoanRequestGuarantors");
            builder.HasKey(x => new {x.Id,x.LoanRequestRefId});
            builder.Property(x => x.GuarantorName).HasMaxLength(100);
          
        }
    }


}
