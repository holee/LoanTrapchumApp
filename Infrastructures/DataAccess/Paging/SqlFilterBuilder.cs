using Dapper;
using Domains.Models.Paging;
namespace Infrastructures.DataAccess.Paging;


public static class SqlFilterBuilder
{
    public static (string whereSql, DynamicParameters parameters) Build(IEnumerable<Filter> filters)
    {
        var parameters = new DynamicParameters();

        var conditions = new List<string>();

        int index = 0;

        foreach (var filter in filters)
        {
            var paramName = $"@p{index++}";

            var op = filter.Operator switch
            {
                Operators.Equals => "=",
                Operators.NotEquals => "<>",
                Operators.GreaterThan => ">",
                Operators.LessThan => "<",
                Operators.GreaterThanOrEqual => ">=",
                Operators.LessThanOrEqual => "<=",
                Operators.Contains => "LIKE",
                Operators.StartsWith => "LIKE",
                Operators.EndsWith => "LIKE",
                _ => throw new ArgumentOutOfRangeException()
            };

            if (filter.Operator == Operators.Contains)
            {
                conditions.Add($"{filter.Field} LIKE {paramName}");

                parameters.Add(paramName, $"%{filter.Value}%");
            }
            else if (filter.Operator == Operators.StartsWith)
            {
                conditions.Add($"{filter.Field} LIKE {paramName}");

                parameters.Add(paramName, $"%{filter.Value}");
            }
            else if (filter.Operator == Operators.EndsWith)
            {
                conditions.Add($"{filter.Field} LIKE {paramName}");

                parameters.Add(paramName, $"{filter.Value}%");
            }
            else
            {
                conditions.Add($"{filter.Field} {op} {paramName}");

                parameters.Add(paramName, filter.Value);
            }
        }

        var whereSql = conditions.Any() ? "WHERE " + string.Join(" AND ", conditions) : "";

        return (whereSql, parameters);
    }



}
