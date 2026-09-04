using Application.Features.Settings.Dto;
using ErrorOr;

namespace Application.Features.Settings.Queries.AppSettings.GetAppSettingById;

public record GetAppSettingByIdQuery(int Id) : IRequest<ErrorOr<AppSettingDto>>;
