using Applications.Responses;
using System.Linq.Expressions;

namespace Applications.Interfaces.Employees
{
    public interface IBaseService<T> where T : class, new()
    {
        Task<Result<string>> CreateAsync(T entity); 

        Task<Result<string>> UpdateAsync(T entity);

        Task<Result<T>> GetByIdAsync<U>(U? id);

        Task<Result<T>> GetAsync(Expression<Func<T,bool>> pridicate); 

        Task<Result<bool>> DeleteByIdAsync<U>(U? id);  
    }
}
