using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses.Lists
{
    public sealed class NationalityResponse
    {
        public int NationalityId { get; set; }
        public string? NationalityName { get; set; } = string.Empty;
        public string? NationalityCode { get; set; } = string.Empty;

    }
}
