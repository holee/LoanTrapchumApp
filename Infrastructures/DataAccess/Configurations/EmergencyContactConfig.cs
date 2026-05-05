using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructures.DataAccess.Configurations
{
    public sealed class EmergencyContactConfig : IBaseEntityTypeConfigurations<EmergencyContact>
    {
        public void Configure(EntityTypeBuilder<EmergencyContact> builder)
        {
            builder.ToTable("lt_emergency_contact");
            builder.HasKey(x => x.ContactId);
            builder.Property(x => x.ContactId).HasMaxLength(450);
            builder.Property(x => x.ContactName).HasMaxLength(400);
            builder.Property(x => x.ContactNumber).HasMaxLength(20);
            builder.Property(x => x.Relationship).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(500);
        }
    }
}
