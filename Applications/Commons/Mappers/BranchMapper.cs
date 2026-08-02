namespace Applications.Commons.Mappers;

public static class BranchMapper
{
    public static BranchDtoCreate ToBranchCrate(this Branch? model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }
        return new BranchDtoCreate
        {
            BranchName = model.BranchName,
            BankRefId = model.BankRefId,
            VilInKhmer = model.VilInKhmer,
            VillageRefId = model.VillageRefId,
            Description = model.Description,
            City = model.City,
            CommuneRefId = model.CommuneRefId,
            ComDescription = model.ComDescription,
            BranchCode = model.BranchCode,
            PhoneNumber = model.PhoneNumber,
            Zone = model.Zone,
            ComInKhmer = model.ComInKhmer,
            CountryName = model.CountryName,
            DistrictRefId = model.DistrictRefId,
            DisInKhmer = model.DisInKhmer,
            ProvinceRefId = model.ProvinceRefId,
            ProInKhmer = model.ProInKhmer,
            ImagePath = model.LtDocument is not null ? model.LtDocument.FileUrl : string.Empty,
            ImagePathId = model.ImagePathId,
            IsActive = model.IsActive,
        };
    }



    public static BranchDtoUpdate ToBranchDtoUpdate(this Branch? model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }
        return new BranchDtoUpdate
        {
            BranchId = model.BranchId,
            BranchName = model.BranchName,
            BankRefId = model.BankRefId,
            VilInKhmer = model.VilInKhmer,
            VillageRefId = model.VillageRefId,
            EditSeq = model.EditSeq,
            Description = model.Description,
            City = model.City,
            CommuneRefId = model.CommuneRefId,
            ComDescription = model.ComDescription,
            BranchCode = model.BranchCode,
            PhoneNumber = model.PhoneNumber,
            Zone = model.Zone,
            ComInKhmer = model.ComInKhmer,
            CountryName = model.CountryName,
            DistrictRefId = model.DistrictRefId,
            DisInKhmer = model.DisInKhmer,
            ProvinceRefId = model.ProvinceRefId,
            ProInKhmer = model.ProInKhmer,
            ImagePath = model.LtDocument is not null ? model.LtDocument.FileUrl : string.Empty,
            ImagePathId = model.ImagePathId,
            IsActive = model.IsActive,
        };
    }
}
