using System.ComponentModel;
using System.Data;
using Microsoft.OpenApi;

namespace GoodsApi.Infrastructure.Extensions;

public static class DataTableExtension
{
    public static DataTable ToDataTable<T>(this IEnumerable<T> data, string name)
    {
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new(name);

        foreach (PropertyDescriptor prop in properties)
        {
            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            
            if (type.IsEnum) type = typeof(string);
            
            table.Columns.Add(prop.DisplayName, type);
        }

        foreach (T item in data)
        {
            DataRow row = table.NewRow();

            foreach (PropertyDescriptor prop in properties)
            {
                var value = prop.GetValue(item);

                if (value is Enum enumValue)
                {
                    row[prop.DisplayName] = enumValue.GetDisplayName();
                }
                else
                {
                    row[prop.DisplayName] = value ?? DBNull.Value;
                }
            }

            table.Rows.Add(row);
        }

        return table;
    }

}