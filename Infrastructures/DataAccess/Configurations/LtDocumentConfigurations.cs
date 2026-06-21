using Domains.Models;
using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructures.DataAccess.Configurations;

public sealed class LtDocumentConfigurations : IBaseEntityTypeConfigurations<LtDocument>
{
    public void Configure(EntityTypeBuilder<LtDocument> builder)
    {
        builder.ToTable("lt_documents");
        builder.HasKey(x => x.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        builder.Property(s => s.OriginalName).HasMaxLength(455);
        builder.Property(s => s.NewFileName).HasMaxLength(455);
        builder.Property(s => s.FileUrl).HasMaxLength(455);
        builder.Property(s => s.AltText).HasMaxLength(455);
        builder.Property(s => s.Description).HasMaxLength(455);
        builder.Property(s => s.Created).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(s => s.Branch)
                .WithOne(x => x.LtDocument)
                .HasForeignKey<Branch>(x => x.ImagePathId)
                .OnDelete(DeleteBehavior.SetNull);
    }
}
