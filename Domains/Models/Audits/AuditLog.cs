using Domains.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models.Audits;

public sealed class AuditLog
{
    public long Id { get; set; }
    public required string EntityName { get; set; } = string.Empty; // Name of the entity (e.g., "Account", "Transaction")
    public required string PrimaryKey { get; set; } // Primary key of the affected entity
    public required string AuditAction { get; set; } // e.g., "Added", "Updated", "Deleted" 
    //public TransactionType TransactionType { get; set; } // From your enum
    public string? Description { get; set; } // Description from TransactionType
    public string? OldValues { get; set; } // JSON serialized old values (for updates)
    public string? NewValues { get; set; } // JSON serialized new values
    public string? UserId { get; set; } // User who performed the action
    public DateTime? Timestamp { get; set; } // When the change occurred
}
