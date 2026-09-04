using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Customers.Queries.GetCustomerDetailById;

public sealed record GetCustomerDetailByIdQuery(string? CustomerId, bool Include = false)
        : IRequest<ErrorOr<VCustomerDetails>>;
