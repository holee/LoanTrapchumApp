using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Journals.Queries.GetJournalForEdit;

public record GetJournalForEditByIdQuery(string? TranxId) : IRequest<ErrorOr<JournalEntryDtoUpdate>>;
