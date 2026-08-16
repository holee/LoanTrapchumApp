using Applications.Features.Ledgers.Dto;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Ledgers.Queries.GetLedgerForUpdateById;

public record GetLedgerForUpdateByIdQuery(int? LedgerId) : IRequest<ErrorOr<LedgerUpdateDto>>;
