
namespace Applications.Responses.Paginations;

public record struct Pager<T>(List<T> Items, int Count = 0)
{
    public bool IsSuccess { get; set; }
}