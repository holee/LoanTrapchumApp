using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Banks
{
    public class LtBackOtherConfig : IEntityTypeConfiguration<BankOther>
    {
        public void Configure(EntityTypeBuilder<BankOther> builder)
        {
            builder.ToTable("st_bankOthers");
            builder.HasKey(x => x.BankOId);
            builder.Property(prop => prop.BankName).HasMaxLength(100);

            ///Data
            builder.HasData(new BankOther[]
            {
                new BankOther
                {
                    BankOId=1,
                    BankName="ANZ ROYAL BANK CAMBODIA"
                },
                new BankOther
                {
                    BankOId=2,
                    BankName="MEKONG BANK"
                },
                new BankOther
                {
                    BankOId=3,
                    BankName="CAMBODIA MEKONG BANK"
                },
                new BankOther
                {
                    BankOId=4,
                    BankName="SINGAPORE BANKING CORPERATION"
                },
                new BankOther
                {
                    BankOId=5,
                    BankName="FOREIGN TRADE BANK OF CAMBODIA"
                },
                new BankOther
                {
                    BankOId=6,
                    BankName="CAMBODIAN COMMERCIAL BANK"
                },
                new BankOther
                {
                    BankOId=7,
                    BankName="CANADIA BANK"
                },
                new BankOther
                {
                    BankOId=8,
                    BankName="KRUNG THAI BANK"
                },
                new BankOther
                {
                    BankOId=9,
                    BankName="CAMBODIA PUBLIC BANK"
                },
                new BankOther
                {
                    BankOId=10,
                    BankName="MAYBANK"
                },
                new BankOther
                {
                    BankOId=11,
                    BankName="UNION COMMERCIAL BANK"
                },
                new BankOther
                {
                    BankOId=12,
                    BankName="ADVANCED BANK OF ASIA LIMITED"
                },
                new BankOther
                {
                    BankOId=13,
                    BankName="RURAL DEVELOPMENT BANK"
                },
                new BankOther
                {
                    BankOId=14,
                    BankName="FIRST COMMERCIAL BANK"
                },
                new BankOther
                {
                    BankOId=15,
                    BankName="SHINHAN KHMER BANK"
                },
                new BankOther
                {
                    BankOId=16,
                    BankName="PENG HENG SME"
                },
                new BankOther
                {
                    BankOId=17,
                    BankName="VATTANAC BANK"
                },
                new BankOther
                {
                    BankOId=18,
                    BankName="NATIONAL BANK OF CAMBODIA"
                },
                 new BankOther
                {
                    BankOId=19,
                    BankName="NATIONAL TREASURY GENERAL"
                },
                  new BankOther
                {
                    BankOId=20,
                    BankName="FIRST INVESTMENT SPECIALIZED BANK"
                },
                   new BankOther
                {
                    BankOId=21,
                    BankName="ACLEDA BANK"
                },

            });





        }
    }
}

