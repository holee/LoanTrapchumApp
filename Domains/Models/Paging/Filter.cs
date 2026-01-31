namespace Domains.Models.Paging;

public sealed class Filter
{
    public string Field { get; set; } = "";
    public Operators Operator { get; set; } = Operators.Equals; // =, LIKE, >, <, >=, <=
    public object Value { get; set; } = default!;
}
