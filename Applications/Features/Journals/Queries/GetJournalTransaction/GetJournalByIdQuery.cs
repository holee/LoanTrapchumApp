using Domains.Models.Transaction;
using ErrorOr;

namespace Applications.Features.Journals.Queries.GetJournalTransaction; 

public record GetJournalTransactionByIdQuery(string? TransactionId,int? BranchId) : IRequest<ErrorOr<LoanTransaction>>;  
