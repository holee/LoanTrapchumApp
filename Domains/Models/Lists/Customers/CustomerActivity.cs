using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Lists.Customers
{
    public class CustomerActivity
    {
        public int Id { get; set; }
        public Guid? CustomerRefId { get; set; }
        public bool? Status { get; set; } = false;
        public string? Description { get; set; }
        public DateTime? CreatedDT { get; set; } = DateTime.Now;
        public DateTime? ModifiedDT { get; set; } = DateTime.Now;
    }
}
