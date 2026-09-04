using Application.Features.Collaterals.Dtos;
using Application.Utils;
using Domains.Models.Collaterals;
using ErrorOr;

namespace Application.Features.Collaterals.Queries.GetCollateralLineById;

public record GetCollateralLineByIdEditQuery(long Id,string CollateralId) : IRequest<ErrorOr<CollateralLineDtoCreate>>; 
