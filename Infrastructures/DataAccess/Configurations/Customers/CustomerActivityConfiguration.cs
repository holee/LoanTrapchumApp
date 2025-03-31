using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domains.Models.Lists.Customers;
using Infrastructures.DataAccess.Configurations;

namespace Data.Configurations
{
    public class CustomerActivityConfiguration : IBaseEntityTypeConfigurations<CustomerActivity>
    {
        public void Configure(EntityTypeBuilder<CustomerActivity> builder)
        {
            builder.ToTable("lt_customerActivies", "dbo");

            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();

            builder.Property(x => x.CustomerRefId)
                .HasColumnName("CustomerRefId")
                .HasMaxLength(36)
                .HasConversion(TypeConversions.GuidConversion);

            builder.Property(x => x.Status)
                .HasColumnName("Status")
                .HasDefaultValue(false)
                .IsRequired(false);

            builder.Property(x => x.Description)
                .HasColumnName("Description")
                .HasMaxLength(450)
                .IsRequired(false);

            builder.Property(x => x.CreatedDT)
                .HasColumnName("CreatedDT")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired(false);

            builder.Property(x => x.ModifiedDT)
                .HasColumnName("ModifiedDT")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired(false);
        }
    }
}
