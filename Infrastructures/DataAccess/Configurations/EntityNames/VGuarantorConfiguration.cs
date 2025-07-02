using Applications.Responses.ViewModels.OtherNames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.EntityNames
{
    public class VGuarantorConfiguration : IBaseEntityTypeConfigurations<VGuarantor>
    {
        public void Configure(EntityTypeBuilder<VGuarantor> builder)
        {
            builder.ToView("VGuarantors");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.GuarantorName).HasMaxLength(100);
            builder.Property(x => x.FullName).HasMaxLength(100);
            builder.Property(x => x.ProInKhmer).HasMaxLength(100);
            builder.Property(x => x.DisInKhmer).HasMaxLength(100);
            builder.Property(x => x.ComInKhmer).HasMaxLength(100);
            builder.Property(x => x.VilInKhmer).HasMaxLength(100);

            builder.HasMany(x => x.GuarantorLoans)
                .WithOne()
                .HasForeignKey(x => x.GuarantorId);


            builder.HasMany(x => x.LoanRequests)
                .WithOne()
                .HasForeignKey(x => x.GuarantorId);
        }
    }


}
