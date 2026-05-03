using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Utils;

/// <summary>
/// Represents a paginated result set, similar to Laravel's LengthAwarePaginator.
/// </summary>
public sealed class PaginatedResult<T>
{
    public IReadOnlyList<T> Data { get; init; } = [];
    public int CurrentPage { get; init; }
    public int PerPage { get; init; }
    public int Total { get; init; }
    public int LastPage => (int)Math.Ceiling(Total / (double)PerPage);
    public bool HasNextPage => CurrentPage < LastPage;
    public bool HasPrevPage => CurrentPage > 1;
    public int From => Total == 0 ? 0 : (CurrentPage - 1) * PerPage + 1;
    public int To => Math.Min(CurrentPage * PerPage, Total);
}

public static class EFExtensionUtil
{
    /// <summary>
    /// Adds an OR WHERE clause to the query if the condition is true.
    /// </summary>
    public static IQueryable<T> OrWhereIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        if (!condition) return query;

        var existing = query.Expression;
        var parameter = predicate.Parameters[0];

        // Combine existing where with OR predicate
        // We rebuild: query.Where(existingPredicate || predicate)
        // Since IQueryable doesn't expose its predicate directly,
        // we union the filtered and unfiltered sets then distinct — 
        // but the idiomatic approach is to build an OR over the two predicates.
        // Here we use a union-based OR: existing OR new predicate.
        return query.Union(query.Provider.CreateQuery<T>(
            Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Where),
                [typeof(T)],
                query.Expression,
                Expression.Quote(predicate)
            )
        ));
    }

    /// <summary>
    /// Adds an OR WHERE Contains clause if the condition is true and value is not null/empty.
    /// </summary>
    public static IQueryable<T> OrWhereContainIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, string>> selector,
        string value)
    {
        if (!condition || string.IsNullOrEmpty(value)) return query;

        var predicate = BuildStringPredicate<T>(selector, value, nameof(string.Contains));
        return query.OrWhereIf(true, predicate);
    }

    /// <summary>
    /// Adds an OR WHERE StartsWith clause if the condition is true and value is not null/empty.
    /// </summary>
    public static IQueryable<T> OrWhereStartWithIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, string>> selector,
        string value)
    {
        if (!condition || string.IsNullOrEmpty(value)) return query;

        var predicate = BuildStringPredicate<T>(selector, value, nameof(string.StartsWith));
        return query.OrWhereIf(true, predicate);
    }

    /// <summary>
    /// Adds an OR WHERE EndsWith clause if the condition is true and value is not null/empty.
    /// </summary>
    public static IQueryable<T> OrWhereEndWithIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, string>> selector,
        string value)
    {
        if (!condition || string.IsNullOrEmpty(value)) return query;

        var predicate = BuildStringPredicate<T>(selector, value, nameof(string.EndsWith));
        return query.OrWhereIf(true, predicate);
    }

    /// <summary>
    /// Adds an AND WHERE Contains clause if the condition is true and value is not null/empty.
    /// </summary>
    public static IQueryable<T> AndWhereContainIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, string>> selector,
        string value)
    {
        if (!condition || string.IsNullOrEmpty(value)) return query;

        var predicate = BuildStringPredicate<T>(selector, value, nameof(string.Contains));
        return query.Where(predicate);
    }

    /// <summary>
    /// Adds an AND WHERE StartsWith clause if the condition is true and value is not null/empty.
    /// </summary>
    public static IQueryable<T> AndWhereStartWithIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, string>> selector,
        string value)
    {
        if (!condition || string.IsNullOrEmpty(value)) return query;

        var predicate = BuildStringPredicate<T>(selector, value, nameof(string.StartsWith));
        return query.Where(predicate);
    }

    /// <summary>
    /// Adds an AND WHERE EndsWith clause if the condition is true and value is not null/empty.
    /// </summary>
    public static IQueryable<T> AndWhereEndWithIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, string>> selector,
        string value)
    {
        if (!condition || string.IsNullOrEmpty(value)) return query;

        var predicate = BuildStringPredicate<T>(selector, value, nameof(string.EndsWith));
        return query.Where(predicate);
    }

    /// <summary>
    /// Paginates the query synchronously, like Laravel's paginate().
    /// </summary>
    /// <param name="query">The source query.</param>
    /// <param name="page">1-based page number (defaults to 1).</param>
    /// <param name="perPage">Items per page (defaults to 15).</param>
    public static PaginatedResult<T> Paginate<T>(
        this IQueryable<T> query,
        int page = 1,
        int perPage = 15)
    {
        page = Math.Max(1, page);
        perPage = Math.Max(1, perPage);

        var total = query.Count();
        var data = query
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToList();

        return new PaginatedResult<T>
        {
            Data = data,
            CurrentPage = page,
            PerPage = perPage,
            Total = total,
        };
    }

    /// <summary>
    /// Paginates the query asynchronously, like Laravel's paginate().
    /// </summary>
    /// <param name="query">The source query.</param>
    /// <param name="page">1-based page number (defaults to 1).</param>
    /// <param name="perPage">Items per page (defaults to 15).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public static async Task<PaginatedResult<T>> PaginateAsync<T>(
        this IQueryable<T> query,
        int page = 1,
        int perPage = 15,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(1, page);
        perPage = Math.Max(1, perPage);

        var total = await query.CountAsync(cancellationToken);
        var data = await query
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<T>
        {
            Data = data,
            CurrentPage = page,
            PerPage = perPage,
            Total = total,
        };
    }

    private static Expression<Func<T, bool>> BuildStringPredicate<T>(
        Expression<Func<T, string>> selector,
        string value,
        string methodName)
    {
        var method = typeof(string).GetMethod(methodName, [typeof(string)])
            ?? throw new InvalidOperationException($"Method '{methodName}' not found on string.");

        var parameter = selector.Parameters[0];
        var body = Expression.Call(selector.Body, method, Expression.Constant(value));
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

