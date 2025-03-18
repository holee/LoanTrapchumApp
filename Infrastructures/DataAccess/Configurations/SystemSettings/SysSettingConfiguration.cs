namespace Infrastructures.DataAccess.Configurations.SystemSettings
{
    //public class SysSettingConfiguration : IEntityTypeConfiguration<SysSetting>
    //{
    //    public void Configure(EntityTypeBuilder<SysSetting> builder)
    //    {
    //        builder.ToTable("SysSettings");
    //        builder.HasKey(x => x.Id);
    //        builder.HasIndex(x => x.Code).IsUnique();
    //        builder.Property(x => x.Code).HasMaxLength(20);
    //        builder.Property(x=>x.Description).HasMaxLength(100);
    //        builder.Property(x => x.Status).HasDefaultValue(true);
    //        builder.Property(x => x.CreatedDT).HasDefaultValueSql("GETDATE()");
    //        builder.Property(x => x.ModifiedDT).HasDefaultValueSql("GETDATE()");
    //        builder.HasQueryFilter(x => x.Status == true);
    //    }
    //}
}
