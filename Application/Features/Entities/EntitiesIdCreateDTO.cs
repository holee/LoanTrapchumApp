using System.ComponentModel.DataAnnotations;
using Application.Features.Addresses.Dto;

namespace Application.Features.Entities 
{
    public class EntitiesIdCreateDto 
    {
        public Guid EntId { get; set; }
        public string EntType { get; set; } = default!;

        [Required]
        public string FirstName { get; set; } = default!;
        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; } = default!;
        public string? AltName { get; set; }
        public string? Sex { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string? MarritalStatus { get; set; }
        public string? Phone { get; set; }
        public string? EmailAddress { get; set; }
        public Guid? AddressRefId { get; set; }
        public AddressDtoCreate? Address { get; set; } = new AddressDtoCreate();
    }
}
