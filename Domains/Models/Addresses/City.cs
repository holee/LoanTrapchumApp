using System.ComponentModel.DataAnnotations;

namespace Domains.Models.Addresses
{
    public class City
    {
        [Required]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public required string CityName { get; set; } 
    }
}
