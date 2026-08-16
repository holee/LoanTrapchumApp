using Applications.Features.Ledgers.Dto;
using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Ledgers.Queries.GetLedgerDetailsById;

public record GetLedgerDetailsByIdQuery(int? LedgerId, params string[] columns) : IRequest<ErrorOr<VLedger?>>;
