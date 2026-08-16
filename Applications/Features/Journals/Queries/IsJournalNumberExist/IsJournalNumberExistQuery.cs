using ErrorOr;

namespace Applications.Features.Journals.Queries.IsJournalNumberExist;

public record IsJournalNumberExistQuery(string? JournalNumber, int? BranchId) : IRequest<ErrorOr<bool>>;
