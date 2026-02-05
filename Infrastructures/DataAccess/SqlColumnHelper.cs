using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructures.DataAccess
{
    public static class SqlColumnHelper
    {
        public static string BuildSafeColumnList<T>(params string[]? columns)
        {
            // no projection → select all
            if (columns == null || columns.Length == 0)
                return "*";

            var validColumns = typeof(T)
                .GetProperties()
                .Select(p => p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var safeColumns = columns
                .Where(c => validColumns.Contains(c))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (safeColumns.Length == 0)
                throw new ArgumentException("No valid columns provided.");

            return string.Join(",", safeColumns);
        }


        public static string BuildColumnList<T>(Expression<Func<T, object>> selector)
        {
            var props = new List<string>();

            if (selector.Body is NewExpression newExp)
            {
                foreach (var arg in newExp.Arguments)
                {
                    if (arg is MemberExpression member)
                        props.Add(member.Member.Name);
                }
            }
            else if (selector.Body is MemberExpression single)
            {
                props.Add(single.Member.Name);
            }

            return string.Join(",", props);
        }

    }
}
