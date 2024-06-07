using CTS.Applens.WorkProfiler.Common;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TicketingModuleUtilsLib.ExportImport.OpenXML
{
    /// <summary>
    /// This class holds XMLOperations details
    /// </summary>
    public class OpenXMLOperations : IOpenXMLOperations
    {
        private enum Formats
        {
            General = 0,
            Number = 1,
            Decimal = 2,
            Currency = 164,
            Accounting = 44,
            DateShort = 14,
            DateLong = 165,
            Time = 166,
            Percentage = 10,
            Fraction = 12,
            Scientific = 11,
            Text = 49
        }


        public static string RemoveLastIndexCharacter(string path)
        {
            if (path.Length > 0)
            {
                int place = path.LastIndexOf(@"\");
                string result = path.Remove(place, (@"\").Length);
                return result;
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the ExcelFileName and Excel Sheet Name.If Sheet Name is not passed it will take the first Sheet
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="SheetName"></param>
        /// <returns>Method returns data table</returns>
        public DataTable ToDataTableBySheetName(string FileName, string SheetName, int headerrowindex = 0,
            int rowstartindex = 2, string[] arr = null)
        {
            try
            {
                if (arr != null && arr.Length > 0)
                {
                    arr = arr.Select(s => s.ToLower(CultureInfo.CurrentCulture)).ToArray();
                }
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Locale = CultureInfo.InvariantCulture;
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(FileName, false))
                {
                    WorksheetPart titlesWorksheetPart = GetWorksheetPart(doc.WorkbookPart, SheetName);
                    Worksheet titlesWorksheet = titlesWorksheetPart.Worksheet;
                    IEnumerable<Row> rows = titlesWorksheet.GetFirstChild<SheetData>().Descendants<Row>();

                    foreach (Cell cell in rows.ElementAt(headerrowindex))
                    {
                        dt.Columns.Add(GetCellValue(doc, cell, false));
                    }

                    foreach (Row row in rows)
                    {
                        if (row.RowIndex != null && row.RowIndex.Value > rowstartindex)
                        {
                                System.Data.DataRow tempRow = dt.NewRow();
                                int columnIndex = 0;
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                                    cellColumnIndex--;
                                    if (columnIndex < cellColumnIndex)
                                    {
                                        do
                                        {
                                            if (dt.Columns.Count > columnIndex)
                                            {
                                                tempRow[columnIndex] = null;

                                            }
                                            columnIndex++;
                                        }
                                        while (columnIndex < cellColumnIndex);
                                    }
                                    if (dt.Columns.Count > columnIndex)
                                    {
                                        bool isDate = false;
                                        if (arr != null && arr.Length > 0)
                                        {
                                            if (arr.Contains(dt.Columns[columnIndex].ColumnName.ToLower(CultureInfo.CurrentCulture)))
                                            {
                                                isDate = true;
                                            }
                                        }
                                        if (isDate)
                                        {
                                            tempRow[columnIndex] = GetFormattedCellValue(doc.WorkbookPart, cell);
                                        }
                                        else
                                        {
                                            tempRow[columnIndex] = GetCellValue(doc, cell, isDate);
                                        }
                                        columnIndex++;
                                    }
                                }
                                dt.Rows.Add(tempRow);
                            
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// To fetch datatable from the excel based on sheet id
        /// </summary>
        /// <param name="FileName">Name of the file</param>
        /// <param name="SheetId">Sheet ID</param>
        /// <returns></returns>
       
        /// <summary>
        /// To get formatted Cell Value
        /// </summary>
        /// <param name="workbookPart">WorkBook Part</param>
        /// <param name="cell">Cell</param>
        /// <returns></returns>
        private static string GetFormattedCellValue(WorkbookPart workbookPart, Cell cell)
        {
            try
            {
                if (cell == null)
                {
                    return null;
                }

                string value = "";
                if (cell.DataType == null)
                {
                    int styleIndex = (int)cell.StyleIndex.Value;
                    double oaDate;
                    CellFormat cellFormat = (CellFormat)workbookPart.WorkbookStylesPart.
                        Stylesheet.CellFormats.ElementAt(styleIndex);
                    uint formatId = cellFormat.NumberFormatId.Value;
                    bool x1 = formatId >= 14 && formatId <= 22;
                    if (((x1)
                    || (formatId >= 164u && formatId <= 180u))
                    && double.TryParse(cell.InnerText, out oaDate))
                    {
                        value = DateTime.FromOADate(oaDate).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        value = cell.InnerText;
                    }
                }
                else
                {
                    int ssiNumber = 0;
                    bool check = Int32.TryParse(cell.CellValue.InnerText, out ssiNumber);
                    switch (cell.DataType.Value)
                    {
                        case CellValues.SharedString:
                            SharedStringItem ssi = workbookPart.SharedStringTablePart.SharedStringTable.
                                Elements<SharedStringItem>().ElementAt(ssiNumber);
                            value = ssi.Text.Text;
                            break;
                        case CellValues.Boolean:
                            value = cell.CellValue.InnerText == "0" ? "false" : "true";
                            break;
                        default:
                            value = cell.CellValue.InnerText;
                            break;
                    }
                }

                return value;
            }
            catch (Exception)
            {

                return cell.InnerText;
            }

        }
        /// <summary>
        /// To get the cell value
        /// </summary>
        /// <param name="document">SpreadSheet document</param>
        /// <param name="cell">Cell</param>
        /// <param name="isDate">Date check</param>
        /// <returns></returns>

        public static string GetCellValue(SpreadsheetDocument document, Cell cell, bool isDate)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            if (cell.CellValue == null && string.IsNullOrEmpty(cell.InnerText))
            {
                return null;
            }
            else if (cell.CellValue == null && !string.IsNullOrEmpty(cell.InnerText))
            {
                return cell.InnerText;
            }
            else
            {
                //mandatory else
            }
            int value = 0;
            bool cellValue = Int32.TryParse(cell.CellValue.InnerXml, out value);
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                string valueTemp = stringTablePart.SharedStringTable.ChildElements[value].InnerText;
                if (valueTemp.Length > 0)
                {
                    return stringTablePart.SharedStringTable.ChildElements[value].InnerText;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (isDate)
                {
                    if (!string.IsNullOrEmpty(cell.CellValue.InnerXml))
                    {
                        return DateTime.FromOADate(Convert.ToDouble(cell.CellValue.InnerXml, CultureInfo.CurrentCulture)).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    if (cellValue)
                    {
                        return value.ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        return cell.CellValue.InnerXml;
                    }
                }
            }
        }
        /// <summary>
        /// Given a cell name, parses the specified cell to get the column name.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. B2)</param>
        /// <returns>Column Name (ie. B)</returns>
        public static string GetColumnName(string cellReference)
        {
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            return match.Value;
        }
        /// <summary>
        /// Given just the column name (no row index), it will return the zero based column index.
        /// Note: This method will only handle columns with a length of up to two (ie. A to Z and AA to ZZ). 
        /// A length of three can be implemented when needed.
        /// </summary>
        /// <param name="columnName">Column Name (ie. A or AB)</param>
        /// <returns>Zero based index if the conversion was successful; otherwise null</returns>
        public static int? GetColumnIndexFromName(string columnName)
        {
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;
        }
        /// <summary>
        /// To get Worksheet part by SheetName
        /// </summary>
        /// <param name="workbookPart">Part Of Workbook</param>
        /// <param name="sheetName">Sheet Name</param>
        /// <returns></returns>
        public WorksheetPart GetWorksheetPart(WorkbookPart workbookPart, string sheetName)
        {
            string relId = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(sheetName) || sheetName.ToLower(CultureInfo.CurrentCulture) == "bulkupload")
                {
                    relId = workbookPart.Workbook.Descendants<Sheet>().First().Id;
                }
                else
                {
                    relId = workbookPart.Workbook.Descendants<Sheet>().First(s => sheetName.Equals(s.Name)).Id;
                }
            }
            catch (InvalidOperationException)
            {
                //CCAP FIX
            }
            return (WorksheetPart)workbookPart.GetPartById(relId);
        }

        /// <summary>
        /// Function to create a excelsheet by DataSet.For each DataTable it will create a Sheet
        /// </summary>
        /// <param name="_DataSet">DataSet</param>
        /// <param name="FileName">FileName</param>
        /// <returns></returns>
        public string ToExcelSheetByDataSet(DataTable dt, DataSet _DataSet, string FileName)
        {
            using (var workbook = SpreadsheetDocument.Create(FileName, DocumentFormat.OpenXml.
                SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                uint sheetId = 1;
                if (_DataSet == null)
                {
                    _DataSet = new DataSet();
                    _DataSet.Locale = CultureInfo.InvariantCulture;
                    _DataSet.Tables.Add(dt.Copy());
                }
                foreach (DataTable table in _DataSet.Tables)
                {
                    var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                    sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.
                        Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                    if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().
                            Select(s => s.SheetId.Value).Max() + 1;
                    }

                    DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.
                        Spreadsheet.Sheet
                    { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);

                    DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    List<String> columns = new List<string>();
                    foreach (DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(headerRow);

                    foreach (DataRow dsrow in table.Rows)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                        foreach (String col in columns)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.
                                Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString());
                            newRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(newRow);
                    }
                }
            }
            if (!File.Exists(FileName))
            {
                //ccap fix
            }

            return FileName;
        }
        /// <summary>
        /// Function to Open a excelsheet by DataTable.For each DataTable it will create a Sheet
        /// </summary>
        /// <param name="table">table</param>
        /// <param name="FileName">FileName</param>
        /// <returns></returns>
        public string ToExcelSheetByDataTable(DataTable table, DataSet ds, string FileName, string sheetName,
            string[] HiddenColumns, int rowstoskip = 0)
        {
            using (var workbook = SpreadsheetDocument.Open(FileName, true))
            {
                WorkbookPart wbp = workbook.WorkbookPart;
                wbp.WorkbookStylesPart.Stylesheet = GenerateStylesheet();

                // Get the worksheet with the required name.
                // To be used to match the ID for the required sheet data
                // because the Sheet class and the SheetData class aren't
                // linked to each other directly.
                Sheet s = null;
                if (wbp.Workbook.Sheets.Elements<Sheet>().Count(nm => nm.Name == sheetName) == 0)
                {
                    // no such sheet with that name
                    workbook.Close();
                    return "";
                }
                else
                {
                    s = wbp.Workbook.Sheets.Elements<Sheet>().Where(nm => nm.Name == sheetName).First();
                }

                WorksheetPart wsp = (WorksheetPart)workbook.WorkbookPart.GetPartById(s.Id.Value);
                SheetData sd = wsp.Worksheet.GetFirstChild<SheetData>();
                workbook.Save();
                List<String> columns = new List<string>();

                foreach (DataColumn column in table.Columns)
                {

                    columns.Add(column.ColumnName);

                }

                if (sheetName == "Weekly")
                {
                    List<string> DataDictionaryColumns = new List<string>();
                    DataDictionaryColumns.Add("User ID");
                    DataDictionaryColumns.Add("User Name");
                    DataDictionaryColumns.Add("Status");
                    DataDictionaryColumns.Add("Total Hours");
                    DataDictionaryColumns.Add(HiddenColumns[0].Split(' ')[0]);
                    DataDictionaryColumns.Add(HiddenColumns[1].Split(' ')[0]);
                    DataDictionaryColumns.Add(HiddenColumns[2].Split(' ')[0]);
                    DataDictionaryColumns.Add(HiddenColumns[3].Split(' ')[0]);
                    DataDictionaryColumns.Add(HiddenColumns[4].Split(' ')[0]);
                    DataDictionaryColumns.Add(HiddenColumns[5].Split(' ')[0]);
                    DataDictionaryColumns.Add(HiddenColumns[6].Split(' ')[0]);
                    DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                    foreach (string dsrow in DataDictionaryColumns)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.CellValue = new CellValue(dsrow);
                        cell.DataType = new EnumValue<CellValues>(CellValues.String);
                        if (HiddenColumns != null && HiddenColumns.Contains(dsrow.ToLower(CultureInfo.CurrentCulture)))
                        {
                            cell.StyleIndex = 2;
                        }
                        else
                        {
                           //CCAP FIX
                        }
                        newRow.AppendChild(cell);
                    }

                    sd.AppendChild(newRow);
                }

                if (sheetName == "DataDictionary")
                {
                    List<string> DataDictionaryColumns = new List<string>();
                    DataDictionaryColumns.Add("Application Name");
                    DataDictionaryColumns.Add("Cause Code");
                    DataDictionaryColumns.Add("Resolution Code");
                    DataDictionaryColumns.Add("Debt Category");
                    DataDictionaryColumns.Add("Avoidable Flag");
                    DataDictionaryColumns.Add("Residual Flag");
                    DataDictionaryColumns.Add("Reason For Residual");
                    DataDictionaryColumns.Add("Expected Completion Date");
                    DataDictionaryColumns.Add("ProjectID");

                    DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                    foreach (string dsrow in DataDictionaryColumns)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.CellValue = new CellValue(dsrow);
                        cell.DataType = new EnumValue<CellValues>(CellValues.String);
                        if (HiddenColumns != null && HiddenColumns.Contains(dsrow.ToLower(CultureInfo.CurrentCulture)))
                        {
                            cell.StyleIndex = 2;
                        }
                        else
                        {
                           //CCAP FIX
                        }
                        newRow.AppendChild(cell);
                    }

                    sd.AppendChild(newRow);
                }
                else if (sheetName == "Ticket_Info")
                {

                    DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                    foreach (string dsrow in columns)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.CellValue = new CellValue(dsrow);
                        cell.DataType = new EnumValue<CellValues>(CellValues.String);
                        if (HiddenColumns != null && HiddenColumns.Contains(dsrow.ToLower(CultureInfo.CurrentCulture)))
                        {
                            cell.StyleIndex = 2;
                        }
                        else
                        {
                            // CCAP FIX
                        }
                        newRow.AppendChild(cell);
                    }

                    sd.AppendChild(newRow);
                }
                else
                {
                    //mandatory else
                }
                foreach (DataRow dsrow in table.Rows)
                {


                    DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                    foreach (String col in columns)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        string objDataType = dsrow[col].GetType().ToString();
                        Int64 outputvalue = 0;
                        decimal outputdecimalvalue = 0;
                        if (Int64.TryParse(dsrow[col].ToString(), out outputvalue))
                        {
                            cell.CellValue = new CellValue(outputvalue.ToString(CultureInfo.CurrentCulture));
                            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }
                        else if (dsrow[col].ToString().Contains(".") && Decimal.TryParse(dsrow[col].ToString(),
                            out outputdecimalvalue))
                        {
                            cell.CellValue = new CellValue(outputdecimalvalue.ToString(CultureInfo.CurrentCulture));
                            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }
                        else
                        {
                            cell.CellValue = new CellValue(dsrow[col].ToString());
                            cell.DataType = new EnumValue<CellValues>(CellValues.String);
                        }
                        if (HiddenColumns != null && HiddenColumns.Contains(col.ToLower(CultureInfo.CurrentCulture)))
                        {
                            cell.StyleIndex = 2;
                        }
                        else
                        {
                            cell.StyleIndex = 0;
                        }
                        newRow.AppendChild(cell);
                    }

                    sd.AppendChild(newRow);
                }

                workbook.Save();
            }
            if (!File.Exists(FileName))
            {
                //CCAP FIX
            }

            return FileName;
        }


        public string ToExcelSheetDebtUnclassifiedByDataTable(DataTable table, DataSet ds, string FileName, string sheetName,
            int rowstoskip = 0, bool appendColumns = false)
        {
            using (var workbook = SpreadsheetDocument.Open(FileName.Replace("..", "",StringComparison.CurrentCulture), true))
            {
                WorkbookPart wbp = workbook.WorkbookPart;

                Sheet s = null;
                if (string.IsNullOrEmpty(sheetName))
                {
                    s = wbp.Workbook.Sheets.Elements<Sheet>().First();

                }
                else
                {

                    if (wbp.Workbook.Sheets.Elements<Sheet>().Count(nm => nm.Name == sheetName) == 0)
                    {
                        workbook.Close();
                        return "";
                    }
                    else
                    {
                        s = wbp.Workbook.Sheets.Elements<Sheet>().Where(nm => nm.Name == sheetName).First();
                    }
                }

                WorksheetPart wsp = (WorksheetPart)workbook.WorkbookPart.GetPartById(s.Id.Value);
                SheetData sd = wsp.Worksheet.GetFirstChild<SheetData>();
                workbook.Save();
                List<String> columns = new List<string>();

                DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                foreach (DataColumn column in table.Columns)
                {

                    columns.Add(column.ColumnName);

                    if (appendColumns)
                    {
                        Run run1 = new Run();
                        run1.Append(new Text(column.ColumnName));
                        RunProperties run1Properties = new RunProperties();
                        run1Properties.Append(new Bold());

                        run1Properties.Append(new Alignment
                        {
                            Horizontal = HorizontalAlignmentValues.Center,
                            Vertical = VerticalAlignmentValues.Center
                        });
                        run1.RunProperties = run1Properties;


                        InlineString inlineString = new InlineString();
                        inlineString.Append(run1);


                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.InlineString;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                        cell.StyleIndex = (UInt32Value)1U;
                        cell.Append(inlineString);

                        headerRow.AppendChild(cell);
                    }

                }
                if (appendColumns)
                {
                    sd.AppendChild(headerRow);
                }
                foreach (DataRow dsrow in table.Rows)
                {


                    DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                    foreach (String col in columns)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        string objDataType = dsrow[col].GetType().ToString();

                        if (objDataType.Contains(TypeCode.Int32.ToString(), StringComparison.OrdinalIgnoreCase) || objDataType.Contains
                            (TypeCode.Int64.ToString(), StringComparison.OrdinalIgnoreCase)
                            || objDataType.Contains(TypeCode.Decimal.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            cell.CellValue = new CellValue(dsrow[col].ToString());
                            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }
                        else if (objDataType.Contains(TypeCode.DateTime.ToString(), StringComparison.OrdinalIgnoreCase))
                        {

                            dsrow[col] = Convert.ToDateTime(dsrow[col], CultureInfo.CurrentCulture).ToShortDateString();
                            cell.CellValue = new CellValue(Convert.ToDateTime(dsrow[col], CultureInfo.CurrentCulture).ToShortDateString());
                            cell.DataType = new EnumValue<CellValues>(CellValues.String);
                        }
                        else
                        {
                            if (Regex.IsMatch(dsrow[col].ToString(), @"^\d+$"))
                            {
                                cell.CellValue = new CellValue(dsrow[col].ToString());
                                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                            }
                            else
                            {
                                cell.CellValue = new CellValue(dsrow[col].ToString());
                                cell.DataType = new EnumValue<CellValues>(CellValues.String);
                            }
                        }
                        newRow.AppendChild(cell);
                    }

                    sd.AppendChild(newRow);
                }


                workbook.Save();


            }
            if (!OpenXMLOperations.ValidateFileIsExist(FileName))
            {
                throw new Exception("Error Occured while exporting to Excel");
            }

            return FileName.Replace("..", "", StringComparison.OrdinalIgnoreCase);
        }

        public static bool ValidateFileIsExist(string sourceFolderPath)
        {
            Regex rgx = new Regex("(\\\\?([^\\/]*[\\/])*)([^\\/]+)");
            if (sourceFolderPath != null)
            {
                sourceFolderPath = sourceFolderPath.Replace(">", "", StringComparison.OrdinalIgnoreCase);
                sourceFolderPath = sourceFolderPath.Replace("<", "", StringComparison.OrdinalIgnoreCase);
                sourceFolderPath = sourceFolderPath.Replace("..", "", StringComparison.OrdinalIgnoreCase);
                if (rgx.IsMatch(sourceFolderPath))
                {
                    return File.Exists(sourceFolderPath);
                }
            }
            return false;
        }


        /// <summary>
        /// To convert data table as Excel
        /// </summary>
        /// <param name="table"></param>
        /// <param name="ds"></param>
        /// <param name="FileName"></param>
        /// <param name="sheetName"></param>
        /// <param name="HiddenColumns"></param>
        /// <param name="rowstoskip"></param>
        /// <returns>Method returns Excel</returns>
        public string DataTableToExcel(DataTable table, DataSet ds, string FileName, string sheetName,
            string[] HiddenColumns, int rowstoskip = 0)
        {
            using (var workbook = SpreadsheetDocument.Open(FileName, true))
            {
                WorkbookPart wbp = workbook.WorkbookPart;
                Sheet s = null;
                if (wbp.Workbook.Sheets.Elements<Sheet>().Count(nm => nm.Name == sheetName) == 0)
                {
                    workbook.Close();
                    return "";
                }
                else
                {
                    s = wbp.Workbook.Sheets.Elements<Sheet>().Where(nm => nm.Name == sheetName).First();
                }

                WorksheetPart wsp = (WorksheetPart)workbook.WorkbookPart.GetPartById(s.Id.Value);
                SheetData sd = wsp.Worksheet.GetFirstChild<SheetData>();
                workbook.Save();
                List<String> columns = new List<string>();
                foreach (DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);
                }

                foreach (DataRow dsrow in table.Rows)
                {


                    DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                    foreach (String col in columns)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        string objDataType = dsrow[col].GetType().ToString();
                        Int64 outputvalue = 0;
                        decimal outputdecimalvalue = 0;
                        if (Int64.TryParse(dsrow[col].ToString(), out outputvalue))
                        {
                            cell.CellValue = new CellValue(outputvalue.ToString(CultureInfo.CurrentCulture));
                            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }
                        else if (dsrow[col].ToString().Contains(".") && Decimal.TryParse(dsrow[col].ToString(),
                            out outputdecimalvalue))
                        {
                            cell.CellValue = new CellValue(outputdecimalvalue.ToString(CultureInfo.CurrentCulture));
                            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        }
                        else
                        {
                            cell.CellValue = new CellValue(dsrow[col].ToString());
                            cell.DataType = new EnumValue<CellValues>(CellValues.String);
                        }
                        newRow.AppendChild(cell);
                    }

                    sd.AppendChild(newRow);
                }

                workbook.Save();
            }
            if (!File.Exists(FileName))
            {
                //CCAP FIX
            }

            return FileName;
        }
        /// <summary>
        /// To generate style sheet
        /// </summary>
        /// <returns></returns>

        private Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font(
                    new FontSize { Val = 10 }

                ),
                new Font(
                    new FontSize { Val = 10 },
                    new Color { Rgb = "FFFFFF" }
                ));

            Fills fills = new Fills(
                     new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue { Value = "000000" } })
                     { PatternType = PatternValues.Solid }),
                    new Fill(new PatternFill { PatternType = PatternValues.Gray125 }),
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue { Value = "FFFFFF" } })
                    { PatternType = PatternValues.Solid })
                );

            Borders borders = new Borders(
                    new Border(),
                    new Border(
                        new LeftBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(),
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true },
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true }
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }
        /// <summary>
        /// Used to return a new OpenXML Cell by its celltype and value
        /// </summary>
        /// <param name="isNumberic">Cell DataType</param>
        /// <param name="cellValue">Cell Value</param>
        /// <returns>OpenXMl Cell</returns>
        public Cell GetCells(bool isNumberic, string cellValue)
        {
            

            
            Cell cell = new Cell
            {
                CellValue = new CellValue(cellValue),
                DataType = new EnumValue<CellValues>(CellValues.String),
            };
            if (isNumberic)
            {
                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            }

            return cell;
        }

        /// <summary>
        /// Used to return a new OpenXML Row by its cellvalue
        /// </summary>
        /// <param name="cell">Cell Value</param>
        /// <returns>OpenXMl Row</returns>
        public Row GetRows(IEnumerable<Cell> cell, SheetData sheetData)
        {
            Row row = new Row();
            foreach (var item in cell)
            {
                row.Append(item);
            }
            sheetData.AppendChild(row);
            return row;
        }

        /// <summary>
        /// Check the Validate status of the excel
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ExcelPrimaryValidation(string filePath)
        {
            //**Validtated before upload Excel Check **//
            var isValidated = IsValidatedExcelBySheet(filePath);
            if (!isValidated)
            {
                return Constants.TemplateNotValidated;
            }
            return string.Empty;
        }
        public static bool IsValidatedExcelBySheet(string filename, string sheetName = "IsValidated")
        {
            try
            {
                DataTable isValidatedDt = new OpenXMLOperations().ToDataTableBySheetName(filename, sheetName, 0, 1);
                if (isValidatedDt.Rows.Count > 0)
                {
                    if (isValidatedDt.Rows[0][0].ToString() != "1")
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

    }
}
