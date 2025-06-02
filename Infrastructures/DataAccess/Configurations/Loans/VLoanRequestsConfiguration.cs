using Applications.Responses.ViewModels.Loans.Requests;
using Infrastructures.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class VLoanRequestsConfiguration : IBaseEntityTypeConfigurations<VLoanRequests>
    {
        public void Configure(EntityTypeBuilder<VLoanRequests> builder)
        {
            builder.HasNoKey();
            builder.ToView("VLoanRequests");
            builder.Property(e => e.LoanRequestId).HasColumnName("LoanRequestId");
            builder.Property(e => e.RequestDate).HasColumnName("RequestDate");
            builder.Property(e => e.AdminFeeAmount).HasPrecision(18,3);
            builder.Property(e => e.RequestAmount).HasPrecision(18, 3);
            builder.Property(e => e.AprovedAmount).HasPrecision(18, 3);
            builder.Property(e => e.Status).HasColumnName("Status");
            builder.Property(e => e.AltName).HasMaxLength(200);
            // Add other property mappings as needed, matching the view's columns
        }
    }
}
