namespace Domains.Models.Lists
{
    public class EntityNameType
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; } = default!;
        public string? TypeOf { get; set; } = string.Empty;
    }
}
