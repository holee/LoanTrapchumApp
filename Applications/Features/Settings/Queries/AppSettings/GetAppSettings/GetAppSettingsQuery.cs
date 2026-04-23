using Applications.Features.Settings.Dto;
using ErrorOr;

namespace Applications.Features.Settings.Queries.AppSettings.GetAppSettings;

public record GetAppSettingsQuery(string? Filter) : IRequest<ErrorOr<IEnumerable<AppSettingDto>>>;
