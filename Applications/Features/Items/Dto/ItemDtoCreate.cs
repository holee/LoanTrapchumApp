using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Items.Dto;

public class ItemDtoCreate
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(250)]
    public string ItemName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [StringLength(400)]
    public string Description { get; set; } = string.Empty;

    public int? LedgerRefId { get; set; }
    public bool IsActive { get; set; } = true;
}
