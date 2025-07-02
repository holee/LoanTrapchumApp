using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Requests.Settings
{
    public sealed class OccupationDtoCreate
    {
        public int OccupationId { get; set; }
        public string? OccupationTitle { get; set; } = string.Empty;
        public string? Khmer { get; set; } = string.Empty;
        public string? TypeOf { get; set; } = string.Empty;
    }

}
