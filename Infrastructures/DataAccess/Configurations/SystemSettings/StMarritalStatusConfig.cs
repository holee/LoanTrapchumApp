using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.SystemSettings
{
    public class StMarritalStatusConfig : IEntityTypeConfiguration<MarritalStatus>
    {
        public void Configure(EntityTypeBuilder<MarritalStatus> builder)
        {
            builder.ToTable("st_MarritalStatuses");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status).HasMaxLength(100);

            builder.HasData(new MarritalStatus[]
            {
                new MarritalStatus
                {
                    Id=1,
                    Status="Single"
                },
                new MarritalStatus
                {
                    Id=2,
                    Status="Married"
                },
                new MarritalStatus
                {
                    Id=4,
                    Status="Divorced"
                },
                new MarritalStatus
                {
                    Id=5,
                    Status="Widow"
                },
                new MarritalStatus
                {
                    Id=6,
                    Status="N/A"
                }
            });

        }
    }
}