using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Customers.Queries.GetCustomerCollateralLine;

public record GetCustomerCollateralLinesQuery(string? CustomerId, bool IncludeInUsed = false)
    : IRequest<Pager<VCollateralLines>>;
