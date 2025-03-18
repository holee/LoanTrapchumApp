using Domains.Models.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.CashManagements
{
    public class CashStatusConfig : IEntityTypeConfiguration<CashStatus>
    {



        public void Configure(EntityTypeBuilder<CashStatus> builder)
        {
            builder.ToTable("st_cashStatuses");
            builder.HasKey(x => x.CashStatusId);
            builder.Property(x => x.CashStatusId).ValueGeneratedOnAdd();
            builder.Property(x => x.Code).HasMaxLength(50);
            builder.Property(x => x.Code).HasMaxLength(250);

            builder.HasData(new CashStatus[]
            {

                new CashStatus
                {
                    CashStatusId=1,
                    Code="IN",
                    Description="Cash comes in through transaction"

                },
                new CashStatus
                {
                     CashStatusId=2,
                    Code="OUT",
                    Description="Cash goes through transaction"
                },
                new CashStatus
                {
                     CashStatusId=3,
                    Code="DAYOPEN",
                    Description="Branch Manager gets money from Cash Vault"
                },
                new CashStatus
                {
                     CashStatusId=4,
                    Code="DAYClOSE",
                    Description="Branch Manager settles money to Cash Vault"
                },
                new CashStatus
                {
                     CashStatusId=5,
                    Code="SEND",
                    Description="User will send money to other user"
                },
                new CashStatus
                {

                     CashStatusId=6,
                    Code="RECEIVE",
                    Description="User will receive money from other user"
                },
                new CashStatus
                {
                    CashStatusId=7,
                    Code="VAULTIN",
                    Description="Manager will return the  money to Cash Vault"
                },
                new CashStatus
                {
                     CashStatusId=8,
                    Code="VAULTOUT",
                    Description="Manager will get the money from Cash Vault"
                }
            });
        }
    }
}
