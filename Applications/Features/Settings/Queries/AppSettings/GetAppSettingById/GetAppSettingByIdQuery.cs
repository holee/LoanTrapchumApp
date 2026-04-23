using Applications.Features.Settings.Dto;
using ErrorOr;

namespace Applications.Features.Settings.Queries.AppSettings.GetAppSettingById;

public record GetAppSettingByIdQuery(int Id) : IRequest<ErrorOr<AppSettingDto>>;
