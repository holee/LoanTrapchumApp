using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public sealed class RecieptConfigurations : IBaseEntityTypeConfigurations<Reciept>
    {
        public void Configure(EntityTypeBuilder<Reciept> builder)
        {
            builder.ToTable("tt_reciepts");
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.RecieptNumber).HasMaxLength(20);
        }
    }
}
