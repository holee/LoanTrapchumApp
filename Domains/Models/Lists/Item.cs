namespace Domains.Models.Lists;

public class Item
{
    public int Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Description { get; set;} = string.Empty;
    public int? LedgerRefId { get; set; }
    public bool? IsActive { get; set; }
}


