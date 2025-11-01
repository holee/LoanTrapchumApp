using Applications.Features.Collaterals.Dtos;
using Applications.Utils;
using Domains.Models.Collaterals;
using ErrorOr;

namespace Applications.Features.Collaterals.Queries.GetCollateralLineById;

public record GetCollateralLineByIdEditQuery(long Id,string CollateralId) : IRequest<ErrorOr<CollateralLineDtoCreate>>; 
