using Applications.Bases;
using Applications.Utils;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Collaterals.Commands.DeleteCOmmand
{
    public sealed class DeleteCollateralLineCommand : IRequest<ErrorOr<DeleteResult>>
    {
        public long Id { get; set; }
        public string? CollateralId { get; set; } = string.Empty;
    }



}
