using Domains.Models;
using Domains.Models.Lists.Customers;

namespace Application.Commons.Mappers.Customers;

public static class CustomerMapper
{
    public static EntityCustomerCreate ToCustomer(this EntityNames? dto)
    {
        if(dto == null) throw new ArgumentNullException(nameof(dto));

        return new EntityCustomerCreate
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
            CustomerId = dto.Id,
            Photo = dto.Customer?.Photo,
            Dependents = dto.Customer?.Dependents,
            Expenditure = dto.Customer?.Expenditure,
            Income = dto.Customer?.Income,
            NoOfChildren = dto.Customer?.NoOfChildren,
            SpouseDateOfBirth = dto.Customer?.SpouseDateOfBirth,
            SpouseName = dto.Customer?.SpouseName,
            SpouseGender = dto.Customer?.SpouseGender ?? string.Empty,
            SpouseNationality = dto.Customer?.SpouseNationality,
            SpouseOccupation = dto.Customer?.SpouseOccupation,
            SpousePassport = dto.Customer?.SpousePassport,
            SpousePhoneNo = dto.Customer?.SpousePhoneNo,
            SpouseIdCard = dto.Customer?.SpouseIdCard,
            CreatedBy = dto.CreatedBy,
            BranchId = dto.BranchId,
            EmergencyContact = new EmergencyContactDtoCreate
            {
                ContactName = dto.Customer?.EmergencyContact?.ContactName,
                ContactNumber = dto.Customer?.EmergencyContact?.ContactNumber,
                Description = dto.Customer?.EmergencyContact?.Description,
                Relationship = dto.Customer?.EmergencyContact?.Relationship,
            }

        };

    }


    public static EntityNames ToCustomerCreate(this EntityCustomerCreate dto)
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
            Converted = false,
            CreatedBy = dto.CreatedBy,
            BranchId = dto.BranchId,
            Customer = new Customer
            {
                CustomerKey = dto.CustomerKey,
                Photo = dto.Photo,
                Dependents = dto.Dependents,
                Expenditure = dto.Expenditure,
                Income = dto.Income,
                NoOfChildren = dto.NoOfChildren,
                Occupation = dto.Occupation,
                SpouseDateOfBirth = dto.SpouseDateOfBirth,
                SpouseName = dto.SpouseName,
                SpouseGender = dto.SpouseGender,
                SpouseNationality = dto.SpouseNationality,
                SpouseOccupation = dto.SpouseOccupation,
                SpousePassport = dto.SpousePassport,
                SpousePhoneNo = dto.SpousePhoneNo,
                SpouseIdCard = dto.SpouseIdCard,
                EmergencyContact = dto.EmergencyContact != null ? new EmergencyContact
                {
                    ContactName = dto.EmergencyContact?.ContactName,
                    ContactNumber = dto.EmergencyContact?.ContactNumber,
                    Description = dto.EmergencyContact?.Description,
                    Relationship = dto.EmergencyContact?.Relationship,
                } : null
            },

        };

    }

    public static EntityNames ToCustomerCreate(this CreateCustomerCommand dto) => dto.Request.ToCustomerCreate();

    public static EntityNames ToCustomerUpdate(this EntityCustomerCreate dto)
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
            CreatedBy = dto.CreatedBy,
            BranchId = dto.BranchId,
            Customer = new Customer
            {
                CustomerId = dto.Id,
                CustomerKey = dto.CustomerKey,
                Photo = dto.Photo,
                Dependents = dto.Dependents,
                Expenditure = dto.Expenditure,
                Income = dto.Income,
                NoOfChildren = dto.NoOfChildren,
                Occupation = dto.Occupation,
                SpouseDateOfBirth = dto.SpouseDateOfBirth,
                SpouseName = dto.SpouseName,
                SpouseGender = dto.SpouseGender,
                SpouseNationality = dto.SpouseNationality,
                SpouseOccupation = dto.SpouseOccupation,
                SpousePassport = dto.SpousePassport,
                SpousePhoneNo = dto.SpousePhoneNo,
                SpouseIdCard = dto.SpouseIdCard,
                EmergencyContact = dto.EmergencyContact != null ? new EmergencyContact
                {
                    ContactId = dto.Id,
                    ContactName = dto.EmergencyContact?.ContactName ?? string.Empty,
                    ContactNumber = dto.EmergencyContact?.ContactNumber ?? string.Empty,
                    Description = dto.EmergencyContact?.Description ?? string.Empty,
                    Relationship = dto.EmergencyContact?.Relationship ?? string.Empty,
                } : null
            }
        };

    }

    public static EntityNames ToCustomerEdit(this EditCustomerCommand source) => source.Request.ToCustomerUpdate();

}
