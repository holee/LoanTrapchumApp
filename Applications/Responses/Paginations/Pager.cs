namespace Applications.Responses.Paginations;

public record struct Pager<T>(List<T> Items, int Count = 0);
public record struct Paging<T>(IReadOnlyList<T> Items, int Count) where T : class;