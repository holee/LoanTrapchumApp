using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class LoanRequestConfiguration : IEntityTypeConfiguration<LoanRequest>
    {
        public void Configure(EntityTypeBuilder<LoanRequest> builder)
        {
            builder.ToTable("tt_loanRequests");
            builder.HasKey(x => x.LoanRequestId);
            builder.Property(x => x.LoanRequestId).HasMaxLength(250);
            builder.Property(x => x.CustomerRefId).HasMaxLength(250);
            builder.Property(x => x.CreditAgentRefId).HasMaxLength(250);
            builder.Property(x => x.RequestUserRefId).HasMaxLength(250);
            builder.Property(x => x.AprovedUserId).HasMaxLength(250);
            builder.Property(x => x.RequestAmount).HasPrecision(18, 3);
            builder.Property(x => x.InterestRate).HasPrecision(18, 3);
            builder.Property(x => x.CollectionFeeRate).HasPrecision(18, 3);
            builder.Property(x => x.RequestDate).HasColumnType("DATE").HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.CreatedDt).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.ModifiedDt).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.Remark).HasMaxLength(400);
            builder.Property(x => x.LoanPurpose).HasMaxLength(400);
            builder.Property(x => x.IsSubmitted).HasDefaultValue(false);


            builder.HasMany(x => x.LoanDocuments)
                    .WithOne(x => x.LoanRequest)
                    .HasForeignKey(x => x.LoanRequestId);

            builder.HasMany(x => x.LoanDescriptions)
                .WithOne()
                .HasForeignKey(fk => fk.LoanRequestRefId);


            builder.HasMany(x => x.CoPawnRequests)
                .WithOne(x => x.LoanRequest)
                .HasForeignKey(x => x.RequestId);

        }
    }


}


// builder.HasOne(x=>x.Lo)
//builder.HasMany(c => c.CollateralLines)
//       .WithMany(l => l.LoanRequests)
//       .UsingEntity<CollateralLink>(
//           j => j.HasOne(x => x.CollateralLine)
//                 .WithMany()
//                 .HasForeignKey(fk => fk.CollateralLineRefId),
//           j => j.HasOne(l => l.LoanRequest)
//                .WithMany()
//                .HasForeignKey(x => x.LoanRequestRefId));

//builder.HasMany(g => g.Guarantors)
//        .WithMany(l => l.LoanRequests)
//        .UsingEntity<GuarantorLink>(
//            j => j.HasOne(x => x.Guarantor)
//                    .WithMany()
//                    .HasForeignKey(fk => fk.EntityNameRefId),
//            j => j.HasOne(l => l.LoanRequest)
//                    .WithMany()
//                    .HasForeignKey(fk => fk.LoanRequestRefId)
//        );
//builder.HasMany(x => x.LoanGuarants)
//    .WithOne();