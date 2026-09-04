using Domains.Models.Settings;
namespace Application.Features.Settings.Queries.GetSystemSettings;

public record GetSystemSettingQuery(int? Id, QueryParam Param) : IRequest<Pager<Domains.Models.Settings.AppSettings>>;
