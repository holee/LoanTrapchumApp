using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Customers.Queries.GetCustomerCollateralLine;

public record GetCustomerCollateralLinesQuery(string? CustomerId, bool IncludeInUsed = false)
    : IRequest<Pager<VCollateralLines>>;
