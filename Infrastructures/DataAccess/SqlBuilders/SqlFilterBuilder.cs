using Dapper;

namespace Infrastructures.DataAccess.SqlBuilders;

public sealed class SqlFilterBuilder
{
    private readonly List<string> _conditions = new();
    private readonly DynamicParameters _params = new();
    public void Eq(string column, object? value)
    {
        if (value == null) return;

        _conditions.Add($"{column} = @{column}");
        _params.Add(column, value);
    }

    public void Like(string column, string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;

        _conditions.Add($"{column} LIKE @{column}");
        _params.Add(column, $"%{value}%");
    }

    public void In<T>(string column, IEnumerable<T>? values)
    {
        if (values == null || !values.Any()) return;

        _conditions.Add($"{column} IN @{column}");
        _params.Add(column, values);
    }

    public void Between(string column, object? from, object? to)
    {
        if (from != null)
        {
            _conditions.Add($"{column} >= @From_{column}");
            _params.Add($"From_{column}", from);
        }

        if (to != null)
        {
            _conditions.Add($"{column} <= @To_{column}");
            _params.Add($"To_{column}", to);
        }
    }

    public (string where, DynamicParameters parameters) Build()
    {
        if (_conditions.Count == 0)
            return ("", _params);

        return ("WHERE " + string.Join(" AND ", _conditions), _params);
    }
}
