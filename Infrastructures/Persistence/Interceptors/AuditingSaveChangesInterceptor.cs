using Applications.Interfaces.Identity;
using Domains.Models.Audits;
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
       
        var context = (DataContext)eventData.Context!;


        foreach (var entry in context.ChangeTracker.Entries<IBaseEntity<Guid>>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.Now;
                entry.Property(x => x.CreatedAt).IsModified = false;
                entry.Property(e => e.UpdatedAt).IsModified = true; // Prevent updating CreatedAt
            }
            if (entry.State == EntityState.Deleted)
            {
                entry.Entity.DeletedAt = DateTime.Now;
                entry.Property(x => x.CreatedAt).IsModified = false;
                entry.Property(e => e.DeletedAt).IsModified = true; // Prevent updating CreatedAt
            }
        }


        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            var audit = BuildAuditEntry(entry);

            context.AuditEntries.Add(audit);
        }

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
}

