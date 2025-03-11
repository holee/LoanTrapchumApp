using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Currencies;

public class CurrencyCounter
{
    public int Id { get; set; }
    public int? CurrencyRefId { get; set; }
    public decimal? CurrencyValue { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
    public bool IsActive { get; set; } = true;
    public Currency? Currency { get; set; }

}
