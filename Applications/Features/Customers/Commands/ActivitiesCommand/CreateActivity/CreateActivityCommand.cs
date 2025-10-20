using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.Utils;

namespace Applications.Features.Customers.Commands.ActivitiesCommand.CreateActivity
{
    public sealed class CreateActivityCommand : IRequest<int>
    {
        public string? CustomerRefId { get; set; }
        public DateTime? ActivityDate { get; set; }
        public string? Description { get; set; } = string.Empty;
        
        public bool Status { get; set; } = true;
    }
}
