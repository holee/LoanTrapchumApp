using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Lists.Customers
{
    public sealed class CustomerActivity
    {
        public int Id { get; set; }
        public string? CustomerRefId { get; set; }
        public DateTime? ActivityDate { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; } = false; 
        public DateTimeOffset? CreatedDT { get; set; } = DateTime.UtcNow;
        public DateTimeOffset? ModifiedDT { get; set; } = DateTime.UtcNow;
    }
}
