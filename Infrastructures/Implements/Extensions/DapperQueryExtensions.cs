using Dapper;
using System.Data;

namespace Infrastructures.Implements.Extensions;

public static class DapperQueryExtensions
{
    /// <summary>
    /// Executes two queries in a single batch and maps the results into a parent object with a list of children.
    /// </summary>
    public static async Task<TParent?> QueryParentWithChildrenAsync<TParent, TChild>(
        this IDbConnection connection,
        string sql,
        object? param,
        Action<TParent, IEnumerable<TChild>> mapChildren,
        CancellationToken cancellationToken = default)
    {
        var grid = await connection.QueryMultipleAsync(
            sql,
            param,
            commandType: CommandType.Text
        );

        var parent = await grid.ReadFirstOrDefaultAsync<TParent>();

        if (parent is null)
            return default;

        var children = (await grid.ReadAsync<TChild>()).ToList();

        mapChildren(parent, children);

        return parent;
    }


    /// <summary>
    /// Executes multiple SELECT statements in one round-trip and allows mapping each result set.
    /// </summary>
    public static async Task<TParent?> QueryWithMultipleChildrenAsync<TParent>(
        this IDbConnection connection,
        string sql,
        object? param,
        Func<SqlMapper.GridReader, Task<TParent?>> map,
        CancellationToken cancellationToken = default)
    {
        var grid = await connection.QueryMultipleAsync(
            sql,
            param,
            commandType: CommandType.Text
        );

        return await map(grid);
    }
}


//public async Task<ErrorOr<VCollateralLines>> GetCollateralLineDetailAsync(
//    long? id,
//    string? collateralId,
//    CancellationToken cancellation = default)
//    {
//        using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

//        const string sql = @"
//        SELECT * 
//        FROM VCollateralLines 
//        WHERE Id = @Id AND CollateralId = @CollateralId;

//        SELECT 
//            d.LoanId,
//            d.InstallmentType,
//            d.Principle,
//            d.PeriodName,
//            d.InterestRate,
//            d.PaidPrinciple,
//            d.PaidInterest,
//            d.OutStanding,
//            d.BranchName,
//            d.LoanStatus
//        FROM VLoansDisbursments AS d
//        INNER JOIN VLoanRequestCollaterals AS r
//            ON d.LoanRequestId = r.LoanRequestId
//        WHERE r.CollateralLineId = @Id AND r.CollateralId = @CollateralId;
//    ";

//        var item = await connection.QueryParentWithChildrenAsync<VCollateralLines, VLoanDisbursments>(
//            sql,
//            new { Id = id, CollateralId = collateralId },
//            (parent, children) => parent.LoanDisbursements = children,
//            cancellation
//        );

//        if (item is null)
//            return Error.NotFound(CollateralMessages.NotFound);

//        return item;
//    }


//var loan = await connection.QueryParentWithChildrenAsync<VLoan, VInstallment>(
//    @"
//        SELECT * FROM VLoans WHERE LoanId = @Id;
//        SELECT * FROM VInstallments WHERE LoanId = @Id;
//    ",
//    new { Id = "LN-0001" },
//    (parent, children) => parent.Installments = children
//);



//public async Task<ErrorOr<VCollateralLines>> GetCollateralLineDetailAsync(
//    long? id,
//    string? collateralId,
//    CancellationToken cancellation = default)
//    {
//        using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

//        const string sql = @"
//        SELECT * 
//        FROM VCollateralLines 
//        WHERE Id = @Id AND CollateralId = @CollateralId;

//        SELECT 
//            d.LoanId,
//            d.InstallmentType,
//            d.Principle,
//            d.PeriodName,
//            d.InterestRate,
//            d.PaidPrinciple,
//            d.PaidInterest,
//            d.OutStanding,
//            d.BranchName,
//            d.LoanStatus
//        FROM VLoansDisbursments AS d
//        INNER JOIN VLoanRequestCollaterals AS r
//            ON d.LoanRequestId = r.LoanRequestId
//        WHERE r.CollateralLineId = @Id 
//          AND r.CollateralId = @CollateralId;

//        SELECT 
//            l.LinkId,
//            l.LinkType,
//            l.CreatedDate
//        FROM lt_collateralLinks l
//        WHERE l.CollateralLineRefId = @Id 
//          AND l.IsActive = 1;
//    ";

//        var result = await connection.QueryWithMultipleChildrenAsync<VCollateralLines>(
//            sql,
//            new { Id = id, CollateralId = collateralId },
//            async grid =>
//            {
//                // 1️⃣ Parent
//                var parent = await grid.ReadFirstOrDefaultAsync<VCollateralLines>();
//                if (parent is null)
//                    return null;

//                // 2️⃣ First child collection
//                var loanDisbursements = (await grid.ReadAsync<VLoanDisbursments>()).ToList();
//                parent.LoanDisbursements = loanDisbursements;

//                // 3️⃣ Second child collection
//                var links = (await grid.ReadAsync<VCollateralLink>()).ToList();
//                parent.CollateralLinks = links;

//                return parent;
//            },
//            cancellation
//        );

//        if (result is null)
//            return Error.NotFound(CollateralMessages.NotFound);

//        return result;
//    }


//var order = await connection.QueryWithMultipleChildrenAsync<Order>(
//    @"
//        SELECT * FROM Orders WHERE Id=@Id;
//        SELECT * FROM OrderItems WHERE OrderId=@Id;
//        SELECT * FROM Payments WHERE OrderId=@Id;
//        SELECT * FROM Shipments WHERE OrderId=@Id;
//    ",
//    new { Id = orderId },
//    async grid =>
//    {
//        var parent = await grid.ReadFirstOrDefaultAsync<Order>();
//        if (parent is null) return null;

//        parent.Items = (await grid.ReadAsync<OrderItem>()).ToList();
//        parent.Payments = (await grid.ReadAsync<Payment>()).ToList();
//        parent.Shipments = (await grid.ReadAsync<Shipment>()).ToList();

//        return parent;
//    }
//);

//public static async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>)> QueryMultipleAsync<T1, T2, T3>(
//    this IDbConnection connection,
//    string sql,
//    object? param = null,
//    CancellationToken cancellationToken = default)
//    {
//        var grid = await connection.QueryMultipleAsync(sql, param, cancellationToken: cancellationToken);

//        var result1 = await grid.ReadAsync<T1>();
//        var result2 = await grid.ReadAsync<T2>();
//        var result3 = await grid.ReadAsync<T3>();

//        return (result1, result2, result3);
//    }}



//public static class DapperQueryExtensions
//{
//    public static async Task<(IEnumerable<T1> Set1, IEnumerable<T2> Set2)>
//        QueryMultipleAsync<T1, T2>(
//        this IDbConnection connection,
//        string sql,
//        object? param = null,
//        CancellationToken cancellationToken = default)
//    {
//        using var grid = await connection.QueryMultipleAsync(
//            sql,
//            param,
//            commandType: CommandType.Text,
//            cancellationToken: cancellationToken
//        );

//        var set1 = await grid.ReadAsync<T1>();
//        var set2 = await grid.ReadAsync<T2>();

//        return (set1, set2);
//    }

//    public static async Task<(IEnumerable<T1> Set1, IEnumerable<T2> Set2, IEnumerable<T3> Set3)>
//        QueryMultipleAsync<T1, T2, T3>(
//        this IDbConnection connection,
//        string sql,
//        object? param = null,
//        CancellationToken cancellationToken = default)
//    {
//        using var grid = await connection.QueryMultipleAsync(
//            sql,
//            param,
//            commandType: CommandType.Text,
//            cancellationToken: cancellationToken
//        );

//        var set1 = await grid.ReadAsync<T1>();
//        var set2 = await grid.ReadAsync<T2>();
//        var set3 = await grid.ReadAsync<T3>();

//        return (set1, set2, set3);
//    }

//    public static async Task<(IEnumerable<T1> Set1, IEnumerable<T2> Set2, IEnumerable<T3> Set3, IEnumerable<T4> Set4)>
//        QueryMultipleAsync<T1, T2, T3, T4>(
//        this IDbConnection connection,
//        string sql,
//        object? param = null,
//        CancellationToken cancellationToken = default)
//    {
//        using var grid = await connection.QueryMultipleAsync(
//            sql,
//            param,
//            commandType: CommandType.Text,
//            cancellationToken: cancellationToken
//        );

//        var set1 = await grid.ReadAsync<T1>();
//        var set2 = await grid.ReadAsync<T2>();
//        var set3 = await grid.ReadAsync<T3>();
//        var set4 = await grid.ReadAsync<T4>();

//        return (set1, set2, set3, set4);
//    }

//    public static async Task<(IEnumerable<T1> Set1, IEnumerable<T2> Set2, IEnumerable<T3> Set3, IEnumerable<T4> Set4, IEnumerable<T5> Set5)>
//        QueryMultipleAsync<T1, T2, T3, T4, T5>(
//        this IDbConnection connection,
//        string sql,
//        object? param = null,
//        CancellationToken cancellationToken = default)
//    {
//        using var grid = await connection.QueryMultipleAsync(
//            sql,
//            param,
//            commandType: CommandType.Text,
//            cancellationToken: cancellationToken
//        );

//        var set1 = await grid.ReadAsync<T1>();
//        var set2 = await grid.ReadAsync<T2>();
//        var set3 = await grid.ReadAsync<T3>();
//        var set4 = await grid.ReadAsync<T4>();
//        var set5 = await grid.ReadAsync<T5>();

//        return (set1, set2, set3, set4, set5);
//    }
//}

//public async Task<ErrorOr<object>> GetCollateralDetailAsync(
//    long? id,
//    string? collateralId,
//    CancellationToken cancellation = default)
//    {
//        using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

//        const string sql = @"
//        SELECT * 
//        FROM VCollateralLines 
//        WHERE Id = @Id AND CollateralId = @CollateralId;

//        SELECT 
//            d.LoanId,
//            d.InstallmentType,
//            d.Principle,
//            d.PeriodName,
//            d.InterestRate,
//            d.PaidPrinciple,
//            d.PaidInterest,
//            d.OutStanding,
//            d.BranchName,
//            d.LoanStatus
//        FROM VLoansDisbursments AS d
//        INNER JOIN VLoanRequestCollaterals AS r
//            ON d.LoanRequestId = r.LoanRequestId
//        WHERE r.CollateralLineId = @Id AND r.CollateralId = @CollateralId;

//        SELECT 
//            l.LinkId,
//            l.LinkType,
//            l.CreatedDate
//        FROM lt_collateralLinks l
//        WHERE l.CollateralLineRefId = @Id AND l.IsActive = 1;
//    ";

//        var (collaterals, disbursements, links) = await connection.QueryMultipleAsync<
//            VCollateralLines,
//            VLoanDisbursments,
//            VCollateralLink
//        >(sql, new { Id = id, CollateralId = collateralId }, cancellation);

//        var collateral = collaterals.FirstOrDefault();
//        if (collateral is null)
//            return Error.NotFound(CollateralMessages.NotFound);

//        // Assign relations
//        collateral.LoanDisbursements = disbursements.ToList();
//        collateral.CollateralLinks = links.ToList();

//        // You could also return a DTO combining all sets
//        return new
//        {
//            Collateral = collateral,
//            LoanDisbursements = collateral.LoanDisbursements,
//            Links = collateral.CollateralLinks
//        };
//    }

//var(loan, installments, payments, collaterals) = await connection.QueryMultipleAsync<
//    VLoan,
//    VInstallment,
//    VPayment,
//    VCollateral
//>(
//    @"
//        SELECT * FROM VLoans WHERE LoanId=@Id;
//        SELECT * FROM VInstallments WHERE LoanId=@Id;
//        SELECT * FROM VPayments WHERE LoanId=@Id;
//        SELECT * FROM VLoanCollaterals WHERE LoanId=@Id;
//    ",
//    new { Id = loanId },
//    cancellation
//);

//var dto = new
//{
//    Loan = loan.FirstOrDefault(),
//    Installments = installments,
//    Payments = payments,
//    Collaterals = collaterals
//};
