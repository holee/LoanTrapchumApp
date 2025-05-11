using System.Reflection;

namespace Infrastructures.DataAccess.SqlBuilders
{
    public sealed class ColumnMapper
    {
        public string ColumnName { get; set; } = "Id";
        public PropertyInfo PropertyInfo { get; set; } = default!;
        public bool IsAutoIncrementing { get; set; } = false; 
    }
}
