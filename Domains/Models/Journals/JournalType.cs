using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Journals;


[Table("st_jounalType")]
public class JournalType
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
}
