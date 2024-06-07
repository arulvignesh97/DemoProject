﻿/***************************************************************************
 * COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET
 * Copyright [2018] – [2021] Cognizant. All rights reserved.
 * NOTICE: This unpublished material is proprietary to Cognizant and
 * its suppliers, if any. The methods, techniques and technical
 * concepts herein are considered Cognizant confidential and/or trade secret information.
 * This material may be covered by U.S. and/or foreign patents or patent applications.
 * Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
 ***************************************************************************/

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;
using System.Reflection;

namespace CTS.Applens.Framework
{
    public class UploadExcelData
    {
        /// <summary>
        /// Used to get datatable from Excel Sheet Name and Sheet Name
        /// </summary>
        /// <param name="FileName">Excel Sheet Path</param>
        /// <param name="SheetName">Excel sheet Name</param>
        /// <returns>DataTable</returns>
        public DataTable ToDataTableBySheetName(string FileName, string SheetName, int headerrowindex = 0, int rowstartindex = 1)
        {
            try
            {
                SpreadsheetDocument doc = SpreadsheetDocument.Open(FileName, false);
                return ConvertSpreadSheetToDatatable(doc, SheetName, headerrowindex, rowstartindex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Used to get datatable from Excel Stream and Sheet Name
        /// </summary>
        /// <param name="fileStream">Excel Stream</param>
        /// <param name="SheetName">Excel sheet Name</param>
        /// <returns>DataTable</returns>
        public DataTable ToDataTableByStream(Stream fileStream, string SheetName, int headerrowindex = 0, int rowstartindex = 1
            , bool IsDefaultFirstSheet = true)
        {
            try
            {
                SpreadsheetDocument doc = SpreadsheetDocument.Open(fileStream, false);
                return ConvertSpreadSheetToDatatable(doc, SheetName, headerrowindex, rowstartindex, IsDefaultFirstSheet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable ConvertSpreadSheetToDatatable(SpreadsheetDocument document, string SheetName, int headerrowindex,
            int rowstartindex, bool IsDefaultFirstSheet = true)
        {
            //Create a new DataTable.
            System.Data.DataTable datatable = new System.Data.DataTable();
            //Open the Excel file in Read Mode using OpenXML
            using (document)
            {

                WorksheetPart titlesWorksheetPart = GetWorksheetPart(document.WorkbookPart, SheetName, IsDefaultFirstSheet);
                if (titlesWorksheetPart != null)
                {
                    Worksheet titlesWorksheet = titlesWorksheetPart.Worksheet;
                    var sheetData = titlesWorksheet.WorksheetPart.Worksheet.Elements<SheetData>().First();
                    IEnumerable<Row> rows = sheetData.Elements<Row>();
                    foreach (Cell cell in rows.ElementAt(headerrowindex))
                    {
                        int styleIndex = cell.StyleIndex != null ? (int)cell.StyleIndex.Value : 0;
                        CellFormat cellFormat = (CellFormat)document.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats.ElementAt(styleIndex);

                        datatable.Columns.Add(GetCellValue(document, cell, cellFormat.NumberFormatId));

                    }
                    int i = 1;
                    foreach (Row row in rows)
                    {
                        if (i > rowstartindex)
                        {
                            System.Data.DataRow tempRow = datatable.NewRow();
                            int columnIndex = 0;
                            foreach (Cell cell in row.Descendants<Cell>())
                            {
                                int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                                cellColumnIndex--;
                                if (columnIndex < cellColumnIndex)
                                {

                                    do
                                    {
                                        if (datatable.Columns.Count > columnIndex)
                                        {
                                            tempRow[columnIndex] = null;

                                        }
                                        columnIndex++;
                                    }
                                    while (columnIndex < cellColumnIndex);
                                }
                                if (datatable.Columns.Count > columnIndex)
                                {

                                    int styleIndex = 0;
                                    if (cell.StyleIndex != null)
                                    {
                                        styleIndex = (int)cell.StyleIndex.Value;
                                    }
                                    CellFormat cellFormat = (CellFormat)document.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats.ElementAt(styleIndex);


                                    tempRow[columnIndex] = GetCellValue(document, cell, cellFormat.NumberFormatId);


                                    columnIndex++;
                                }
                            }
                            datatable.Rows.Add(tempRow);
                        }
                        i++;
                    }
                }
                else
                {
                    datatable = null;
                }
            }
            return datatable;
        }

        /// <summary>
        /// Get All Work sheets Names
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name=""></param>
        /// <returns></returns>
        public Sheets GetAllWorksheets(string fileName)
        {
            Sheets theSheets = null;

            using (SpreadsheetDocument document =
                SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                theSheets = wbPart.Workbook.Sheets;
            }
            return theSheets;
        }
        /// <summary>
        /// Convert IEnumarable List To DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Linqlist"></param>
        /// <returns></returns>
        public DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();


            PropertyInfo[] columns = null;

            if (Linqlist == null)
            {
                return dt;
            }

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            var dt = new DataTable();
            string dirctoryName = Path.GetDirectoryName(csv_file_path);
            string fName = Path.GetFileNameWithoutExtension(csv_file_path);
            string validatePath = Path.Combine(dirctoryName, fName, ".csv");
            validatePath = RemoveLastIndexCharacter(validatePath);

            using (var reader = new StreamReader(validatePath))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
            {
                using (var dr = new CsvDataReader(csv))
                {
                    dt.Load(dr);
                }
            }
            return dt;
        }


        /// <summary>
        /// Used To Get excel sheet cell value
        /// </summary>
        /// <param name="document">Spreadsheet Document</param>
        /// <param name="cell">Excel Cell</param>
        /// <param name="index">Index</param>
        /// <returns>Cell Value</returns>
        public static string GetCellValue(SpreadsheetDocument document, Cell cell, DocumentFormat.OpenXml.UInt32Value index)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            if (cell.CellValue == null)
            {
                return null;
            }
            string value = cell.CellValue.InnerXml;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                string valueTemp = stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                if (valueTemp.Length > 0)
                {
                    return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                //Date Convertion
                if (index.Value == 14 && cell.CellValue != null)
                {
                    return DateTime.FromOADate(Convert.ToDouble(cell.CellValue.InnerXml)).ToString();
                }
                else
                {
                    return value;
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
            // Create a regular expression to match the column name portion of the cell name.
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
        /// Get Work sheet Part
        /// </summary>
        /// <param name="workbookPart"></param>
        /// <param name="sheetName">Excel Sheet Name</param>
        /// <returns></returns>
        public WorksheetPart GetWorksheetPart(WorkbookPart workbookPart, string sheetName, bool IsDefaultFirstSheet = true)
        {
            string relId = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(sheetName))
                {
                    relId = workbookPart.Workbook.Descendants<Sheet>().First().Id;
                }
                else
                {
                    relId = workbookPart.Workbook.Descendants<Sheet>().First(s => sheetName.Equals(s.Name)).Id;
                }
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
                if (IsDefaultFirstSheet)
                {
                    relId = workbookPart.Workbook.Descendants<Sheet>().First().Id;
                }
                else
                {
                    relId = null;
                }

            }

            return (relId != null) ? (WorksheetPart)workbookPart.GetPartById(relId) : null;
        }


        /// <summary>
        /// ClearSheetData
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="SheetName"></param>
        /// <param name="rowstartindex"></param>
        private void ClearSheetData(SheetData sheet, int rowstartindex = 0)
        {
            if (rowstartindex == 0)
            {
                sheet.RemoveAllChildren();
                return;
            }
            int i = 1;
            IEnumerable<Row> rows = sheet.Elements<Row>();
            foreach (Row row in rows)
            {
                if (i > rowstartindex)
                {
                    row.RemoveAllChildren();
                }
                i++;
            }
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
        /// Delete Upload files
        /// </summary>
        /// <param name="fileName">File Name</param>
        public void DeleteUploadFile(string fileName)
        {


            if (fileName != null || fileName != string.Empty)
            {
                if ((System.IO.File.Exists(fileName)))
                {
                    string dirctoryName = Path.GetDirectoryName(fileName);
                    string fName = Path.GetFileName(fileName);
                    string validatePath = Path.Combine(dirctoryName, fName, ".xlsx");
                    validatePath = RemoveLastIndexCharacter(validatePath);
                    System.IO.File.Delete(validatePath);
                }

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string RemoveLastIndexCharacter(string path)
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

    }
}
