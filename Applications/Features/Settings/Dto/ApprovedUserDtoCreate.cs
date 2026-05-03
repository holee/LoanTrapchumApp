namespace Applications.Features.Settings.Dto;

public record ApprovedUserDtoCreate(int? BranchId, int? CurrencyId, decimal? Amount, QueryParam? Param);

