namespace Infrastructures.DataAccess.Paging;

public record NavigationMap(
    string Property,
    string Table,
    string Alias,
    string JoinCondition
);
