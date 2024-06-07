using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using CTS.Applens.WorkProfiler.Models;
using System.Globalization;

namespace CTS.Applens.WorkProfiler.Common.Common
{
    /// <summary>
    /// This class holds ListExtensions details
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// This method is used to return datatable 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<TSource>(this IList<TSource> data)
        {
            DataTable dataTable = new DataTable(typeof(TSource).Name);
            dataTable.Locale = CultureInfo.InvariantCulture;
            PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (TSource item in data)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        /// <summary>
        /// This method is used to return targetList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToListof<T>(this DataTable dt)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => 
                columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }
        /// <summary>
        /// This Method Is Used To ConvertTicketDetails
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public static List<T> ConvertTicketDetails<T>(this DataTable dt, Dictionary<string, string> keyValuePairs)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => 
                keyValuePairs.ContainsKey(properties.Name) && columnNames.Contains(keyValuePairs[properties.Name]) &&
                dataRow[keyValuePairs[properties.Name]] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, Convert.ToString(dataRow[keyValuePairs[properties.Name]], CultureInfo.CurrentCulture), null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }

        public static List<T> ConvertTicketDetailsWorkPattern<T>(this DataTable dt, Dictionary<string, string> keyValuePairs, WorkPatternColumns workpaterncol)
        {
            bool isServiceClassify = false;
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties =>
                keyValuePairs.ContainsKey(properties.Name) && columnNames.Contains(keyValuePairs[properties.Name]) &&
                dataRow[keyValuePairs[properties.Name]] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, Convert.ToString(dataRow[keyValuePairs[properties.Name]], CultureInfo.CurrentCulture), null);
                }

                if (columnNames.Contains("Desc_Base_WorkPattern", StringComparer.OrdinalIgnoreCase) 
                    && columnNames.Contains("Desc_Sub_WorkPattern", StringComparer.OrdinalIgnoreCase) 
                    && columnNames.Contains("Res_Base_WorkPattern", StringComparer.OrdinalIgnoreCase) 
                    && columnNames.Contains("Res_Sub_WorkPattern", StringComparer.OrdinalIgnoreCase))
                {
                    isServiceClassify = true;
                    foreach (var properties in objectProperties.Where(properties =>
                      properties.Name == "TicketDescriptionBasePattern" 
                      //&& columnNames.Contains(workpaterncol.TicketDescriptionBasePattern) 
                      && dataRow["Desc_Base_WorkPattern"] != DBNull.Value))
                    {
                        properties.SetValue(instanceOfT, Convert.ToString(dataRow["Desc_Base_WorkPattern"], CultureInfo.CurrentCulture), null);
                    }
                    foreach (var properties in objectProperties.Where(properties =>
                      properties.Name == "TicketDescriptionSubPattern" 
                      //&& columnNames.Contains(workpaterncol.TicketDescriptionSubPattern) 
                      && dataRow["Desc_Sub_WorkPattern"] != DBNull.Value))
                    {
                        properties.SetValue(instanceOfT, Convert.ToString(dataRow["Desc_Sub_WorkPattern"], CultureInfo.CurrentCulture), null);
                    }
                    foreach (var properties in objectProperties.Where(properties =>
                      properties.Name == "ResolutionRemarksBasePattern" 
                      //&& columnNames.Contains(workpaterncol.ResolutionRemarksBasePattern) 
                      && dataRow["Res_Base_WorkPattern"] != DBNull.Value))
                    {
                        properties.SetValue(instanceOfT, Convert.ToString(dataRow["Res_Base_WorkPattern"], CultureInfo.CurrentCulture), null);
                    }
                    foreach (var properties in objectProperties.Where(properties =>
                      properties.Name == "ResolutionRemarksSubPattern" 
                      //&& columnNames.Contains(workpaterncol.ResolutionRemarksSubPattern) 
                      && dataRow["Res_Sub_WorkPattern"] != DBNull.Value))
                    {
                        properties.SetValue(instanceOfT, Convert.ToString(dataRow["Res_Sub_WorkPattern"], CultureInfo.CurrentCulture), null);
                    }
                }
                else
                {
                    isServiceClassify = true;
                    foreach (var properties in objectProperties.Where(properties =>
                      properties.Name == "TicketDescriptionBasePattern" 
                      && columnNames.Contains(workpaterncol.TicketDescriptionBasePattern) &&
                    dataRow[workpaterncol.TicketDescriptionBasePattern] != DBNull.Value))
                    {
                        properties.SetValue(instanceOfT, 
                            Convert.ToString(dataRow[workpaterncol.TicketDescriptionBasePattern], CultureInfo.CurrentCulture), null);
                    }
                    foreach (var properties in objectProperties.Where(properties =>
                      properties.Name == "TicketDescriptionSubPattern" 
                      && columnNames.Contains(workpaterncol.TicketDescriptionSubPattern) &&
                    dataRow[workpaterncol.TicketDescriptionSubPattern] != DBNull.Value))
                    {
                        properties.SetValue(instanceOfT, 
                            Convert.ToString(dataRow[workpaterncol.TicketDescriptionSubPattern], CultureInfo.CurrentCulture), null);
                    }
                    foreach (var properties in objectProperties.Where(properties =>
                      properties.Name == "ResolutionRemarksBasePattern" 
                      && columnNames.Contains(workpaterncol.ResolutionRemarksBasePattern) &&
                    dataRow[workpaterncol.ResolutionRemarksBasePattern] != DBNull.Value))
                    {
                        properties.SetValue(instanceOfT, 
                            Convert.ToString(dataRow[workpaterncol.ResolutionRemarksBasePattern], CultureInfo.CurrentCulture), null);
                    }
                    foreach (var properties in objectProperties.Where(properties =>
                      properties.Name == "ResolutionRemarksSubPattern" 
                      && columnNames.Contains(workpaterncol.ResolutionRemarksSubPattern) &&
                    dataRow[workpaterncol.ResolutionRemarksSubPattern] != DBNull.Value))
                    {
                        properties.SetValue(instanceOfT, 
                            Convert.ToString(dataRow[workpaterncol.ResolutionRemarksSubPattern], CultureInfo.CurrentCulture), null);
                    }
                }
                if(isServiceClassify)
                {
                    foreach (var properties in objectProperties.Where(properties =>
                     properties.Name == "ServiceName"
                     && columnNames.Contains("ServiceName") &&
                        dataRow["ServiceName"] != DBNull.Value))
                    {
                        properties.SetValue(instanceOfT,
                            Convert.ToString(dataRow["ServiceName"], CultureInfo.CurrentCulture), null);
                    }
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }
    }
    /// <summary>
    /// This class holds ExceptionLog details
    /// </summary>
    public static class ExceptionLog
    {
        /// <summary>
        /// This Method Is Used To LogExceptionMessage
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string LogExceptionMessage(Exception ex)
        {
            return "An Error Occured !";

        }
    }
    /// <summary>
    /// This class holds OpenXMLFunctions  details
    /// </summary>
    public class OpenXMLFunctions
    {
        /// <summary>
        /// This method is used to Export DataSet
        /// </summary>
        /// <param name="table"></param>
        /// <param name="destination"></param>
        public void ExportDataSet(DataTable table, string destination)
        {
            using (var workbook = SpreadsheetDocument.Create(destination, 
                DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();

                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();



                var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = 
                    workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                uint sheetId = 1;
                if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                {
                    sheetId =
                        sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().
                        Select(s => s.SheetId.Value).Max() + 1;
                }

                DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = 
                    new DocumentFormat.OpenXml.Spreadsheet.Sheet
                    { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                sheets.Append(sheet);

                DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                List<String> columns = new List<string>();
                foreach (System.Data.DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);

                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }


                sheetData.AppendChild(headerRow);

                foreach (System.Data.DataRow dsrow in table.Rows)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                    foreach (String col in columns)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()); 
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }


            }
        }
    }
}
