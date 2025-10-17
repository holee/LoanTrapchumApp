using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructures.Implements.Extensions;

public static class EfProjectionHelper
{
    private static readonly ConcurrentDictionary<(Type, Type), LambdaExpression> _cache = new();

    public static Expression<Func<TSource, TDestination>> CreateProjection<TSource, TDestination>()
        where TDestination : class, new()
    {
        var key = (typeof(TSource), typeof(TDestination));

        if (_cache.TryGetValue(key, out var cached))

            return (Expression<Func<TSource, TDestination>>)cached;

        var parameter = Expression.Parameter(typeof(TSource), "x");

        var body = BuildMemberInit(typeof(TDestination), typeof(TSource), parameter);

        var lambda = Expression.Lambda<Func<TSource, TDestination>>(body, parameter);

        _cache[key] = lambda;

        return lambda;
    }

    private static MemberInitExpression BuildMemberInit(Type destType, Type srcType, Expression srcExpression)
    {
        var bindings = new List<MemberBinding>();

        foreach (var destProp in destType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!destProp.CanWrite) continue;

            Expression valueExpr = TryGetSourceExpression(destProp, srcType, srcExpression)!;

            if (valueExpr != null)

                bindings.Add(Expression.Bind(destProp, valueExpr));
        }

        return Expression.MemberInit(Expression.New(destType), bindings);
    }

    private static Expression? TryGetSourceExpression(PropertyInfo destProp, Type srcType, Expression srcExpr)
    {
        // 1️⃣ Try direct match
        var srcProp = srcType.GetProperty(destProp.Name);

        if (srcProp != null && srcProp.CanRead)
        {
            var propExpr = Expression.Property(srcExpr, srcProp);

            if (IsSimpleType(srcProp.PropertyType) && destProp.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                return propExpr;
        }

        // 2️⃣ Try nested match (e.g., Address.City)
        foreach (var srcNestedProp in srcType.GetProperties().Where(p => !IsSimpleType(p.PropertyType)))
        {
            var innerExpr = Expression.Property(srcExpr, srcNestedProp);

            var match = srcNestedProp.PropertyType.GetProperty(destProp.Name);

            if (match != null && match.CanRead && IsSimpleType(match.PropertyType))
            {
                return Expression.Property(innerExpr, match);
            }
        }

        return null;
    }

    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive ||
               new[]
               {
                   typeof(string), typeof(decimal), typeof(DateTime),
                   typeof(DateTimeOffset), typeof(TimeSpan), typeof(Guid)
               }.Contains(type) ||
               Convert.GetTypeCode(type) != TypeCode.Object;
    }





    public static Expression<Func<TSource, TDestination>> ProjectTo<TSource, TDestination>() where TSource : class where TDestination : class, new()
    {
        var sourceParam = Expression.Parameter(typeof(TSource), "src");

        var sourceProps = typeof(TSource)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead)
            .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

        var destinationBindings = new List<MemberBinding>();

        foreach (var destProp in typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!destProp.CanWrite) continue;

            if (sourceProps.TryGetValue(destProp.Name, out var sourceProp))
            {
                
                if (sourceProp.PropertyType == destProp.PropertyType)
                {
                    var sourceValue = Expression.Property(sourceParam, sourceProp);
                    destinationBindings.Add(Expression.Bind(destProp, sourceValue));
                }
                else if (IsConvertible(sourceProp.PropertyType, destProp.PropertyType))
                {
                    var sourceValue = Expression.Property(sourceParam, sourceProp);
                    var converted = Expression.Convert(sourceValue, destProp.PropertyType);
                    destinationBindings.Add(Expression.Bind(destProp, converted));
                }
                else
                {
                    destinationBindings.Add(Expression.Bind(destProp, Expression.Default(destProp.PropertyType)));
                }
            }
            else
            {
                destinationBindings.Add(Expression.Bind(destProp, Expression.Default(destProp.PropertyType)));
            }
        }

        var body = Expression.MemberInit(Expression.New(typeof(TDestination)), destinationBindings);

        return Expression.Lambda<Func<TSource, TDestination>>(body, sourceParam);
    }


    private static bool IsConvertible(Type sourceType, Type destinationType)
    {
        if (destinationType.IsAssignableFrom(sourceType))
            return true;

        try
        {
            var converted = Expression.Convert(Expression.Default(sourceType), destinationType);
            return converted != null;
        }
        catch
        {
            return false;
        }
    }
}


//public async Task<Pager<VCustomerList>> GetCustomersAsync(QueryParam q, CancellationToken cancellation = default)
//{

//    await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);

//    IQueryable<VCustomerList> queries = cxt.VCustomerLists.OrderByDescending(x => x.ModifiedDT);

//    var counts = queries.Count();

//    if (!q.IsIncludeInActive)
//    {
//        queries = queries.Where(x => x.IsActive == true);
//    }

//    if (!string.IsNullOrEmpty(q.Filter))
//    {
//        queries = queries.Where(x => (EF.Functions.Like(x.FullName!, $"%{q.Filter}%") ||
//                                      EF.Functions.Like(EF.Functions.Collate(x.CustomerId, "Khmer_100_CI_AI_SC_UTF8"),
//                                          $"%{q.Filter}%") ||
//                                      EF.Functions.Like(x.AltName!, $"%{q.Filter}%") ||
//                                      EF.Functions.Like(x.Phone!, $"%{q.Filter}%")));
//    }

//    if (q.SortDescending)
//    {
//        queries = queries.OrderByDescending(x => EF.Property<object>(x, q.SortColumn));
//    }

//    var count = queries.Count();

//    var customers = queries.AsNoTracking().Skip(q.Skip).Take(q.PageSize);

//    return new Pager<VCustomerList>
//    {
//        Items = await customers.ToListAsync(),
//        Count = count,
//    };
//}

//public async Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string? customerId, bool include = false)
//{
//    try
//    {
//        using var db = await dapperFactory.CreateConnectionAsync();

//        if (!include)
//        {
//            var customer = await db.QueryFirstOrDefaultAsync<VCustomerList>("spGetCustomerDetailsById",
//                new
//                {
//                    customerId = customerId,
//                    include = include
//                }, commandType: CommandType.StoredProcedure);

//            if (customer is null)
//            {
//                return Result.Failure<VCustomerList>(
//                    ErrorResponse.NotFound("no data found."));
//            }

//            return Result.Success(customer);
//        }

//        var coustmerDictionary = new Dictionary<string, VCustomerList>();
//        //"spGetCustomerDetailsById"
//        var customerInclude = await db.QueryAsync(ProcedureConstant.SpGetCustomerDetailsById,
//            (VCustomerList customer, VLoanDisbursments loan, VLoanRequests loanRequest,
//                VCollateralLines collateralLine, VCustomerIncomes customerIncome, VCustomerExpenses customerExpense,
//                VCustomerNotes customerNote) =>
//            {
//                if (coustmerDictionary.TryGetValue(customer.CustomerId!, out var existingCustomer))
//                {
//                    customer = existingCustomer;
//                }
//                else
//                {
//                    coustmerDictionary.Add(customer.CustomerId!, customer);
//                }

//                customer.Loans.Add(loan);
//                customer.LoanRequests.Add(loanRequest);
//                customer.CollateralLines.Add(collateralLine!);
//                customer.CustomerExpenses.Add(customerExpense);
//                customer.CustomerIncomes.Add(customerIncome);
//                customer.CustomerNotes.Add(customerNote);
//                return customer;
//            },
//            new
//            {
//                customerId = customerId,
//                include = include
//            },
//            commandType: CommandType.StoredProcedure,
//            splitOn: "Id,LoanRequestId,CustomerRefId,IncomeId,ExpendId,NoteId");


//        if (!customerInclude.Any())
//            return Result.Failure<VCustomerList>(ErrorResponse.NotFound("No note found."));

//        var item = customerInclude.First();

//        return Result.Success(item);
//    }
//    catch (SqlException ex)
//    {
//        return Result.Failure<VCustomerList>(
//            ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
//    }
//}



//public async Task<Result<IEnumerable<T>>> GetCustomersAsync<T>(string? customerId, int take = 0,
//    bool includeInactive = false,
//    bool exceptActiveLoan = true,
//    string? searchTerm = null) where T : class
//{
//    await using var context =await contextFactory.CreateDbContextAsync();
//    IQueryable<VCustomerList> customerQueries = context.VCustomerLists;

//    if (includeInactive)
//    {
//        customerQueries = customerQueries.IgnoreQueryFilters();
//    }

//    if (exceptActiveLoan)
//    {
//        customerQueries = customerQueries.Where(x => x.IsLoanActive == false);
//    }

//    if (!string.IsNullOrEmpty(searchTerm))
//    {
//        customerQueries = customerQueries.Where(x => (EF.Functions.Like(x.FullName!, $"%{searchTerm}%") ||
//                                                      EF.Functions.Like(x.AltName!, $"%{searchTerm}%") ||
//                                                      EF.Functions.Like(
//                                                          EF.Functions.Collate(x.CustomerId!.ToString(),
//                                                              "Khmer_100_CI_AI_SC_UTF8"), $"%{searchTerm}%") ||
//                                                      EF.Functions.Like(x.Phone!, $"%{searchTerm}%")));
//    }

//    if (customerId.IsNotNullOrEmpty())
//    {
//        customerQueries = customerQueries.Where(x => x.CustomerId == customerId);
//    }

//    if (take > 0)
//    {
//        customerQueries = customerQueries.Take(take).OrderBy(x => x.CustomerId);
//    }

//    if (!customerQueries.Any())
//    {
//        return Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("no customer found."));
//    }

//    var results = await customerQueries
//        .ProjectToType<T>()
//        .AsNoTracking()
//        .ToListAsync();
//    return Result.Success<IEnumerable<T>>(results);
//}
//public async Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string? customerId, CancellationToken cancellationToken = default)
//{
//    logger.LogInformation("Fetching customer details for ID {CustomerId}", customerId);

//    await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

//    var customer = await context.VCustomerLists
//                        .Include(x => x.CustomerActivities)
//                        .Include(x => x.CustomerExpenses)
//                        .Include(x => x.CustomerIncomes)
//                        .Include(x => x.CustomerNotes)
//                        .Include(x => x.CollateralLines)
//                        .Include(x => x.LoanRequests)
//                        .Include(x => x.Loans)
//                        .AsSplitQuery()
//                        .AsNoTracking()
//                        .FirstOrDefaultAsync(x => x.CustomerId == customerId, cancellationToken);

//    if (customer is null)
//        return Result.Failure<VCustomerList>(ErrorResponse.NotFound("Customer Not Found."));

//    return Result.Success(customer);

//}
