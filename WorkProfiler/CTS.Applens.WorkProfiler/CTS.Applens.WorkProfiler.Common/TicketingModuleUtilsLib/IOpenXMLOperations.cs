using System.Data;

namespace TicketingModuleUtilsLib.ExportImport.OpenXML
{
    /// <summary>
    /// Interface for OpenXMLOperations
    /// </summary>
    internal interface IOpenXMLOperations
    {
        /// <summary>
        /// this is used to ToDataTableBySheetName
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="SheetName"></param>
        /// <param name="headerrowindex"></param>
        /// <param name="rowstartindex"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        DataTable ToDataTableBySheetName(string FileName, string SheetName, int headerrowindex, int rowstartindex,
            string[] arr = null);
        /// <summary>
        /// this is used to ToDataTableBySheetId
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="SheetId"></param>
        /// <returns></returns>
        
        /// <summary>
        /// this is used to ToExcelSheetByDataSet
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="_DataSet"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        string ToExcelSheetByDataSet(DataTable dt, DataSet _DataSet, string FileName);
        /// <summary>
        /// this is used to ToExcelSheetByDataTable
        /// </summary>
        /// <param name="table"></param>
        /// <param name="ds"></param>
        /// <param name="FileName"></param>
        /// <param name="sheetName"></param>
        /// <param name="HiddenColumns"></param>
        /// <param name="rowstoskip"></param>
        /// <returns></returns>
        string ToExcelSheetByDataTable(DataTable table, DataSet ds, string FileName, string sheetName,
            string[] HiddenColumns, int rowstoskip);
    }
}
