using Domains.Models.Journals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Ledgers
{
    public class StLedgerTypeConfiguration : IEntityTypeConfiguration<LedgerType>
    {
        public void Configure(EntityTypeBuilder<LedgerType> builder)
        {
            builder.ToTable("st_ledgerTypes");
            builder.Property(t => t.LedgerTypeId);
            builder.Property(t => t.LedgerTypeCode).HasMaxLength(50);
            builder.Property(t => t.CrOrDr).HasColumnName("CrOrDr").HasMaxLength(20);
            builder.Property(t => t.NOrder).HasColumnName("NOrder");

            builder.HasData(new LedgerType[]
            {
                new LedgerType
                {
                    LedgerTypeId=1,
                    LedgerTypeCode="Cash and Bank",
                    CrOrDr="Dr",
                    NOrder=1
                },
                new LedgerType
                {
                    LedgerTypeId=2,
                    LedgerTypeCode="Asset",
                    CrOrDr="Dr",
                    NOrder=1
                },
                new LedgerType
                {
                    LedgerTypeId=3,
                    LedgerTypeCode="Liability",
                    CrOrDr="Cr",
                    NOrder=3
                },
                 new LedgerType
                {
                    LedgerTypeId=4,
                    LedgerTypeCode="Equity",
                    CrOrDr="Cr",
                    NOrder=4
                },
                  new LedgerType
                {
                    LedgerTypeId=5,
                    LedgerTypeCode="Interest Income",
                    CrOrDr="Cr",
                    NOrder=4
                },
                  new LedgerType
                {
                    LedgerTypeId=6,
                    LedgerTypeCode="Interest Expense",
                    CrOrDr="Dr",
                    NOrder=6
                },
                new LedgerType
                {
                    LedgerTypeId=7,
                    LedgerTypeCode="Other Income",
                    CrOrDr="Cr",
                    NOrder=5

                },
                new LedgerType
                {
                    LedgerTypeId=8,
                    LedgerTypeCode="Operating Expense",
                    CrOrDr="Dr",
                    NOrder=6
                },
                new LedgerType
                {
                    LedgerTypeId=9,
                    LedgerTypeCode="Depreciation Expense",
                    CrOrDr="Dr",
                    NOrder=6
                },
                new LedgerType
                {
                    LedgerTypeId=10,
                    LedgerTypeCode="Amortization",
                    CrOrDr="Dr",
                    NOrder=6
                },
                new LedgerType
                {
                    LedgerTypeId=11,
                    LedgerTypeCode="Foreign Exchange Losses",
                    CrOrDr="Dr",
                    NOrder=6
                },
                new LedgerType
                {
                    LedgerTypeId=12,
                    LedgerTypeCode="Commission Expense",
                    CrOrDr="Dr",
                    NOrder=6
                },
                new LedgerType
                {
                    LedgerTypeId=13,
                    LedgerTypeCode=" Off BS Item",
                    CrOrDr="Dr",
                    NOrder=9
                },
                new LedgerType
                {
                    LedgerTypeId=14,
                    LedgerTypeCode="Contra Off BS Item",
                    CrOrDr="Cr",
                    NOrder=9
                },
            });
        }
    }
}



