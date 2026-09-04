namespace Application.Commons;

public interface ICollectionResponse<T>
{
    IReadOnlyList<T> Items { get; init; }
}