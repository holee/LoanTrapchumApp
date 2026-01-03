using Applications.Features.Currencies.Responses;
using Domains.Models.Currencies;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Currencies.Queries.GetCurrencyDetails;

public record GetCurrencyByIdQuery(int? Id) : IRequest<ErrorOr<Currency>>;
