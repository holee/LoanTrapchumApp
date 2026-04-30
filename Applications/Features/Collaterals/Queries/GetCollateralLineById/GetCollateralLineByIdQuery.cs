using Domains.Models.Collaterals;
using ErrorOr;

namespace Applications.Features.Collaterals.Queries.GetCollateralLineById;

public record GetCollateralLineByIdQuery(long? Id,string? CollateralId) : IRequest<ErrorOr<CollateralLine>>; 
