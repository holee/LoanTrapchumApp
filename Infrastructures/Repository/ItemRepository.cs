using Applications.Features.Items.Dto;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using ErrorOr;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Repository;

public sealed class ItemRepository(
    DataContext contextFactory,
    ILogger<ItemRepository> logger) : IItemService
{
    public async Task<Pager<ItemDto>> GetItemsAsync(QueryParam param, CancellationToken cancellation = default)
    {
        logger.LogInformation("Fetching items");

        var context = contextFactory;

        var query = context.Items.AsQueryable();

        if (!param.IncludeInActive)
        {
            query = query.Where(x => x.IsActive == true);
        }

        if (!string.IsNullOrWhiteSpace(param.Filter))
        {
            query = query.Where(x =>
                x.ItemName.Contains(param.Filter) ||
                x.Description.Contains(param.Filter));
        }

        if (!string.IsNullOrWhiteSpace(param.SortColumn))
        {
            query = param.SortDescending
                ? query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn))
                : query.OrderBy(x => EF.Property<object>(x, param.SortColumn));
        }
        else
        {
            query = query.OrderBy(x => x.ItemName);
        }

        var count = await query.CountAsync(cancellation);

        if (param.Take > 0)
        {
            query = query.Skip(param.Page).Take(param.Take);
        }

        var items = await query
            .AsNoTracking()
            .Select(x => new ItemDto
            {
                Id = x.Id,
                ItemName = x.ItemName,
                Description = x.Description,
                LedgerRefId = x.LedgerRefId,
                IsActive = x.IsActive ?? false
            })
            .ToListAsync(cancellation);

        return new Pager<ItemDto>(items, count);
    }

    public async Task<ErrorOr<ItemDto>> GetItemByIdAsync(int id, CancellationToken cancellation = default)
    {
        logger.LogInformation("Fetching item by id: {ItemId}", id);

        var context = contextFactory;

        var item = await context.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellation);

        if (item is null)
        {
            return Error.NotFound(description: $"Item with id {id} was not found.");
        }

        return new ItemDto
        {
            Id = item.Id,
            ItemName = item.ItemName,
            Description = item.Description,
            LedgerRefId = item.LedgerRefId,
            IsActive = item.IsActive ?? false
        };
    }

    public async Task<ErrorOr<ItemDto>> CreateAsync(ItemDtoCreate request, CancellationToken cancellation = default)
    {
        logger.LogInformation("Creating item: {ItemName}", request.ItemName);

        var context = contextFactory;

        var exists = await context.Items
            .AnyAsync(x => x.ItemName == request.ItemName, cancellation);

        if (exists)
        {
            return Error.Conflict(description: $"Item '{request.ItemName}' already exists.");
        }

        var entity = new Item
        {
            ItemName = request.ItemName,
            Description = request.Description,
            LedgerRefId = request.LedgerRefId,
            IsActive = request.IsActive
        };

        context.Items.Add(entity);
        await context.SaveChangesAsync(cancellation);

        return new ItemDto
        {
            Id = entity.Id,
            ItemName = entity.ItemName,
            Description = entity.Description,
            LedgerRefId = entity.LedgerRefId,
            IsActive = entity.IsActive ?? false
        };
    }

    public async Task<ErrorOr<ItemDto>> UpdateAsync(ItemDtoUpdate request, CancellationToken cancellation = default)
    {
        logger.LogInformation("Updating item: {ItemId}", request.Id);

        var context = contextFactory;

        var entity = await context.Items.FirstOrDefaultAsync(x => x.Id == request.Id, cancellation);

        if (entity is null)
        {
            return Error.NotFound(description: $"Item with id {request.Id} was not found.");
        }

        var duplicate = await context.Items
            .AnyAsync(x => x.ItemName == request.ItemName && x.Id != request.Id, cancellation);

        if (duplicate)
        {
            return Error.Conflict(description: $"Item '{request.ItemName}' already exists.");
        }

        entity.ItemName = request.ItemName;
        entity.Description = request.Description;
        entity.LedgerRefId = request.LedgerRefId;
        entity.IsActive = request.IsActive;

        await context.SaveChangesAsync(cancellation);

        return new ItemDto
        {
            Id = entity.Id,
            ItemName = entity.ItemName,
            Description = entity.Description,
            LedgerRefId = entity.LedgerRefId,
            IsActive = entity.IsActive ?? false
        };
    }

    public async Task<ErrorOr<string>> DeleteAsync(int id, CancellationToken cancellation = default)
    {
        logger.LogInformation("Deleting item: {ItemId}", id);

        var context = contextFactory;

        var entity = await context.Items.FirstOrDefaultAsync(x => x.Id == id, cancellation);

        if (entity is null)
        {
            return Error.NotFound(description: $"Item with id {id} was not found.");
        }

        context.Items.Remove(entity);
        await context.SaveChangesAsync(cancellation);

        return "Item deleted successfully.";
    }
}
