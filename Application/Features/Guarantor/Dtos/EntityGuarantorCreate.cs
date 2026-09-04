using Application.Features.OtherNames.Dto;

namespace Application.Features.Guarantor.Dtos;

public sealed class EntityGuarantorCreate : BaseEntityNamesCreate 
{
    public EntityGuarantorCreate() { }
    public EntityGuarantorCreate(string id) : this() => Id = id; 
}