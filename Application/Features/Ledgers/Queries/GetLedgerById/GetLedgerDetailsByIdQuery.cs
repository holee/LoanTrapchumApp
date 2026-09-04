using Application.Features.Ledgers.Dto;
using Domains.Models.Journals;
using ErrorOr;

namespace Application.Features.Ledgers.Queries.GetLedgerById;

public record GetLedgerByIdQuery(int? LedgerId, params string[] columns) : IRequest<ErrorOr<Ledger?>>;
