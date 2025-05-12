using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace Infrastructures.DataAccess.SqlBuilders
{
    public sealed class EntityMapper : EntityBaseMapper
    {
        public static EntityMapper BuildMapper()
        {
            return new EntityMapper();
        }
        private List<ColumnMapper> BuildColumnCollection<TEntity>()
        {
            List<ColumnMapper> ret = new();

            ColumnMapper colMap;

            // Get all the properties in <TEntity>
            PropertyInfo[] props = typeof(TEntity).GetProperties();

            // Check for [DatabaseGenerated] attribute Is the column an auto-incrementing column?

            // Loop through all properties
            foreach (PropertyInfo prop in props)
            {
                // Is there a [NotMapped] attribute?
                NotMappedAttribute? nm = prop.GetCustomAttribute<NotMappedAttribute>();
                // Only add properties that map to a column
                if (nm == null)
                {
                    // Create a column mapping object
                    colMap = new()
                    {
                        // Set column properties
                        PropertyInfo = prop,

                        ColumnName = prop.Name
                    };

                    // Is column name in [Column] attribute?
                    ColumnAttribute? ca = prop.GetCustomAttribute<ColumnAttribute>();

                    if (ca != null && !string.IsNullOrEmpty(ca.Name))
                    {
                        // Set column name from [Column] attr
                        colMap.ColumnName = ca.Name;
                    }

                    DatabaseGeneratedAttribute? dg = prop.GetCustomAttribute<DatabaseGeneratedAttribute>();

                    if (dg != null)
                    {
                        colMap.IsAutoIncrementing = dg.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity || dg.DatabaseGeneratedOption == DatabaseGeneratedOption.Computed;
                    }

                    // Create collection of columns
                    ret.Add(colMap);
                }
            }

            return ret;
        }

        internal string ToSql<TEntity>(string? table=null)
        {

            if (!string.IsNullOrEmpty(table))
            {
                TableName=table;
            }

            Type typ = typeof(TEntity);

            StringBuilder sb = new(2048);

            string comma = string.Empty;

            // Build Column Mapping Collection
            Columns = BuildColumnCollection<TEntity>();

            // Set Table and Schema properties
            SetTableAndSchemaName(typ);

            // Build the SELECT statement
            sb.Append("SELECT");

            foreach (ColumnMapper item in Columns)
            {
                // Add column
                sb.Append($"{comma} [{item.ColumnName}]");

                comma = ",";
            }
            // Add 'FROM schema.table'
            sb.Append($" FROM {SchemaName}.{TableName}");

            return sb.ToString();
        }



        internal string BuildInsertStatement<TEntity>(string? table=null)
        {

            if(!string.IsNullOrEmpty(table))
            {
                TableName = table;
            }

            StringBuilder sbCol = new(1024);

            StringBuilder sbParam = new(1024);

            string comma = string.Empty;

            Type typ = typeof(TEntity);

            // Build Column Mapping Collection
            Columns = BuildColumnCollection<TEntity>();

            // Set Table and Schema properties
            SetTableAndSchemaName(typ);

            // Build the INSERT statement
            sbCol.Append($"INSERT INTO {SchemaName}.{TableName} (");

            foreach (ColumnMapper item in Columns.Where(c => !c.IsAutoIncrementing))
            {
                // Add column
                sbCol.Append($"{comma}[{item.ColumnName}]");
                // Add Parameter
                sbParam.Append($"{comma} {ParameterPrefix}{item.ColumnName}");
                comma = ", ";
            }
            sbCol.Append(") VALUES (");

            sbCol.Append(sbParam);

            sbCol.Append(')');

            return sbCol.ToString();
        }

    }

}
