using Application.Responses;
using Domains.Models.Lists.Customers;
using Application.Responses.ViewModels.Customers;
using ErrorOr;

namespace Application.Interfaces.Customers
{
    public interface ICustomerNoteService
    {
        Task<ErrorOr<int>> AddOrUpdateNote(CustomerNote model, CancellationToken cancellation = default);

        Task<ErrorOr<bool>> DeleteNote(int? noteId,string? customerId, CancellationToken cancellation = default);

        Task<Result<IEnumerable<T>>> GetCustomerNotes<T>(string? customerId, CancellationToken cancellation = default) where T : class;


        Task<ErrorOr<IEnumerable<VCustomerNotes>>> GetNotes(string? customerId, CancellationToken cancellation = default);


        Task<ErrorOr<VCustomerNotes>> GetNoteById(int? id, string? customerId, CancellationToken cancellation = default);
    }
}
