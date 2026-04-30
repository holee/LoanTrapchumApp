using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Ledgers.Queries.GetLedgerTypeById;

public record GetLedgerTypeByIdQuery(int? LedgerTypeId) : IRequest<ErrorOr<LedgerType>>;
