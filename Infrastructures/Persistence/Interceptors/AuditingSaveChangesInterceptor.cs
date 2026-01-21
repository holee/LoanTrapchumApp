using Applications.Interfaces.Identity;
using Domains.Models.Audits;
using Domains.Models.Bases;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace Infrastructures.Persistence.Interceptors;

public sealed class AuditingSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUser;
    public AuditingSaveChangesInterceptor(ICurrentUserService currentUser) =>
        _currentUser = currentUser;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {

        var now = DateTimeOffset.UtcNow;

        var context = (DataContext)eventData.Context!;

        foreach (var entry in context.ChangeTracker.Entries().ToArray())
        {
            if (entry.Entity is AuditEntry) continue;                     // skip self-audit

            ApplyTimestamps(entry, now);

            if (entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            {
                context.AuditEntries.Add(BuildAuditEntry(entry, now));
            }
        }

        //var entries = context.ChangeTracker.Entries()
        //    .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
        //    .ToList();

        //foreach (var entry in entries)
        //{
        //    var audit = BuildAuditEntry(entry);
        
        //    context.AuditEntries.Add(audit);
        //}

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private AuditEntry BuildAuditEntry(EntityEntry entry)
    {
        var pk = entry.Properties
                     .Where(p => p.Metadata.IsPrimaryKey())
                     .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);

        var audit = new AuditEntry
        {
            Id = Guid.NewGuid(),
            UserId = _currentUser.UserId ?? "Anonymous",
            TableName = entry.Metadata.GetTableName()!,
            ActionType = entry.State.ToString(), // Added / Modified / Deleted
            KeyValues = JsonSerializer.Serialize(pk),
            OldValues = entry.State == EntityState.Added
                         ? null
                         : JsonSerializer.Serialize(entry.Properties
                               .Where(p => p.IsModified || entry.State == EntityState.Deleted)
                               .ToDictionary(p => p.Metadata.Name, p => p.OriginalValue)),
            NewValues = entry.State == EntityState.Deleted
                         ? null
                         : JsonSerializer.Serialize(entry.Properties
                               .Where(p => p.IsModified || entry.State == EntityState.Added)
                               .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue))
        };

        return audit;
    }


    private AuditEntry BuildAuditEntry(EntityEntry entry, DateTimeOffset now)
    {
        var pk = entry.Properties
                     .Where(p => p.Metadata.IsPrimaryKey())
                     .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);

        var oldVals = entry.State == EntityState.Added
                      ? null
                      : JsonSerializer.Serialize(entry.Properties
                             .Where(p => p.IsModified || entry.State == EntityState.Deleted)
                             .ToDictionary(p => p.Metadata.Name, p => p.OriginalValue));

        var newVals = entry.State == EntityState.Deleted
                      ? null
                      : JsonSerializer.Serialize(entry.Properties
                             .Where(p => p.IsModified || entry.State == EntityState.Added)
                             .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue));

        return new AuditEntry
        {
            Id = Guid.NewGuid(),
            CreatedAt = now,
            UserId = _currentUser.UserId ?? "Anonymous",
            TableName = entry.Metadata.GetTableName()!,
            ActionType = entry.State.ToString(),  // Added / Modified / Deleted
            KeyValues = JsonSerializer.Serialize(pk),
            OldValues = oldVals,
            NewValues = newVals
        };
    }

    private static void ApplyTimestamps(EntityEntry entry, DateTimeOffset now)
    {
        switch (entry.Entity)
        {
            case IBaseEntity<Guid> e when entry.State == EntityState.Added:
                e.CreatedAt = now;
                break;

            case BaseAddress addr:
                if (entry.State == EntityState.Added)
                {
                    addr.CreatedDt = now;
                    addr.ModifiedDt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    addr.ModifiedDt = now;
                    addr.EditSeq++;
                    entry.Property(nameof(BaseAddress.CreatedDt)).IsModified = false;
                }
                break;

            case BaseEntity be:
                if (entry.State == EntityState.Added)
                {
                    be.CreatedDt = now;
                    be.ModifiedDt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    be.ModifiedDt = now;
                    be.EditSeq++;
                    entry.Property(nameof(BaseEntity.CreatedDt)).IsModified = false;
                }
                break;
        }
    }
}

//private void ProcessSaveChangedAsync()
//{
//    foreach (var item in ChangeTracker.Entries()
//                        .Where(e => e.Entity is BaseAddress && e.State == EntityState.Added))
//    {
//        BaseAddress baseEntry = (BaseAddress)item.Entity;
//        baseEntry.CreatedDt = DateTime.Now;
//        baseEntry.ModifiedDt = DateTime.Now;
//    }

//    foreach (var item in ChangeTracker.Entries()
//                       .Where(e => e.Entity is BaseAddress && e.State == EntityState.Modified))
//    {
//        var baseEntries = (BaseAddress)item.Entity;
//        baseEntries.ModifiedDt = DateTime.Now;
//        baseEntries.EditSeq += 1;
//        item.Property(nameof(BaseAddress.CreatedDt)).IsModified = false;

//    }
//    //for entity inherti from base entity

//    foreach (var item in ChangeTracker.Entries()
//                        .Where(e => e.Entity is BaseEntity && e.State == EntityState.Added))
//    {
//        BaseEntity baseEntry = (BaseEntity)item.Entity;
//        baseEntry.CreatedDt = DateTime.Now;
//        baseEntry.ModifiedDt = DateTime.Now;
//    }

//    foreach (var item in ChangeTracker.Entries()
//                       .Where(e => e.Entity is BaseEntity && e.State == EntityState.Modified))
//    {
//        var baseEntries = (BaseEntity)item.Entity;
//        baseEntries.ModifiedDt = DateTime.Now;
//        baseEntries.EditSeq += 1;
//        item.Property(nameof(BaseEntity.CreatedDt)).IsModified = false;

//    }

//}



