using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.AdditionalInformations.Dto
{
    public sealed class NationalityDto
    {
        public int NationalityId { get; set; }
        public string? NationalityName { get; set; } = string.Empty;
        public string? NationalityCode { get; set; } = string.Empty;
    }
}
