using Domains.Models.Lists.Vendors;
using Domains.Models.Lists;
using Applications.Features.OtherNames.Dto;


namespace Applications.Commons.Mappers
{
    public static class VendorMapper
    {
        public static EntityVendorCreate ToVendor(this EntityNames dto) 
        {

            return new EntityVendorCreate
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                AltName = dto.AltName,
                Name = dto.Name,
                EntType = dto.EntType,
                PassportId = dto.PassportId,
                Nationality = dto.Nationality,
                DateOfBirth = dto.DateOfBirth,
                Salutation = dto.Salutation,
                Gender = dto.Gender,
                JobTitle = dto.JobTitle,
                Phone = dto.Phone,
                AltPhone = dto.AltPhone,
                EmailAddress = dto.EmailAddress,
                IdCard = dto.IdCard,
                MarritalStatus = dto.MarritalStatus,
                IsActive = dto.IsActive,
                CountryName = dto.CountryName,
                City = dto.City,
                Occupation = dto.Occupation,
                ProvinceRefId = dto.ProvinceRefId,
                ProDescription = dto.ProDescription,
                ProInKhmer = dto.ProInKhmer,
                DistrictRefId = dto.DistrictRefId,
                DisDescription = dto.DisDescription,
                DisInKhmer = dto.DisInKhmer,
                CommuneRefId = dto.CommuneRefId,
                ComDescription = dto.ComDescription,
                ComInKhmer = dto.ComInKhmer,
                VillageRefId = dto.VillageRefId,
                VillDescription = dto.VillDescription,
                VilInKhmer = dto.VilInKhmer,
                Description = dto.Description,
                CompanyAddress = dto.Vendor?.CompanyAddress,
                CompanyName = dto.Vendor?.CompanyName,
                VendorId = dto.Id
            };

        }

        public static EntityNames ToVendorCreate(this EntityVendorCreate dto)
        {

            return new EntityNames
            {
                Id = $"ent-{Guid.CreateVersion7()}",
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                AltName = dto.AltName,
                Name = dto.Name,
                EntType = dto.EntType,
                PassportId = dto.PassportId,
                Nationality = dto.Nationality,
                DateOfBirth = dto.DateOfBirth,
                Salutation = dto.Salutation,
                Gender = dto.Gender,
                JobTitle = dto.JobTitle,
                Phone = dto.Phone,
                AltPhone = dto.AltPhone,
                EmailAddress = dto.EmailAddress,
                IdCard = dto.IdCard,
                CompanyName = dto.CompanyName,
                MarritalStatus = dto.MarritalStatus,
                IsActive = dto.IsActive,
                CountryName = dto.CountryName,
                City = dto.City,
                Occupation = dto.Occupation,
                ProvinceRefId = dto.ProvinceRefId,
                ProDescription = dto.ProDescription,
                ProInKhmer = dto.ProInKhmer,
                DistrictRefId = dto.DistrictRefId,
                DisDescription = dto.DisDescription,
                DisInKhmer = dto.DisInKhmer,
                CommuneRefId = dto.CommuneRefId,
                ComDescription = dto.ComDescription,
                ComInKhmer = dto.ComInKhmer,
                VillageRefId = dto.VillageRefId,
                VillDescription = dto.VillDescription,
                VilInKhmer = dto.VilInKhmer,
                Description = dto.Description,
                Vendor = new Vendor
                {
                    CompanyAddress = dto.CompanyAddress,
                    CompanyName = dto.CompanyName
                }
            };

        }

        public static EntityNames ToVendorEdit(this EntityVendorCreate dto) 
        {

            return new EntityNames
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                AltName = dto.AltName,
                Name = dto.Name,
                EntType = dto.EntType,
                PassportId = dto.PassportId,
                Nationality = dto.Nationality,
                DateOfBirth = dto.DateOfBirth,
                Salutation = dto.Salutation,
                Gender = dto.Gender,
                JobTitle = dto.JobTitle,
                Phone = dto.Phone,
                AltPhone = dto.AltPhone,
                EmailAddress = dto.EmailAddress,
                IdCard = dto.IdCard,
                CompanyName = dto.CompanyName,
                MarritalStatus = dto.MarritalStatus,
                IsActive = dto.IsActive,
                CountryName = dto.CountryName,
                City = dto.City,
                Occupation = dto.Occupation,
                ProvinceRefId = dto.ProvinceRefId,
                ProDescription = dto.ProDescription,
                ProInKhmer = dto.ProInKhmer,
                DistrictRefId = dto.DistrictRefId,
                DisDescription = dto.DisDescription,
                DisInKhmer = dto.DisInKhmer,
                CommuneRefId = dto.CommuneRefId,
                ComDescription = dto.ComDescription,
                ComInKhmer = dto.ComInKhmer,
                VillageRefId = dto.VillageRefId,
                VillDescription = dto.VillDescription,
                VilInKhmer = dto.VilInKhmer,
                Description = dto.Description,
                Vendor = new Vendor
                {
                    VendorId = dto.VendorId,
                    CompanyName = dto.CompanyName,
                    CompanyAddress = dto.CompanyAddress
                }
            };

        }


    }
}
