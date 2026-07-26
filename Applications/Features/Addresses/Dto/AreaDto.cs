using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Features.Addresses.Dto;

public sealed class AreaDto
{
    public long Id { get; set; } 
    public string? Code { get; set; }
    public string? Khet { get; set; }
    public string? Srok { get; set; }
    public string? Khum { get; set; }
    public string? Phum { get; set; }
    public string? AreaName { get; set; }
    [NotMapped] public int RowIndex { get; set; } = 0;
}
