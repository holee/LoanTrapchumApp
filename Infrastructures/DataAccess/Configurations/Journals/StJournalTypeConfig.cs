using Domains.Models.Journals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Journals
{
    public class StJournalTypeConfig : IEntityTypeConfiguration<JournalType>
    {
        public void Configure(EntityTypeBuilder<JournalType> builder)
        {
            builder.ToTable("st_journalTypes");

            builder.HasData(new JournalType[]
            {
                new JournalType
                {
                    Id=1,
                    Code="PV",
                    Description="Payment Voucher"
                },
                new JournalType
                {
                    Id=2,
                    Code="FT",
                    Description="Fund Transfer"
                },
                new JournalType
                {
                    Id=3,
                    Code="RV",
                    Description="Receipt Voucher"
                }, new JournalType
                {
                    Id=4,
                    Code="JV",
                    Description="Journal Voucher"
                }

            });


        }
    }
}
