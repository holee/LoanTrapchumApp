using Domains.Models.Journals;
using ErrorOr;

namespace Application.Features.Journals.Queries.GetJournalById;

public record GetJournalByIdQuery(string? JournalId) : IRequest<ErrorOr<JournalEntry>>;
