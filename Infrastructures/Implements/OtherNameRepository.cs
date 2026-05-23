using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.Guarantor.Dtos;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Interfaces.Lists;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Bases;
using Domains.Models.Lists;
using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Lists.Vendors;
using ErrorOr;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public sealed class OtherNameRepository(
    IDbContextFactory<DataContext> dbContextFactory,
    ILogger<OtherNameRepository> logger) : IOtherNameService
{
    public async Task<ErrorOr<VOtherNames>> CreateAsync(EntityNames othername, CancellationToken cancellation = default)
    {
        try
        {

            logger.LogInformation("Creating other name for {FirstName} {LastName}.", othername.FirstName, othername.LastName);

            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            context.EntityNames.Add(othername);

            await context.SaveChangesAsync(cancellation);

            var vOtherNames = await GetOtherNameByIdAsync(othername.Id, cancellation);

            return vOtherNames;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create other name for {FirstName} {LastName}.", othername.FirstName, othername.LastName);

            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<VGuarantor>> CreateGaurantorAsync(EntityNames othername, CancellationToken cancellation = default)
    {
        try
        {

            logger.LogInformation("Creating guarantor for {FirstName} {LastName}.", othername.FirstName, othername.LastName);

            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            context.EntityNames.Add(othername);

            await context.SaveChangesAsync(cancellation);

            var guarantor = await GetGuarantorsAsync(othername.Id, cancellation);

            return guarantor;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create guarantor for {FirstName} {LastName}.", othername.FirstName, othername.LastName);

            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<VOtherNames>> UpdateAsync(EntityNames othername, CancellationToken cancellation = default)
    {
        try
        {

            logger.LogInformation("Updating other name {Id} for {FirstName} {LastName}.", othername.Id, othername.FirstName, othername.LastName);

            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            context.EntityNames.Update(othername);

            await context.SaveChangesAsync(cancellation);

            var vOtherNames = await GetOtherNameByIdAsync(othername.Id, cancellation);

            return vOtherNames;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update other name {Id}.", othername.Id);

            return Error.Conflict(description: ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> DeleteAsync(string? othernameId, CancellationToken cancellation = default)
    {
        try
        {

            logger.LogInformation("Deleting other name with ID {Id}.", othernameId);

            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            var entity = await context.EntityNames
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == othernameId, cancellation);

            if (entity is null)
                return Error.NotFound(description: "No data found.");

            var isUsed = await context.Database
                .SqlQuery<VLoanGuarantors>($"SELECT * FROM VLoanGuarators WHERE GuarantorId={othernameId}")
                .AnyAsync(cancellation);

            if (isUsed)
                return Error.Conflict(description: "Name is already in use and cannot be deleted.");

            context.EntityNames.Remove(entity);

            await context.SaveChangesAsync(cancellation);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete other name {Id}.", othernameId);

            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<VOtherNames>> UpdateStatusAsync(string? othernameId, CancellationToken cancellation = default)
    {
        try
        {
            logger.LogInformation("Toggling status for other name with ID {Id}.", othernameId);

            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            var entity = await context.EntityNames
                .FirstOrDefaultAsync(x => x.Id == othernameId, cancellation);

            if (entity is null)
                return Error.NotFound(description: "Name not found.");

            entity.IsActive = !entity.IsActive;

            await context.SaveChangesAsync(cancellation);

            var vOtherNames = await GetOtherNameByIdAsync(othernameId, cancellation);

            return vOtherNames;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to toggle status for other name {Id}.", othernameId);

            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<VOtherNames>> UpdateStatusAsync(string? othernameId, bool state, CancellationToken cancellation = default)
    {
        try
        {
            logger.LogInformation("Updating status for other name with ID {Id} to {State}.", othernameId, state ? "Active" : "Inactive");

            if (string.IsNullOrEmpty(othernameId))
                return Error.Validation(description: $"Invalid ID: {othernameId}");

            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            var entity = await context.EntityNames.FindAsync([othernameId], cancellation);

            if (entity is null)
                return Error.NotFound(description: Messages.DataNotFound);

            entity.IsActive = state;

            await context.SaveChangesAsync(cancellation);

            var updated = await GetOtherNameByIdAsync(othernameId, cancellation);

            return updated;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update status for other name {Id}.", othernameId);

            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<EntityNames>> GetByIdAsync(string? id, CancellationToken cancellation = default)
    {

        logger.LogInformation("Retrieving other name with ID {Id}.", id);

        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var entity = await context.EntityNames
                                    .Where(x => x.Id == id && x.EntType!.Equals(EntType.OtherName.ToString()))
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(cancellation);

        if (entity is null)
            return Error.NotFound(description: Messages.DataNotFound);

        return entity;
    }

    public async Task<ErrorOr<T>> GetDetailAsync<T>(string? id, CancellationToken cancellation = default) where T : class
    {

        logger.LogInformation("Retrieving details for other name with ID {Id}.", id);

        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var item = await context.Database
                                    .SqlQuery<VOtherNames>($"SELECT * FROM VNames WHERE Id={id}")
                                    .ProjectToType<T>()
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(cancellation);

        return item is null
            ? Error.NotFound(description: Messages.DataNotFound)
            : item;
    }

    public async Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, string type, CancellationToken cancellation = default)
    {

        logger.LogInformation("Retrieving names with filter {Filter}, sort column {SortColumn}, and type {Type}.", q.Filter, q.SortColumn, type);

        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.Database.SqlQuery<VOtherNames>($"SELECT * FROM VNames WHERE EntType={type}");

        if (!string.IsNullOrEmpty(q.Filter))
        {
            queries = queries.Where(e =>
                EF.Functions.Like(e.FirstName, $"%{q.Filter}%") ||
                EF.Functions.Like(e.LastName, $"%{q.Filter}%") ||
                EF.Functions.Like(e.Phone, $"%{q.Filter}%") ||
                EF.Functions.Like(e.AltName, $"%{q.Filter}%"));
        }

        if (q.SortDescending && !string.IsNullOrEmpty(q.SortColumn))
            queries = queries.OrderByDescending(e => EF.Property<object>(e, q.SortColumn));

        var counts = await queries.CountAsync(cancellation);

        var items = await queries.Skip(q.Page).Take(q.Take).ToListAsync(cancellation);

        return new Pager<VOtherNames>(items, counts);
    }

    public async Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, CancellationToken cancellation = default)
    {


        logger.LogInformation("Retrieving names with filter {Filter}, sort column {SortColumn}, and include inactive {IncludeInActive}.", q.Filter, q.SortColumn, q.IncludeInActive);

        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.VOtherNames.AsNoTracking();

        if (!string.IsNullOrEmpty(q.Filter))
        {
            var filter = q.Filter.Trim();

            string[] filterColumns = ["FirstName", "LastName", "EmailAddress", "Gender", "Phone", "AltName"];

            queries = queries.StartWiths(filter, filterColumns);
        }

        if (!q.IncludeInActive)
            queries = queries.Where(x => x.IsActive == true);

        if (q.SortDescending && !string.IsNullOrEmpty(q.SortColumn))
            queries = queries.OrderByDescending(e => EF.Property<object>(e, q.SortColumn));

        var counts = await queries.CountAsync(cancellation);

        if (q.Take > 0)
            queries = queries.Skip(q.Page).Take(q.Take);

        var items = await queries.AsNoTracking().ToListAsync(cancellation);

        return PagerExtensions.Create(items, counts);
    }

    public async Task<IEnumerable<T>> GetNameListsAsync<T>(string? nameId, QueryParam param, CancellationToken cancellation = default) where T : class, new()
    {

        logger.LogInformation("Retrieving name lists with filter {Filter} and name ID {NameId}.", param.Filter, nameId);

        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        IQueryable<NameListVm> queries = context.Database
            .SqlQuery<NameListVm>($"SELECT * FROM VNameList")
            .WhereIf(!string.IsNullOrEmpty(param.Filter), x =>
                EF.Functions.Like(x.FirstName!, $"%{param.Filter}%") ||
                EF.Functions.Like(x.LastName!, $"%{param.Filter}%") ||
                EF.Functions.Like(x.AltName!, $"%{param.Filter}%"))
            .WhereIf(!string.IsNullOrEmpty(nameId), x => x.Id.Contains(nameId!));

        if (param.Take > 0)
        {
            queries = queries.Take(param.Take)
                .OrderByDescending(x => x.FirstName)
                .ThenByDescending(x => x.LastName);
        }

        return await queries.ProjectToType<T>().AsNoTracking().ToListAsync(cancellation);
    }

    public async Task<Pager<T>> GetNameAsync<T>(string? nameId, QueryParam param, CancellationToken cancellation = default) where T : class, new()
    {

        logger.LogInformation("Retrieving names with filter {Filter} and name ID {NameId}.", param.Filter, nameId);

        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.VNameLists
            .WhereIf(!string.IsNullOrEmpty(param.Filter), x =>
                EF.Functions.Like(x.FullName!, $"%{param.Filter}%") ||
                EF.Functions.Like(x.AltName!, $"%{param.Filter}%") ||
                EF.Functions.Like(x.LastName!, $"%{param.Filter}%") ||
                EF.Functions.Like(x.Phone!, $"%{param.Filter}%") ||
                EF.Functions.Like(x.FirstName!, $"%{param.Filter}%"))
            .WhereIf(!string.IsNullOrEmpty(nameId), x => x.Id.Contains(nameId!));

        var counts = await queries.CountAsync(cancellation);

        if (param.Take > 0)
        {
            queries = queries.Take(param.Take)
                .OrderByDescending(x => x.FirstName)
                .ThenByDescending(x => x.LastName);
        }

        var items = await queries.ProjectToType<T>().AsNoTracking().ToListAsync(cancellation);
        return new Pager<T>(items, counts);
    }

    public async Task<ErrorOr<bool>> ConvertToCustomerAsync(string? id, Customer customer, CancellationToken cancellation = default)
    {
        try
        {

            logger.LogInformation("Converting other name with ID {Id} to customer.", id);

            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            var otherName = await context.EntityNames
                .FirstOrDefaultAsync(x => x.Id == id &&
                    x.Converted == false &&
                    x.EntType == EntType.OtherName.ToString(), cancellation);

            if (otherName is null)
                return Error.NotFound(description: Messages.DataNotFound);

            otherName.EntType = EntityTypes.Customer.ToString();

            otherName.Converted = true;

            otherName.Customer = customer;

            await context.SaveChangesAsync(cancellation);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to convert name {Id} to customer.", id);
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> ConvertToVendorAsync(string? id, Vendor vendor, CancellationToken cancellation = default)
    {
        try
        {

            logger.LogInformation("Converting other name with ID {Id} to vendor.", id);

            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            var otherName = await context.EntityNames
                .FirstOrDefaultAsync(x => x.Id == id &&
                    x.Converted == false &&
                    x.EntType == EntType.OtherName.ToString(), cancellation);

            if (otherName is null)
                return Error.NotFound(description: Messages.DataNotFound);

            otherName.EntType = EntityTypes.Vendor.ToString();
            otherName.Converted = true;
            otherName.Vendor = vendor;
            await context.SaveChangesAsync(cancellation);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to convert name {Id} to vendor.", id);
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> ConvertToEmployeeAsync(string? id, Employee employee, CancellationToken cancellation = default)
    {
        try
        {

            logger.LogInformation("Converting other name with ID {Id} to employee.", id);

            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            var otherName = await context.EntityNames
                .FirstOrDefaultAsync(x => x.Id == id &&
                    x.Converted == false &&
                    x.EntType == EntType.OtherName.ToString(), cancellation);

            if (otherName is null)
                return Error.NotFound(description: Messages.DataNotFound);

            otherName.EntType = EntityTypes.Employee.ToString();
            otherName.Converted = true;
            otherName.Employee = employee;
            await context.SaveChangesAsync(cancellation);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to convert name {Id} to employee.", id);
            return Error.Unexpected(description: ex.Message);
        }
    }

    private async Task<VGuarantor> GetGuarantorsAsync(string? id, CancellationToken cancellation = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var guarantor = await context.Database
                                    .SqlQuery<VGuarantor>($"SELECT TOP(1) * FROM VGuarantors WHERE Id={id}")
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(cancellation);
        return guarantor!;
    }

    private async Task<VOtherNames> GetOtherNameByIdAsync(string? id, CancellationToken cancellation = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var guarantor = await context.Database
                                    .SqlQuery<VOtherNames>($"SELECT TOP(1) * FROM VNames WHERE Id={id}")
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(cancellation);
        return guarantor!;
    }

}