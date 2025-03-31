using Domains.Models.Lists.Employees;
using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Employees
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("lt_entEmployees");
            builder.HasKey(e => e.EmployeeId);
            builder.Property(e => e.EmployeeId);
            //builder.Property(e => e.Nationality).HasDefaultValue("Khmer").IsRequired(false);
            builder.Property(e => e.HireDate).HasColumnType("Date");
            builder.Property(e => e.EndDate).HasColumnType("Date");
            builder.Property(e => e.DepartmentRefId);
            builder.Property(x => x.StaffId).HasComputedColumnSql("[EmployeeId]");
            builder.Property(x => x.UserRefId).HasMaxLength(60);
            builder.Property(e => e.BranchRefId);
            //builder.Property(e => e.Occupation).HasMaxLength(100);
            builder.Property(e => e.Photo);
            //builder.Property(e => e.DateOfBirth).HasColumnType("Date");
            //builder.Property(e => e.MarritalStatus).HasMaxLength(50).IsRequired(false);
            //builder.Property(e => e.AltPhone).HasMaxLength(20);
            //builder.Property(e => e.Gender).HasMaxLength(10);
            //builder.Property(e => e.Description).HasMaxLength(250);
            //builder.Property(e => e.CreatedDt).HasColumnType("DateTime");
            //builder.Property(e => e.ModifiedDt).HasColumnType("DateTime");
            //builder.HasQueryFilter(x => x.IsActive == true);

            builder.HasOne(x => x.Department)
                        .WithMany(x=>x.Employees)
                            .HasForeignKey(x => x.DepartmentRefId);

            builder.HasOne(x => x.Branch)
                      .WithMany(x=>x.Employees)
                          .HasForeignKey(x => x.BranchRefId);

            builder.HasOne(x => x.LoanCreditAgent)
                        .WithOne(x => x.Employee)
                            .HasForeignKey<LoanCreditAgent>(x => x.EmployeeRefId);

        }
   
    }
}
