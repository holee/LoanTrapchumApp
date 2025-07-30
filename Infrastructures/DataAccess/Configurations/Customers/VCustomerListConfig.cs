using Applications.Responses.ViewModels.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Customers
{
    public sealed class VCustomerExpenseConfig : IBaseEntityTypeConfigurations<VCustomerExpenses>
    {
        public void Configure(EntityTypeBuilder<VCustomerExpenses> builder)
        {
            builder.ToView("VCustomerExpenses");
            builder.HasKey(x => new { x.ExpendId, x.CustomerRefId });
            builder.Property(x => x.ExpendAmount).HasPrecision(18, 3);
        }
    }

    public sealed class VCustomerIncomeConfig : IBaseEntityTypeConfigurations<VCustomerIncomes>
    {
        public void Configure(EntityTypeBuilder<VCustomerIncomes> builder)
        {
            builder.ToView("VCustomerIncomes");
            builder.HasKey(x => new { x.IncomeId, x.CustomerRefId });
            builder.Property(x => x.IncomeAmount).HasPrecision(18, 3);
        }
    }

    public sealed class VCustomerNoteConfig : IBaseEntityTypeConfigurations<VCustomerNotes>
    {
        public void Configure(EntityTypeBuilder<VCustomerNotes> builder)
        {
            builder.ToView("VCustomerNotes");
            builder.HasKey(x => new { x.NoteId, x.CustomerRefId });
        }
    }

    public sealed class VCustomerActiviesConfig : IBaseEntityTypeConfigurations<VCustomerActivities>
    {
        public void Configure(EntityTypeBuilder<VCustomerActivities> builder)
        {
            builder.ToView("VCustomerActivities");
            builder.HasKey(x => new { x.ActivityId, x.CustomerRefId });
        }
    }





    public sealed class VCustomerListConfig : IBaseEntityTypeConfigurations<VCustomerList>
    {
        public void Configure(EntityTypeBuilder<VCustomerList> builder)
        {
            builder.ToView("VCustomerList");
            builder.HasKey(x => x.CustomerId);
            builder.Property(x => x.CustomerId).HasMaxLength(250);
            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.MiddleName).HasMaxLength(50);
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.Property(x => x.FullName).HasMaxLength(150);
            builder.Property(x => x.AltName).HasMaxLength(150);
            builder.Property(x => x.EntType).HasMaxLength(20);
            builder.Property(x => x.PassportId).HasMaxLength(50);
            builder.Property(x => x.Nationality).HasMaxLength(50);
            builder.Property(x => x.Salutation).HasMaxLength(10);
            builder.Property(x => x.JobTitle).HasMaxLength(100);
            builder.Property(x => x.Phone).HasMaxLength(20);
            builder.Property(x => x.AltPhone).HasMaxLength(20);
            builder.Property(x => x.EmailAddress).HasMaxLength(100);
            builder.Property(x => x.IdCard).HasMaxLength(50);
            builder.Property(x => x.CompanyName).HasMaxLength(200);
            builder.Property(x => x.MarritalStatus).HasMaxLength(20);
            builder.Property(x => x.SpouseName).HasMaxLength(150);
            builder.Property(x => x.SpouseNationality).HasMaxLength(50);
            builder.Property(x => x.SpousePassport).HasMaxLength(50);
            builder.Property(x => x.SpousePhoneNo).HasMaxLength(20);
            builder.Property(x => x.SpouseOccupation).HasMaxLength(100);
            builder.Property(x => x.Occupation).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.CountryName).HasMaxLength(50);
            builder.Property(x => x.City).HasMaxLength(50);
            builder.Property(x => x.ProInKhmer).HasMaxLength(100);
            builder.Property(x => x.DisInKhmer).HasMaxLength(100);
            builder.Property(x => x.ComInKhmer).HasMaxLength(100);
            builder.Property(x => x.VilInKhmer).HasMaxLength(100);
            builder.Property(x => x.Photo).HasMaxLength(500);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.IsLoanActive).HasDefaultValue(false);
            builder.HasQueryFilter(x => x.IsActive == true);
           
            builder.HasMany(x => x.CustomerActivities)
                    .WithOne()
                    .HasForeignKey(x => x.CustomerRefId);

            builder.HasMany(x => x.CustomerIncomes)
                   .WithOne()
                   .HasForeignKey(x => x.CustomerRefId);

            builder.HasMany(x => x.CustomerExpenses)
                  .WithOne()
                  .HasForeignKey(x => x.CustomerRefId);

            builder.HasMany(x => x.CustomerNotes)
                 .WithOne()
                 .HasForeignKey(x => x.CustomerRefId);

            builder.HasMany(x => x.LoanRequests)
                .WithOne()
                .HasForeignKey(x => x.CustomerId);

            builder.HasMany(x => x.Loans)
                    .WithOne()
                    .HasForeignKey(x => x.CustomerId);

            builder.HasMany(x => x.CollateralLines)
                   .WithOne()
                   .HasForeignKey(x => x.CollateralRefId);

        }
    }
}
