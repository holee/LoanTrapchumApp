using Domains.Models.Collaterals;
using ErrorOr;

namespace Application.Features.Collaterals.Queries.GetCollateralLineById;

public record GetCollateralLineByIdQuery(long? Id,string? CollateralId) : IRequest<ErrorOr<CollateralLine>>; 
