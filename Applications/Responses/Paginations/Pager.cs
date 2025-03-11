namespace Applications.Responses.Paginations;

public record struct Pager<T>(List<T> Items, int Count);

public record struct PageWrapper<T, TV>(List<T> Items, int Count, TV NextCursor);