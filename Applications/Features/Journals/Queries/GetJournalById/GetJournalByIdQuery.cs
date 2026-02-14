using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Journals.Queries.GetJournalById;

public record GetJournalByIdQuery(string? JournalId) : IRequest<ErrorOr<JournalEntry>>;
