using Applications.Responses.Audits;
using Domains.Models.Audits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Commons.Mappers
{
    public static class AuditMapper
    {
        public static AuditVm ProjectToAuditVm(this AuditEntry auditEntry)    
        {
            if (auditEntry == null) throw new ArgumentNullException(nameof(auditEntry));
            return new AuditVm
            {
                Id = auditEntry.Id,
                UserId = auditEntry.UserId,
                TableName = auditEntry.TableName,
                ActionType = auditEntry.ActionType,
                KeyValues = auditEntry.KeyValues,
                OldValues = auditEntry.OldValues,
                NewValues = auditEntry.NewValues,
                CreatedAt = auditEntry.CreatedAt
            };
        }
    }
}
