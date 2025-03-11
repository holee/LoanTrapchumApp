using System.ComponentModel.DataAnnotations;

namespace Applications.Requests.Lists.Currency;

public class CurrencyNoteDtoCreate
{

    [Required]
    public int? CurrencyRefId { get; set; }

    [Required]
    public int? CurrencyValue { get; set; }

    [Required]
    [StringLength(250)]
    public string? Description { get; set; }
    public int? Quantity { get; set; }
}