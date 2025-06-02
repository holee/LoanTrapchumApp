using Applications.Responses.ViewModels.Addresses;
using Applications.Responses.ViewModels.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.DataAccess.Configurations.Addresses
{
    public class VDistrictConfig : IEntityTypeConfiguration<VDistricts>
    {
        public void Configure(EntityTypeBuilder<VDistricts> builder)
        {
            builder.ToView("VDistricts");
            builder.HasNoKey();
        }
    }
    

    
}
