using System.ComponentModel.DataAnnotations;

namespace Applications.Requests.Lists.Entities
{
    public class EntVendorDtoCreate
    {
        public string? CompanyName { get; set; }


        [ValidateComplexType]
        public EntityNameDtoCreate EntityName { get; set; } = new();
    }
}
