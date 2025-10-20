using Applications.Interfaces.Lists;
using Applications.Responses.Lists.Collaterals;
using Applications.Utils;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Collaterals.Commands.Queries.GetCollateralLineByCollateralId;

public sealed record GetCollateralLineByCollateralIdQuery(string CollateralId) : IRequest<ErrorOr<IEnumerable<VCollateralLines>>>;
