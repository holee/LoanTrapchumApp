using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Extensions
{
    public static class QueryableExtensions
    {
        // Conditionally applies a filter to the IQueryable source
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IQueryable<T> WhereIf<T, TParam>(this IQueryable<T> source, bool condition,
            Func<TParam, Expression<Func<T, bool>>> predicateBuilder, TParam param)
        {
            if (!condition)
                return source;

            var predicate = predicateBuilder(param);
            return source.Where(predicate);
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source,
            Dictionary<bool, Expression<Func<T, bool>>> conditions)
        {
            return conditions.Where(c => c.Key).Aggregate(source,
                (current, c) => current.Where(c.Value));
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition,
            Expression<Func<T, bool>> predicate, bool useOr)
        {
            if (!condition)
                return source;

            if (useOr)
            {
                // Combine with OR (requires expression tree manipulation)
                var parameter = predicate.Parameters[0];
                var orPredicate = Expression.Lambda<Func<T, bool>>(
                    Expression.OrElse(predicate.Body, Expression.Constant(true)),
                    parameter);
                return source.Where(orPredicate);
            }

            return source.Where(predicate);
        }
        
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, 
            params Expression<Func<T, bool>>[]? predicates)
        {
            if (!condition || predicates == null || !predicates.Any())
                return source;

            return predicates.Aggregate(source, (current, predicate) => current.Where(predicate));
        }

        public static async Task<IQueryable<T>> WhereIfAsync<T>(this IQueryable<T> source,
            Task<bool> conditionTask, Expression<Func<T, bool>> predicate)
        {
            bool condition = await conditionTask;
            return condition ? source.Where(predicate) : source;
        }

        public static IQueryable<T> IgnoreFilterIf<T>(this IQueryable<T> source, bool ignoreFilters = false)
            where T : class
        {
            return ignoreFilters ? source.IgnoreQueryFilters() : source;
        }

        // Conditionally retrieves the first element asynchronously based on a predicate
        public static async Task<T?> FirstIfAsync<T>(this IQueryable<T> source, bool condition,
            Expression<Func<T, bool>> predicate, CancellationToken cancellation = default)
        {
            if (condition)
            {
                return await source.FirstOrDefaultAsync(predicate, cancellation);
            }

            return await source.FirstOrDefaultAsync(cancellation);
        }

        // Conditionally applies an OrderByDescending or OrderBy to the IQueryable source
        public static IQueryable<T> OrderByDescendingIf<T, TKey>(this IQueryable<T> source, bool sortDescending,
            Expression<Func<T, TKey>> keySelector)
        {
            return sortDescending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }


        public static IQueryable<T> ApplySafeSorting<T>(this IQueryable<T> queries, DbContext context,
            string sortColumn, bool sortDescending)
        {
            if (string.IsNullOrEmpty(sortColumn))
                return queries;

            // Get the entity type from EF Core model
            var entityType = context.Model.FindEntityType(typeof(T));
            if (entityType == null)
                throw new InvalidOperationException($"Entity type {typeof(T).Name} not found in model.");

            // Check if the property exists
            var property = entityType.GetProperties()
                .FirstOrDefault(p => p.Name.Equals(sortColumn, StringComparison.OrdinalIgnoreCase));
            if (property == null)
            {
                throw new ArgumentException($"Invalid sort column: {sortColumn}");
                // Or fallback: return queries.OrderBy(e => EF.Property<object>(e, "Id"));
            }

            return sortDescending
                ? queries.OrderByDescending(o => EF.Property<object>(o!, sortColumn))
                : queries.OrderBy(o => EF.Property<object>(o!, sortColumn));
        }

        public static IQueryable<T> ApplySafeSorting<T>(this IQueryable<T> queries, string sortColumn,
            bool sortDescending)
        {
            if (string.IsNullOrEmpty(sortColumn))
                return queries; // Or apply a default sort

            // Validate the sort column
            var propertyInfo = typeof(T).GetProperty(sortColumn,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Invalid sort column: {sortColumn}");
                // Or return queries with a default sort: return queries.OrderBy(e => EF.Property<object>(e, "Id"));
            }

            // Apply sorting
            return sortDescending
                ? queries.OrderByDescending(o => EF.Property<object>(o, sortColumn))
                : queries.OrderBy(o => EF.Property<object>(o, sortColumn));
        }

        public static IQueryable<T> ApplySafeSorting<T>(this IQueryable<T> queries, string sortColumn,
            bool sortDescending, bool caseInsensitive)
        {
            if (string.IsNullOrEmpty(sortColumn))
                return queries;

            var param = Expression.Parameter(typeof(T), "o");
            var property = Expression.Property(param, sortColumn);
            Expression keySelectorBody = property;

            if (caseInsensitive && property.Type == typeof(string))
            {
                keySelectorBody = Expression.Call(property, nameof(string.ToLower), null);
            }

            var keySelector = Expression.Lambda<Func<T, object>>(
                Expression.Convert(keySelectorBody, typeof(object)),
                param);

            return sortDescending
                ? queries.OrderByDescending(keySelector)
                : queries.OrderBy(keySelector);
        }
        
        public static IQueryable<T> Search<T>(this IQueryable<T> source, string filter,
            params Expression<Func<T, string>>[] properties)
        {
            if (string.IsNullOrEmpty(filter))
                return source;

            // Sanitize filter (e.g., escape special characters)
            filter = filter.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
            
            filter = $"%{filter}%";

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? predicateBody = null;

            foreach (var property in properties)
            {
                var propertyExpression = Expression.Invoke(property, parameter);
                var likeExpression = Expression.Call(
                    typeof(EF).GetMethod("EF.Functions.Like", new[] { typeof(string), typeof(string) })!,
                    propertyExpression,
                    Expression.Constant(filter));

                predicateBody = predicateBody == null
                    ? likeExpression
                    : Expression.OrElse(predicateBody, likeExpression);
            }

            if (predicateBody == null)
                return source;

            var predicate = Expression.Lambda<Func<T, bool>>(predicateBody, parameter);
            return source.Where(predicate);
        }
        
        
        public static IQueryable<T> FullTextSearch<T>(this IQueryable<T> source, string filter,
            params Expression<Func<T, string>>[] properties)
        {
            if (string.IsNullOrEmpty(filter))
                return source;

            // Example for SQL Server CONTAINS (adjust for your DB)
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? predicateBody = null;

            foreach (var property in properties)
            {
                var propertyExpression = Expression.Invoke(property, parameter);
                var containsExpression = Expression.Call(
                    typeof(EF).GetMethod("EF.Functions.Contains", new[] { typeof(string), typeof(string) })!,
                    propertyExpression,
                    Expression.Constant(filter));

                predicateBody = predicateBody == null
                    ? containsExpression
                    : Expression.OrElse(predicateBody, containsExpression);
            }

            if (predicateBody == null)
                return source;

            var predicate = Expression.Lambda<Func<T, bool>>(predicateBody, parameter);
            return source.Where(predicate);
        }
        
    }
}