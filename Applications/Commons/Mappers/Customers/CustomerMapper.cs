using Applications.Features.Customers.Commands.Customers.CreateCustomer;
using Applications.Features.Customers.Commands.Customers.EditCustomer;
using Applications.Features.Customers.Dto;
using Domains.Models.Lists;
using Domains.Models.Lists.Customers;

namespace Applications.Commons.Mappers.Customers;

public static class CustomerMapper
{
    public static EntityCustomerCreate ToCustomer(this EntityNames dto)
    {

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
            SpouseNationality = dto.Customer?.SpouseNationality,
            SpouseOccupation = dto.Customer?.SpouseOccupation,
            SpousePassport = dto.Customer?.SpousePassport,
            SpousePhoneNo = dto.Customer?.SpousePhoneNo,

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
                SpouseNationality = dto.SpouseNationality,
                SpouseOccupation = dto.SpouseOccupation,
                SpousePassport = dto.SpousePassport,
                SpousePhoneNo = dto.SpousePhoneNo,

            }
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
                SpouseNationality = dto.SpouseNationality,
                SpouseOccupation = dto.SpouseOccupation,
                SpousePassport = dto.SpousePassport,
                SpousePhoneNo = dto.SpousePhoneNo,
            }
        };

    }

    public static EntityNames ToCustomerEdit(this EditCustomerCommand source) => source.Request.ToCustomerUpdate();

}


//public static Customer ToCustomerCreate(this EntityNamesCreate dto)
//{
//    return new Customer()
//    {
//        Dependents = dto.Dependents,
//        Expenditure = dto.Expenditure,
//        Income = dto.Income,
//        NoOfChildren = dto.NoOfChildren,
//        Occupation = dto.Occupation,
//        Photo = dto.Photo,
//        SpouseDateOfBirth = dto.SpouseDateOfBirth,
//        SpouseName = dto.SpouseName,
//        SpouseNationality = dto.SpouseNationality,
//        SpouseOccupation = dto.SpouseOccupation,
//        SpousePassport = dto.SpousePassport,
//        SpousePhoneNo = dto.SpousePhoneNo,
//    };
//}
//public static Customer ToCustomerEdit(this EntityNamesCreate dto)
//{
//    return new Customer()
//    {
//        CustomerId = dto.CustomerId,
//        Dependents = dto.Dependents,
//        Expenditure = dto.Expenditure,
//        Income = dto.Income,
//        NoOfChildren = dto.NoOfChildren,
//        Occupation = dto.Occupation,
//        Photo = dto.Photo,
//        SpouseDateOfBirth = dto.SpouseDateOfBirth,
//        SpouseName = dto.SpouseName,
//        SpouseNationality = dto.SpouseNationality,
//        SpouseOccupation = dto.SpouseOccupation,
//        SpousePassport = dto.SpousePassport,
//        SpousePhoneNo = dto.SpousePhoneNo,
//    };
//}

//public static Vendor ToVendorCreate(this EntityNamesCreate dto)
//{
//    return new Vendor
//    {
//        CompanyAddress = dto.CompanyAddress,
//        CompanyName = dto.CompanyName,
//    };
//}


//public static EntityNames ToEntityNamesCreate(this EntityNamesCreate dto)
//{

//    return new EntityNames
//    {
//        Id = $"ent-{Guid.CreateVersion7()}",
//        FirstName = dto.FirstName,
//        MiddleName = dto.MiddleName,
//        LastName = dto.LastName,
//        AltName = dto.AltName,
//        Name = dto.Name,
//        EntType = dto.EntType,
//        PassportId = dto.PassportId,
//        Nationality = dto.Nationality,
//        DateOfBirth = dto.DateOfBirth,
//        Salutation = dto.Salutation,
//        Gender = dto.Gender,
//        JobTitle = dto.JobTitle,
//        Phone = dto.Phone,
//        AltPhone = dto.AltPhone,
//        EmailAddress = dto.EmailAddress,
//        IdCard = dto.IdCard,
//        CompanyName = dto.CompanyName,
//        MarritalStatus = dto.MarritalStatus,
//        IsActive = dto.IsActive,
//        CountryName = dto.CountryName,
//        City = dto.City,
//        Occupation = dto.Occupation,
//        ProvinceRefId = dto.ProvinceRefId,
//        ProDescription = dto.ProDescription,
//        ProInKhmer = dto.ProInKhmer,
//        DistrictRefId = dto.DistrictRefId,
//        DisDescription = dto.DisDescription,
//        DisInKhmer = dto.DisInKhmer,
//        CommuneRefId = dto.CommuneRefId,
//        ComDescription = dto.ComDescription,
//        ComInKhmer = dto.ComInKhmer,
//        VillageRefId = dto.VillageRefId,
//        VillDescription = dto.VillDescription,
//        VilInKhmer = dto.VilInKhmer,
//        Description = dto.Description,
//        Converted = dto.Converted,
//    };
//}

//public static EntityNames ToEntityNamesEdit(this EntityNamesCreate dto)
//{

//    return new EntityNames
//    {
//        Id = dto.Id!,
//        FirstName = dto.FirstName,
//        MiddleName = dto.MiddleName,
//        LastName = dto.LastName,
//        AltName = dto.AltName,
//        Name = dto.Name,
//        EntType = dto.EntType,
//        PassportId = dto.PassportId,
//        Nationality = dto.Nationality,
//        DateOfBirth = dto.DateOfBirth,
//        Salutation = dto.Salutation,
//        Gender = dto.Gender,
//        JobTitle = dto.JobTitle,
//        Phone = dto.Phone,
//        AltPhone = dto.AltPhone,
//        EmailAddress = dto.EmailAddress,
//        IdCard = dto.IdCard,
//        CompanyName = dto.CompanyName,
//        MarritalStatus = dto.MarritalStatus,
//        IsActive = dto.IsActive,
//        CountryName = dto.CountryName,
//        City = dto.City,
//        Occupation = dto.Occupation,
//        ProvinceRefId = dto.ProvinceRefId,
//        ProDescription = dto.ProDescription,
//        ProInKhmer = dto.ProInKhmer,
//        DistrictRefId = dto.DistrictRefId,
//        DisDescription = dto.DisDescription,
//        DisInKhmer = dto.DisInKhmer,
//        CommuneRefId = dto.CommuneRefId,
//        ComDescription = dto.ComDescription,
//        ComInKhmer = dto.ComInKhmer,
//        VillageRefId = dto.VillageRefId,
//        VillDescription = dto.VillDescription,
//        VilInKhmer = dto.VilInKhmer,
//        Description = dto.Description,
//        Converted = dto.Converted
//    };
//}



//public static EntityNames ToGuarantorCreate(this EntityNamesCreate dto)
//{
//    var entity = new EntityNames
//    {
//        Id = $"ent-{Guid.CreateVersion7()}",
//        FirstName = dto.FirstName,
//        MiddleName = dto.MiddleName,
//        LastName = dto.LastName,
//        AltName = dto.AltName,
//        Name = dto.Name,
//        EntType = dto.EntType,
//        PassportId = dto.PassportId,
//        Nationality = dto.Nationality,
//        DateOfBirth = dto.DateOfBirth,
//        Salutation = dto.Salutation,
//        Gender = dto.Gender,
//        JobTitle = dto.JobTitle,
//        Phone = dto.Phone,
//        AltPhone = dto.AltPhone,
//        EmailAddress = dto.EmailAddress,
//        IdCard = dto.IdCard,
//        CompanyName = dto.CompanyName,
//        MarritalStatus = dto.MarritalStatus,
//        IsActive = dto.IsActive,
//        CountryName = dto.CountryName,
//        City = dto.City,
//        Occupation = dto.Occupation,
//        ProvinceRefId = dto.ProvinceRefId,
//        ProDescription = dto.ProDescription,
//        ProInKhmer = dto.ProInKhmer,
//        DistrictRefId = dto.DistrictRefId,
//        DisDescription = dto.DisDescription,
//        DisInKhmer = dto.DisInKhmer,
//        CommuneRefId = dto.CommuneRefId,
//        ComDescription = dto.ComDescription,
//        ComInKhmer = dto.ComInKhmer,
//        VillageRefId = dto.VillageRefId,
//        VillDescription = dto.VillDescription,
//        VilInKhmer = dto.VilInKhmer,
//        Description = dto.Description,
//        Converted = dto.Converted,
//    };

//    return entity;
//}

//public static EntityNames ToGuarantorEdit(this EntityNamesCreate dto)
//{
//    var entity = new EntityNames
//    {
//        Id = dto.Id!,
//        FirstName = dto.FirstName,
//        MiddleName = dto.MiddleName,
//        LastName = dto.LastName,
//        AltName = dto.AltName,
//        Name = dto.Name,
//        EntType = dto.EntType,
//        PassportId = dto.PassportId,
//        Nationality = dto.Nationality,
//        DateOfBirth = dto.DateOfBirth,
//        Salutation = dto.Salutation,
//        Gender = dto.Gender,
//        JobTitle = dto.JobTitle,
//        Phone = dto.Phone,
//        AltPhone = dto.AltPhone,
//        EmailAddress = dto.EmailAddress,
//        IdCard = dto.IdCard,
//        CompanyName = dto.CompanyName,
//        MarritalStatus = dto.MarritalStatus,
//        IsActive = dto.IsActive,
//        CountryName = dto.CountryName,
//        City = dto.City,
//        Occupation = dto.Occupation,
//        ProvinceRefId = dto.ProvinceRefId,
//        ProDescription = dto.ProDescription,
//        ProInKhmer = dto.ProInKhmer,
//        DistrictRefId = dto.DistrictRefId,
//        DisDescription = dto.DisDescription,
//        DisInKhmer = dto.DisInKhmer,
//        CommuneRefId = dto.CommuneRefId,
//        ComDescription = dto.ComDescription,
//        ComInKhmer = dto.ComInKhmer,
//        VillageRefId = dto.VillageRefId,
//        VillDescription = dto.VillDescription,
//        VilInKhmer = dto.VilInKhmer,
//        Description = dto.Description,
//    };
//    return entity;
//}

//public static EntityNamesCreate ToGuarantorEdit(this EntityNames dto)
//{
//    var entity = new EntityNamesCreate
//    {
//        Id = dto.Id,
//        FirstName = dto.FirstName,
//        MiddleName = dto.MiddleName,
//        LastName = dto.LastName,
//        AltName = dto.AltName,
//        Name = dto.Name,
//        EntType = dto.EntType,
//        PassportId = dto.PassportId,
//        Nationality = dto.Nationality,
//        DateOfBirth = dto.DateOfBirth,
//        Salutation = dto.Salutation,
//        Gender = dto.Gender,
//        JobTitle = dto.JobTitle,
//        Phone = dto.Phone,
//        AltPhone = dto.AltPhone,
//        EmailAddress = dto.EmailAddress,
//        IdCard = dto.IdCard,
//        CompanyName = dto.CompanyName,
//        MarritalStatus = dto.MarritalStatus,
//        IsActive = dto.IsActive,
//        CountryName = dto.CountryName,
//        City = dto.City,
//        Occupation = dto.Occupation,
//        ProvinceRefId = dto.ProvinceRefId,
//        ProDescription = dto.ProDescription,
//        ProInKhmer = dto.ProInKhmer,
//        DistrictRefId = dto.DistrictRefId,
//        DisDescription = dto.DisDescription,
//        DisInKhmer = dto.DisInKhmer,
//        CommuneRefId = dto.CommuneRefId,
//        ComDescription = dto.ComDescription,
//        ComInKhmer = dto.ComInKhmer,
//        VillageRefId = dto.VillageRefId,
//        VillDescription = dto.VillDescription,
//        VilInKhmer = dto.VilInKhmer,
//        Description = dto.Description,
//    };
//    return entity;
//}



//// Application/Mappers/EntityNamesMapper.cs
//using Domain.Entities; // Assuming EntityNames is in the Domain layer
//using Application.Dtos; // Assuming EntityNamesCreate is in the Application layer

//namespace Application.Mappers
//{
//    public static class EntityNamesMapper
//    {
//        /// <summary>
//        /// Maps EntityNames to EntityNamesCreate DTO.
//        /// </summary>
//        public static EntityNamesCreate ToDto(this EntityNames entity)
//        {
//            if (entity == null)
//            {
//                return null;
//            }

//            return new EntityNamesCreate
//            {
//                Id = entity.Id,
//                FirstName = entity.FirstName,
//                MiddleName = entity.MiddleName,
//                LastName = entity.LastName,
//                AltName = entity.AltName,
//                Name = entity.Name,
//                EntType = entity.EntType,
//                PassportId = entity.PassportId,
//                Nationality = entity.Nationality,
//                DateOfBirth = entity.DateOfBirth,
//                Salutation = entity.Salutation,
//                Gender = entity.Gender,
//                JobTitle = entity.JobTitle,
//                Phone = entity.Phone,
//                AltPhone = entity.AltPhone,
//                EmailAddress = entity.EmailAddress,
//                IdCard = entity.IdCard,
//                CompanyName = entity.CompanyName,
//                MarritalStatus = entity.MarritalStatus,
//                IsActive = entity.IsActive,
//                CountryName = entity.CountryName,
//                City = entity.City,
//                Occupation = entity.Occupation,
//                ProvinceRefId = entity.ProvinceRefId,
//                ProDescription = entity.ProDescription,
//                ProInKhmer = entity.ProInKhmer,
//                DistrictRefId = entity.DistrictRefId,
//                DisDescription = entity.DisDescription,
//                DisInKhmer = entity.DisInKhmer,
//                CommuneRefId = entity.CommuneRefId,
//                ComDescription = entity.ComDescription,
//                ComInKhmer = entity.ComInKhmer,
//                VillageRefId = entity.VillageRefId,
//                VillDescription = entity.VillDescription,
//                VilInKhmer = entity.VilInKhmer,
//                Description = entity.Description,
//                Converted = entity.Converted,
//                // Customer-specific fields
//                CustomerId = entity.Customer?.Id,
//                Dependents = entity.Customer?.Dependents,
//                NoOfChildren = entity.Customer?.NoOfChildren,
//                Income = entity.Customer?.Income,
//                Expenditure = entity.Customer?.Expenditure,
//                SpouseName = entity.Customer?.SpouseName,
//                SpouseDateOfBirth = entity.Customer?.SpouseDateOfBirth,
//                SpouseNationality = entity.Customer?.SpouseNationality,
//                SpousePassport = entity.Customer?.SpousePassport,
//                SpousePhoneNo = entity.Customer?.SpousePhoneNo,
//                SpouseOccupation = entity.Customer?.SpouseOccupation,
//                // Employee-specific fields
//                EmployeeId = entity.Employee?.Id,
//                StaffId = entity.Employee?.StaffId,
//                UserRefId = entity.Employee?.UserRefId,
//                HireDate = entity.Employee?.HireDate,
//                EndDate = entity.Employee?.EndDate,
//                DepartmentRefId = entity.Employee?.DepartmentRefId,
//                BranchRefId = entity.Employee?.BranchRefId,
//                // Vendor-specific field
//                VendorId = entity.Vendor?.Id,
//                // Shared field
//                Photo = entity.Customer?.Photo ?? entity.Employee?.Photo
//            };
//        }

//        /// <summary>
//        /// Maps EntityNamesCreate DTO to EntityNames entity.
//        /// </summary>
//        public static EntityNames ToEntity(this EntityNamesCreate dto)
//        {
//            if (dto == null)
//            {
//                return null;
//            }

//            var entity = new EntityNames
//            {
//                Id = dto.Id ?? Guid.NewGuid().ToString(), // Generate new ID if null
//                FirstName = dto.FirstName,
//                MiddleName = dto.MiddleName,
//                LastName = dto.LastName,
//                AltName = dto.AltName,
//                Name = dto.Name,
//                EntType = dto.EntType,
//                PassportId = dto.PassportId,
//                Nationality = dto.Nationality,
//                DateOfBirth = dto.DateOfBirth,
//                Salutation = dto.Salutation,
//                Gender = dto.Gender,
//                JobTitle = dto.JobTitle,
//                Phone = dto.Phone,
//                AltPhone = dto.AltPhone,
//                EmailAddress = dto.EmailAddress,
//                IdCard = dto.IdCard,
//                CompanyName = dto.CompanyName,
//                MarritalStatus = dto.MarritalStatus,
//                IsActive = dto.IsActive,
//                CountryName = dto.CountryName,
//                City = dto.City,
//                Occupation = dto.Occupation,
//                ProvinceRefId = dto.ProvinceRefId,
//                ProDescription = dto.ProDescription,
//                ProInKhmer = dto.ProInKhmer,
//                DistrictRefId = dto.DistrictRefId,
//                DisDescription = dto.DisDescription,
//                DisInKhmer = dto.DisInKhmer,
//                CommuneRefId = dto.CommuneRefId,
//                ComDescription = dto.ComDescription,
//                ComInKhmer = dto.ComInKhmer,
//                VillageRefId = dto.VillageRefId,
//                VillDescription = dto.VillDescription,
//                VilInKhmer = dto.VilInKhmer,
//                Description = dto.Description,
//                Converted = dto.Converted,
//                // Initialize navigation properties
//                Customer = dto.CustomerId != null ? new Customer { Id = dto.CustomerId } : null,
//                Employee = dto.EmployeeId != null ? new Employee { Id = dto.EmployeeId } : null,
//                Vendor = dto.VendorId != null ? new Vendor { Id = dto.VendorId } : null,
//                GuarantorsLink = []
//            };

//            // Populate Customer-specific fields if applicable
//            if (dto.CustomerId != null)
//            {
//                entity.Customer = new Customer
//                {
//                    Id = dto.CustomerId,
//                    Dependents = dto.Dependents,
//                    NoOfChildren = dto.NoOfChildren,
//                    Income = dto.Income,
//                    Expenditure = dto.Expenditure,
//                    SpouseName = dto.SpouseName,
//                    SpouseDateOfBirth = dto.SpouseDateOfBirth,
//                    SpouseNationality = dto.SpouseNationality,
//                    SpousePassport = dto.SpousePassport,
//                    SpousePhoneNo = dto.SpousePhoneNo,
//                    SpouseOccupation = dto.SpouseOccupation,
//                    Photo = dto.Photo
//                };
//            }

//            // Populate Employee-specific fields if applicable
//            if (dto.EmployeeId != null)
//            {
//                entity.Employee = new Employee
//                {
//                    Id = dto.EmployeeId,
//                    StaffId = dto.StaffId,
//                    UserRefId = dto.UserRefId,
//                    HireDate = dto.HireDate,
//                    EndDate = dto.EndDate,
//                    DepartmentRefId = dto.DepartmentRefId,
//                    BranchRefId = dto.BranchRefId,
//                    Photo = dto.Photo
//                };
//            }

//            // Populate Vendor-specific field if applicable
//            if (dto.VendorId != null)
//            {
//                entity.Vendor = new Vendor
//                {
//                    Id = dto.VendorId
//                };
//            }

//            return entity;
//        }
//    }
//}