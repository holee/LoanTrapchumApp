using Applications.Responses.ViewModels.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Customers
{
    public sealed class VCustomerNoteConfig : IBaseEntityTypeConfigurations<VCustomerNotes>
    {
        public void Configure(EntityTypeBuilder<VCustomerNotes> builder)
        {
            builder.ToView("VCustomerNotes");
            builder.HasNoKey();
        }
    }
}
