using Domains.Models.Transaction;
using ErrorOr;

namespace Applications.Features.Journals.Queries.GetJournalEntryForUpdate; 

public record GetJournalEntryForUpdateByIdQuery(string? TransactionId,int? BranchId) : IRequest<ErrorOr<JournalTransactionDtoUpdate>>;  
