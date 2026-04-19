using Applications.Features.OtherNames.Dto;

namespace Applications.Features.Guarantor.Dtos;

public sealed class EntityGuarantorCreate : BaseEntityNamesCreate 
{
    public EntityGuarantorCreate() { }
    public EntityGuarantorCreate(string id) : this() => Id = id; 
}