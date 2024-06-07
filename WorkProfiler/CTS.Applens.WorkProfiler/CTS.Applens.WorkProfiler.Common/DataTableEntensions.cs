using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Common
{
    /// <summary>
    /// this class holds DataTableEntensions details
    /// </summary>
    public static class DataTableEntensions
    {
        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="table">table</param>
        /// <returns>Method returns tables</returns>
        public static IList<T> ToList<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            IList<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">table</param>
        /// <param name="mappings">mappings</param>
        /// <returns></returns>
        public static IList<T> ToList<T>(this DataTable table,
            Dictionary<string, string> mappings) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            IList<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties, mappings);
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// CreateItemFromRow
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row">row</param>
        /// <param name="properties">properties</param>
        /// <returns></returns>
        private static T CreateItemFromRow<T>(DataRow row,
            IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {

                if (row.Table.Columns.Contains(property.Name)
                    && row[property.Name] != DBNull.Value)
                {
                    property.SetValue(item, Convert.ChangeType(row[property.Name],
                        Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType,CultureInfo.CurrentCulture), null);
                }
            }

            return item;
        }


        /// <summary>
        /// CreateItemFromRow
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row">row</param>
        /// <param name="properties">properties</param>
        /// <param name="mappings">mappings</param>
        /// <returns></returns>
        private static T CreateItemFromRow<T>(DataRow row,
            IList<PropertyInfo> properties,
            Dictionary<string, string> mappings) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (mappings.ContainsKey(property.Name))
                {
                    property.SetValue(item, row[mappings[property.Name]], null);
                }
            }
            return item;
        }
    }
}
