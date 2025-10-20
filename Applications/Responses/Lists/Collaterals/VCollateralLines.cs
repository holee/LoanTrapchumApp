using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Responses.Lists.Collaterals;

public sealed class VCollateralLines
{
    public long Id { get; set; }
    public string? CollateralId { get; set; }
    public int? CategoryRefId { get; set; }
    public string? TypeName { get; set; }
    public int? SubCategoryRefId { get; set; }
    public string? CollateralSubTypeName { get; set; }
    public string? ReferenceNo { get; set; }
    public string? CollateralNo { get; set; } 
    public bool IsAvailable { get; set; } = true;  
    public bool? IsReturn { get; set; }=false;    
    public DateTime? ReturnDate { get; set; } 
    public string? OwnerName { get; set; }
    public string? Location { get; set; }
    public string? WareHouse { get; set; }
    public int? Quantity { get; set; } = 1;
    public string? Size { get; set; } 
    public decimal? MarketPrice { get; set; } = 0.00M;
    public decimal? HotPrice { get; set; } = 0.00M;
    public decimal? PawnPrice { get; set; } = 0.00M;
    public string? Description { get; set; }
    public bool IsUsed { get; set; } = false;
    public decimal ValueRate { get; set; }
    public DateTime? RegisterDate { get; set; }

    [NotMapped]
    public int RowIndex { get; set; } = 1;

}
