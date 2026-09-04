using Radzen;

namespace Web.UI.Helpers;

public static class RowRenderHelper
{
    public static void Apply<T>(RowRenderEventArgs<T> args,
        int pageIndex,
        int pageSize,
        Action<T, int>? setRowIndex = null,
        IEnumerable<RowClassRule<T>>? classRules = null)
    {
        if (args.Data is null)
        {
            return;
        }

        var absoluteRowIndex = (pageIndex * pageSize) + args.Index + 1;

        setRowIndex?.Invoke(args.Data, absoluteRowIndex);

        if (classRules is null)
        {
            return;
        }

        foreach (var rule in classRules)
        {
            if (rule.Predicate(args.Data))
            {
                AddCssClass(args.Attributes, rule.CssClass);
            }
        }
    }

    public static bool ContainsAny(
        string? searchText,
        params string?[] values)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return false;
        }

        var term = searchText.Trim();

        return values.Any(value =>
            value?.Contains(
                term,
                StringComparison.OrdinalIgnoreCase) == true);
    }

    private static void AddCssClass(
        IDictionary<string, object> attributes,
        string cssClass)
    {
        if (string.IsNullOrWhiteSpace(cssClass))
        {
            return;
        }

        var existingClasses = attributes.TryGetValue("class", out var value)
            ? value?.ToString()
            : null;

        var classes = string.IsNullOrWhiteSpace(existingClasses)
            ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            : existingClasses
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var className in cssClass.Split(
                     ' ',
                     StringSplitOptions.RemoveEmptyEntries))
        {
            classes.Add(className);
        }

        attributes["class"] = string.Join(" ", classes);
    }
}

public sealed record RowClassRule<T>(
    Func<T, bool> Predicate,
    string CssClass);