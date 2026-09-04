using ErrorOr;

namespace Application.Features.Journals.Queries.IsJournalNumberExist;

public record IsJournalNumberExistQuery(string? JournalNumber, int? BranchId) : IRequest<ErrorOr<bool>>;
