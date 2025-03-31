using Domains.Models.Lists.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Customers
{
    public class CustomerNoteConfiguration : IEntityTypeConfiguration<CustomerNote>
    {
        public void Configure(EntityTypeBuilder<CustomerNote> builder)
        {
            builder.ToTable("lt_notes");
            builder.HasKey(p=>p.NoteId);
            builder.Property(p => p.NoteId).ValueGeneratedOnAdd();
            builder.Property(p => p.Description).HasMaxLength(400); 
            builder.Property(p=>p.CustomerRefId).HasConversion(TypeConversions.GuidConversion);
            builder.Property(p=>p.CreditAgentRefId).HasConversion(TypeConversions.GuidConversion);
        }
    }
}
