using Applications.Features.Collaterals.Dtos;
using Applications.Utils;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Collaterals.Commands.CreateCollateralLines;

public record AddOrUpdateCollateralLineCommand(CollateralLineDtoCreate Request) : IRequest<ErrorOr<string>>;
