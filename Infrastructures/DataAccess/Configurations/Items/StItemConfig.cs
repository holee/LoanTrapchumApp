using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Items
{
    public class StItemConfig : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("st_items");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.ItemName).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(400).IsRequired();
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.HasData(new Item[]
            {
                new Item
                {
                    Id=1,
                    ItemName="Principle",
                    Description="Principle",
                    IsActive = true,
                },
                new Item
                {
                    Id=2,
                    ItemName="Interest",
                    Description="Interest",
                    IsActive = true,
                },
                new Item
                {
                    Id=3,
                    ItemName="Affective Interest",
                    Description="Affective Interest",
                    IsActive = true,
                },
                new Item
                {
                    Id=4,
                    ItemName="Admin Free",
                     Description="Admin Free",
                    IsActive = true,
                },
                new Item
                {
                    Id=5,
                    ItemName="Other Charged",
                     Description="Other Charged",
                    IsActive = true,
                }
            });
        }
    }
}
