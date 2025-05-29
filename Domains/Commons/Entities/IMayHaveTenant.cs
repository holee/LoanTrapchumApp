namespace Domains.Commons.Entities;

public interface IMayHaveTenant
{
    string? TenantId { get; set; }
}
