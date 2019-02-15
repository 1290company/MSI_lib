using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSI_lib
{
     static class IEnumerableExtensions
    {
        public static List<T> AsList<T>(this IDataReader dr)
        {

            var cols = dr.GetSchemaTable()
                 .Rows
                 .OfType<DataRow>()
                 .Select(row => row["ColumnName"]);

            String ColName = null;
            List<T> list = new List<T>();
            T obj = default(T);

            while (dr.Read())
            {
                try
                {
                    obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (prop.Name.Equals("RecordCount"))
                            continue;

                        if (cols.Contains(prop.Name) && !object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            ColName = prop.Name;
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    list.Add(obj);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write("ColName =" + ColName + ", Msg = " + ex.Message);
                }
            }
            dr.Close();
            dr.Dispose();
            return list;
        }

        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            List<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        public static List<T> ToList<T>(this DataTable table, Dictionary<string, string> mappings) where T : new()
        {
            List<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties, mappings);
                result.Add(item);
            }

            return result;
        }

        private static T CreateItemFromRow<T>(DataRow row, List<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                property.SetValue(item, row[property.Name], null);
            }
            return item;
        }

        private static T CreateItemFromRow<T>(DataRow row, List<PropertyInfo> properties, Dictionary<string, string> mappings) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (mappings.ContainsKey(property.Name))
                    property.SetValue(item, row[mappings[property.Name]], null);
            }
            return item;
        }
    }
}