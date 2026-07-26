using System;
using System.Collections.Generic;
using System.Text;

namespace Domains.Models;

public sealed class Area
{
    public long Id { get; set; }
    public int? BranchId { get; set; }
    public string? Code { get; set; }
    public int? VillageId { get; set; }
    public int? CommuneId { get; set; }
    public int? DistrictId { get; set; }
    public int? ProvinceId { get; set; }
    public int? Mapping { get; set; }
}
