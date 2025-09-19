using Applications.Features.OtherNames;
using Domains.Models.Lists;
using Domains.Models.Lists.Employees;

namespace Applications.Commons.Mappers
{
    public static class EmployeeMapper
    {
        public static EntityEmployeeCreate ToEmployeeEdit(this EntityNames dto)
        {

            return new EntityEmployeeCreate
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
                EmployeeId = dto.Employee?.EmployeeId,
                Photo = dto.Employee?.Photo,
                BranchRefId = dto.Employee?.BranchRefId,
                DepartmentRefId = dto.Employee?.DepartmentRefId,
                HireDate = dto.Employee?.HireDate,
                EndDate = dto.Employee?.EndDate
            };

        }


        public static EntityNames ToEmployeeCreate(this EntityEmployeeCreate dto)
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
                Employee = new Employee
                {
                    Photo = dto.Photo,
                    BranchRefId = dto.BranchRefId,
                    DepartmentRefId = dto.DepartmentRefId,
                    HireDate = dto.HireDate,
                    EndDate = dto.EndDate
                }
            };

        }

        public static EntityNames ToEmployeeEdit(this EntityEmployeeCreate dto)
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
                Employee = new Employee
                {
                    EmployeeId = dto.EmployeeId,
                    Photo = dto.Photo,
                    BranchRefId = dto.BranchRefId,
                    DepartmentRefId = dto.DepartmentRefId,
                    HireDate = dto.HireDate,
                    EndDate = dto.EndDate
                }
            };

        }


    }
}