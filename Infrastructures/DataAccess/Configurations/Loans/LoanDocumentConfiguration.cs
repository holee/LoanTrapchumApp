using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class LoanDocumentConfiguration : IEntityTypeConfiguration<LoanDocuments> 
    {
        public void Configure(EntityTypeBuilder<LoanDocuments> builder)
        {
            builder.ToTable("lt_loandocuments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.OriginalName).HasMaxLength(400);
            builder.Property(x => x.NewFileName).HasMaxLength(400);
            builder.Property(x => x.FileUrl).HasMaxLength(400);
            builder.Property(x => x.AltText).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(200);
        }
    }


}
