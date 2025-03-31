using Applications.Responses;

namespace Applications.Services.LIsts
{
    public interface IBaseServiceImpl<T> where T : class
    {
        Task<Result<string>> CreateAsync(T entity);
        Task<Result<string>> UpdateAsync(T entity);
        Task<Result<string>> DeleteAsync<U>(U? id);
        Task<Result<T>> GetByIdAsync<U>(U? id);
        Task<Result<IEnumerable<T>>> GetsAsync<U>(U? id, string? term = null);
    }
}
