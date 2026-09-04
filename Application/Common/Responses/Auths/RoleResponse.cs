namespace Application.Responses.Auths;

public sealed class RoleResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public RoleResponse(string id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    public RoleResponse()
    {
        Id = string.Empty;
        Name = "";
        Description = string.Empty;
    }
}
