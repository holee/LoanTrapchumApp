namespace Infrastructures.DataAccess.Configurations.LoanDisbursements
{
    //public class LoanDisbursmentLineConfig : IEntityTypeConfiguration<LoanDisbursmentLine>
    //{
    //    public void Configure(EntityTypeBuilder<LoanDisbursmentLine> builder)
    //    {
    //        builder.ToTable("st_LoanDisbursmentLines");
    //        builder.HasKey(x => x.Id);
    //        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    //        builder.Property(x => x.LoanRefId).IsRequired(false);
    //        builder.Property(x => x.InstallmentNo).IsRequired(false);
    //        builder.Property(x => x.InstallmentDate).IsRequired(false);
    //        builder.Property(x => x.Days).IsRequired(false);
    //        builder.Property(x => x.Principle).HasPrecision(18,2).IsRequired(false);
    //        builder.Property(x => x.Interest).HasPrecision(18,2).IsRequired(false);
    //        builder.Property(x => x.OtherInterest).HasPrecision(18, 2).IsRequired(false);
    //        builder.Property(x => x.SubTotal).HasPrecision(18, 2).IsRequired(false);
    //        builder.Property(x => x.Balance).HasPrecision(18, 2).IsRequired(false);
    //        builder.Property(x => x.StatusRefId).IsRequired(false);
    //        builder.Property(x => x.AproveBy).HasMaxLength(100).IsRequired(false);
    //        builder.Property(x => x.CheckedBy).HasMaxLength(100).IsRequired(false);
    //        builder.Property(x => x.RequestBy).HasMaxLength(10).IsRequired(false);
    //        builder.Property(x => x.IsActive).IsRequired(false).HasDefaultValue(true);
    //        builder.Property(x => x.IsDelete).IsRequired(false).HasDefaultValue(false);
    //    }
    //}
}
