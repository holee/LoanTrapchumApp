namespace Applications.Requests.Lists.Entities;

public class EntNameTypeDto
{
    public int TypeId { get; set; }
    public string TypeName { get; set; } = default!;
    public string? Description { get; set; }
}
