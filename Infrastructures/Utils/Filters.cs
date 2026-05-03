namespace Infrastructures.Utils;

public record Filters(string Field, object? Value, FilterOp Operator = FilterOp.Equal);
