using Application.Features.Collaterals.Dtos;
using Application.Utils;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Collaterals.Commands.CreateCollateralLines;

public record AddOrUpdateCollateralLineCommand(CollateralLineDtoCreate Request) : IRequest<ErrorOr<string>>;
