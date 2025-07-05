namespace Infrastructures.DataAccess
{
    using System.Linq.Expressions;
    using System.Reflection;

    public static class QueryableExtensions
    {

        public static IQueryable<T> SelectBy<T>(this IQueryable source, params string[] properties) where T : class, new()
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (properties == null || !properties.Any())
                throw new ArgumentException("At least one property must be specified", nameof(properties));

            // Get the type of the source items
            Type sourceType = source.ElementType;

            // Create parameter expression for the source type
            ParameterExpression parameter = Expression.Parameter(sourceType, "x");

            // Create new instance of T
            NewExpression newExpression = Expression.New(typeof(T));

            // Create property bindings
            var bindings = new List<MemberBinding>();
            PropertyInfo[] targetProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (string propertyName in properties)
            {
                // Find matching property in source
                PropertyInfo? sourceProperty = sourceType.GetProperty(propertyName,BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (sourceProperty == null)
                    throw new ArgumentException($"Property '{propertyName}' not found on type {sourceType.Name}");

                // Find matching property in target
                PropertyInfo? targetProperty = targetProperties.FirstOrDefault(p =>
                    p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase) &&
                    p.PropertyType == sourceProperty.PropertyType);

                if (targetProperty == null)
                    throw new ArgumentException($"Property '{propertyName}' with matching type not found on type {typeof(T).Name}");

                // Create property binding
                MemberExpression propertyExpression = Expression.Property(parameter, sourceProperty);

                bindings.Add(Expression.Bind(targetProperty, propertyExpression));
            }

            // Create member init expression
            MemberInitExpression memberInit = Expression.MemberInit(newExpression, bindings);

            // Create lambda expression
            LambdaExpression lambda = Expression.Lambda(memberInit, parameter);

            // Create the query using EF Core's Select method
            var selectCall = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Select),
                new[] { sourceType, typeof(T) },
                source.Expression,
                Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(selectCall);
        }



        public static IQueryable<T> OrWhere<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            // Get the existing Where expression from the source query if it exists
            Expression combinedPredicate = predicate.Body;

            if (source.Expression is MethodCallExpression methodCall &&
                methodCall.Method.Name == nameof(Queryable.Where))
            {
                // Extract the existing predicate from the Where method
                var existingLambda = (LambdaExpression)((UnaryExpression)methodCall.Arguments[1]).Operand;

                // Combine with OR
                var parameter = predicate.Parameters[0];
                var rebinder = new ParameterRebinder(parameter);
                var existingBody = rebinder.ReplaceParameters(existingLambda.Body);
                combinedPredicate = Expression.Or(existingBody, predicate.Body);
            }

            // Create new lambda with combined predicate
            var lambda = Expression.Lambda<Func<T, bool>>(combinedPredicate, predicate.Parameters);

            // Apply the combined predicate
            return source.Provider.CreateQuery<T>(
                Expression.Call(
                    typeof(Queryable),
                    nameof(Queryable.Where),
                    new[] { typeof(T) },
                    source.Expression,
                    Expression.Quote(lambda)));
        }

        public static IQueryable<T> AndWhere<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source)); 

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            // Get the existing Where expression from the source query if it exists
            Expression combinedPredicate = predicate.Body;

            if (source.Expression is MethodCallExpression methodCall &&
                methodCall.Method.Name == nameof(Queryable.Where))
            {
                // Extract the existing predicate from the Where method
                var existingLambda = (LambdaExpression)((UnaryExpression)methodCall.Arguments[1]).Operand;

                // Combine with OR
                var parameter = predicate.Parameters[0];
                var rebinder = new ParameterRebinder(parameter);
                var existingBody = rebinder.ReplaceParameters(existingLambda.Body);
                combinedPredicate = Expression.And(existingBody, predicate.Body);
            }

            // Create new lambda with combined predicate
            var lambda = Expression.Lambda<Func<T, bool>>(combinedPredicate, predicate.Parameters);

            // Apply the combined predicate
            return source.Provider.CreateQuery<T>(
                Expression.Call(
                    typeof(Queryable),
                    nameof(Queryable.Where),
                    new[] { typeof(T) },
                    source.Expression,
                    Expression.Quote(lambda)));
        }

        // Helper class to rebind parameters in expression trees
        private class ParameterRebinder : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;

            public ParameterRebinder(ParameterExpression parameter)
            {
                _parameter = parameter;
            }

            public Expression ReplaceParameters(Expression expression)
            {
                return Visit(expression);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return _parameter;
            }
        }
    }

}
