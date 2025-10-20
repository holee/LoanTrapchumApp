using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Bases
{
    public abstract class BasePropEntity
    {
        [NotMapped] public int RowIndex { get; set; } = 1;
    }
}