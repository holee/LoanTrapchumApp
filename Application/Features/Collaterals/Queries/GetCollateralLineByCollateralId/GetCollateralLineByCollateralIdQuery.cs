using Application.Features.Collaterals.Dtos;
using Application.Interfaces.Lists;
using Application.Utils;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Collaterals.Queries.GetCollateralLineByCollateralId;

public sealed record GetCollateralLineByCollateralIdQuery(string CollateralId) : IRequest<ErrorOr<IEnumerable<VCollateralLines>>>;
