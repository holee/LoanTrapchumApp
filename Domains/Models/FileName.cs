using System;
using System.Collections.Generic;
using System.Text;

namespace Domains.Models;

public sealed class EmergencyContact 
{
    public string? ContactId { get; set; }
    public string? ContactName { get; set; }
    public string? ContactNumber { get; set; }
    public string? Relationship { get; set; }
    public string? Description { get; set; }
}