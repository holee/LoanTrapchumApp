namespace Applications.Features.OtherNames;


public sealed class EntityNamesCreate : BaseEntityNamesCreate
{
    public EntityNamesCreate() { }
    public EntityNamesCreate(string id) : this() => Id = id;
}
