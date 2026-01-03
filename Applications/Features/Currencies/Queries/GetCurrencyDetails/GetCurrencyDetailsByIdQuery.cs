using Applications.Features.Currencies.Responses;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Currencies.Queries.GetCurrencyDetails;

public record GetCurrencyDetailsByIdQuery(int? Id) : IRequest<ErrorOr<VCurrenciesList>>;
