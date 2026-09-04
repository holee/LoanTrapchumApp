using Application.Features.Settings.Dto;
using ErrorOr;

namespace Application.Features.Settings.Queries.AppSettings.GetAppSettings;

public record GetAppSettingsQuery(string? Filter) : IRequest<ErrorOr<IEnumerable<AppSettingDto>>>;
