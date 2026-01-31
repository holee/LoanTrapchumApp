using Dapper;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructures.DataAccess.Paging;

public class SqlExpressionVisitor : ExpressionVisitor
{
    private readonly StringBuilder _sql = new();
    private readonly DynamicParameters _params = new();
    private int _index = 0;

    public (string Sql, DynamicParameters Params) Translate(Expression exp)
    {
        Visit(exp);
        return (_sql.ToString(), _params);
    }


    //protected override Expression VisitMember(MemberExpression node)
    //{
    //    if (node.Expression is MemberExpression parent)
    //    {
    //        var nav = NavigationMaps[typeof(Product)].FirstOrDefault(n => n.Property == parent.Member.Name);

    //        if (nav != null)
    //        {
    //            _joins.Add(
    //                $"LEFT JOIN {nav.Table} {nav.Alias} ON {nav.JoinCondition}"
    //            );

    //            _sql.Append($"{nav.Alias}.{node.Member.Name}");
    //            return node;
    //        }
    //    }

    //    _sql.Append(node.Member.Name);
    //    return node;
    //}



    protected override Expression VisitBinary(BinaryExpression node)
    {
        _sql.Append("(");
        Visit(node.Left);

        _sql.Append(node.NodeType switch
        {
            ExpressionType.AndAlso => " AND ",
            ExpressionType.OrElse => " OR ",
            ExpressionType.Equal => " = ",
            ExpressionType.NotEqual => " <> ",
            ExpressionType.GreaterThan => " > ",
            ExpressionType.GreaterThanOrEqual => " >= ",
            ExpressionType.LessThan => " < ",
            ExpressionType.LessThanOrEqual => " <= ",
            _ => throw new NotSupportedException($"Operator {node.NodeType} not supported")
        });

        Visit(node.Right);
        _sql.Append(")");
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        // Boolean property: p => p.IsActive
        if (node.Type == typeof(bool))
        {
            _sql.Append($"{node.Member.Name} = 1");

            return node;
        }

        _sql.Append(node.Member.Name);

        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        var param = $"@p{_index++}";
        _sql.Append(param);
        _params.Add(param, node.Value);
        return node;
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.Name == "Contains")
        {
            Visit(node.Object!);
            var param = $"@p{_index++}";
            _sql.Append($" LIKE {param}");
            _params.Add(param, $"%{((ConstantExpression)node.Arguments[0]).Value}%");
            return node;
        }

        throw new NotSupportedException($"Method {node.Method.Name} not supported");
    }
}
