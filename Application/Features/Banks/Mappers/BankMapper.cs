using Application.Features.Banks.Dtos;
using Domains.Models.Currencies;

namespace Application.Features.Banks.Mappers;

/// <summary>
/// Extension methods for mapping between Bank domain model and Bank DTOs.
/// </summary>
public static class BankMapper
{
    /// <summary>
    /// Maps a <see cref="Bank"/> domain model to a <see cref="BankDto"/> response DTO.
    /// </summary>
    public static BankDto ToBankDto(this Bank bank) => new()
    {
        BankId = bank.BankId,
        BankCode = bank.BankCode,
        BankName = bank.BankName,
        AltName = bank.AltName,
        AltPhone = bank.AltPhone,
        Email = bank.Email,
        WebSite = bank.WebSite,
        IsClosed = bank.IsClosed ?? false,
        IsHead = bank.IsHead ?? false,
        Country = bank.Country,
        ProvinceName = bank.ProvinceName,
        DistrictName = bank.DistrictName,
        CommuneName = bank.CommuneName,
        VillageName = bank.VillageName,
        Remark = bank.Remark
    };

    /// <summary>
    /// Maps a <see cref="BankDtoCreate"/> request DTO to a <see cref="Bank"/> domain model.
    /// </summary>
    public static Bank ToBank(this BankDtoCreate dto) => new()
    {
        BankCode = dto.BankCode,
        BankName = dto.BankName,
        AltName = dto.AltName,
        AltPhone = dto.AltPhone,
        Email = dto.Email,
        WebSite = dto.WebSite,
        IsClosed = dto.IsClosed,
        IsHead = dto.IsHead,
        Country = dto.Country,
        ProvinceName = dto.ProvinceName,
        DistrictName = dto.DistrictName,
        CommuneName = dto.CommuneName,
        VillageName = dto.VillageName,
        Remark = dto.Remark
    };

    /// <summary>
    /// Maps a <see cref="BankDtoUpdate"/> request DTO to a <see cref="Bank"/> domain model.
    /// </summary>
    public static Bank ToBank(this BankDtoUpdate dto) => new()
    {
        BankId = dto.BankId,
        BankCode = dto.BankCode,
        BankName = dto.BankName,
        AltName = dto.AltName,
        AltPhone = dto.AltPhone,
        Email = dto.Email,
        WebSite = dto.WebSite,
        IsClosed = dto.IsClosed,
        IsHead = dto.IsHead,
        Country = dto.Country,
        ProvinceName = dto.ProvinceName,
        DistrictName = dto.DistrictName,
        CommuneName = dto.CommuneName,
        VillageName = dto.VillageName,
        Remark = dto.Remark
    };
}
