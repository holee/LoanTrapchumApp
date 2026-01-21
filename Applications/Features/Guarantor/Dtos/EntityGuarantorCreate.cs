using Applications.Features.OtherNames;

namespace Applications.Features.Guarantor.Dtos;

public sealed class EntityGuarantorCreate : BaseEntityNamesCreate 
{
    public EntityGuarantorCreate() { }
    public EntityGuarantorCreate(string id) : this() => Id = id; 
}