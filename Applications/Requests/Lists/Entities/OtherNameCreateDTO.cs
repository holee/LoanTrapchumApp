using System.ComponentModel.DataAnnotations;

namespace Applications.Requests.Lists.Entities
{
    public class OtherNameCreateDto : EntityNameDtoCreate
    {
        [StringLength(200)]
        public string? Note { get; set; }
    }
}
