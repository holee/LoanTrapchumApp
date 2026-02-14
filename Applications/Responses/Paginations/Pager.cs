
namespace Applications.Responses.Paginations;

public record struct Pager<T>(List<T> Items, int Count = 0)
{
    public bool IsSuccess { get; set; }
}

public static class PagerExtensions
{
    public static Pager<T> Create<T>(List<T> items, int count = 0) => new Pager<T>(items, count);

}