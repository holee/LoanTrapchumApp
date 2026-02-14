namespace Applications.Commons;

public interface ICollectionResponse<T>
{
    List<T> Items { get; init; }
}