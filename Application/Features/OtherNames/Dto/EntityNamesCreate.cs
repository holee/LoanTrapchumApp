namespace Application.Features.OtherNames.Dto;


public sealed class EntityNamesCreate : BaseEntityNamesCreate
{
    public EntityNamesCreate() { }
    public EntityNamesCreate(string id) : this() => Id = id;
}
