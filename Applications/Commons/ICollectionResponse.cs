namespace Applications.Commons;

public interface ICollectionResponse<T>
{
    IReadOnlyList<T> Items { get; init; }
}