using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Infrastructures.DataAccess.SqlBuilders
{
    public class EntityBaseMapper
    {
        public EntityBaseMapper() 
        {
            Init();
        }
        public string? SchemaName { get; set; } = string.Empty;
        public string? TableName { get; set; } = string.Empty;
        public List<ColumnMapper> Columns { get; set; } = [];
        public string? SQL { get; set; } = string.Empty;
        public string? ParameterPrefix { get; set; } = string.Empty;
        protected virtual void SetTableAndSchemaName(Type typ)
        {
            // Is there is a [Table] attribute?
            TableAttribute table = typ.GetCustomAttribute<TableAttribute>()!;

            // Assume table name is the class name
            TableName = !CheckIfAnonymousType(typ) ? typ.Name : nameof(typ);

            if (table != null)
            {
                // Set properties from [Table] attribute
                TableName = table.Name;

                SchemaName = table.Schema ?? SchemaName;
            }
        }

        protected virtual void Init()
        {
            SchemaName = "dbo";
            TableName = string.Empty;
            ParameterPrefix = "@";
            SQL = string.Empty;
            Columns = new();
        }

        private static bool CheckIfAnonymousType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && type.Attributes.HasFlag(TypeAttributes.NotPublic);
        }
    }
}
