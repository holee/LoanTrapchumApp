using Applications.Responses.ViewModels.Auths;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.DataAccess.Configurations.Apps
{
    public sealed class VUserConfigurations : IBaseEntityTypeConfigurations<VUser>
    {
        public void Configure(EntityTypeBuilder<VUser> builder)
        {
            builder.ToView("VUsers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName).HasMaxLength(128);
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.BranchName).HasMaxLength(250);
            builder.Property(x => x.Gender).HasMaxLength(20);
        }
    }
}
