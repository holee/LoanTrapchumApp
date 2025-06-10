namespace Applications.Requests.Lists.OtherNames;


public sealed class EntityNamesCreate : BaseEntityNamesCreate
{
    public EntityNamesCreate() { }
    public EntityNamesCreate(string id) : this() => Id = id;
}

public sealed class EntityGuarantorCreate : BaseEntityNamesCreate 
{
    public EntityGuarantorCreate() { }
    public EntityGuarantorCreate(string id) : this() => Id = id; 
}