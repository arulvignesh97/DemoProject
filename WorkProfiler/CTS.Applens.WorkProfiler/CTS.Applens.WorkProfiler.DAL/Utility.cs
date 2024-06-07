using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Data.SqlClient;
using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.Framework;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CTS.Applens.WorkProfiler.Entities.Base;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;

namespace CTS.Applens.WorkProfiler.DAL
{
    public static class Utility
    {
        public static readonly string InputDestPath = InitialLearningConstants.InputDestinationPath;
        private static readonly string connectionString = new AppSettings().AppsSttingsKeyValues["ConnectionStrings:AppLensConnection"];
        /// <summary> CM
        /// Download the file from Data Lake
        /// </summary>
        /// <param name="datalakeFilePath"></param>
        /// <param name="downloadPath"></param>
        /// <returns>downloadPath</returns>
        public static string DownloadFile(string datalakeFilePath, string downloadPath, int SupportTypeID)
        {
            string result = string.Empty;
            try
            {

                downloadPath = string.Concat(downloadPath, Path.GetFileName(datalakeFilePath));
                var response = DownloadFileFromDL(datalakeFilePath, downloadPath, SupportTypeID);
                if (response.ToString().ToLower() == "ok")
                {
                    result = downloadPath;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        /// <summary>
        /// CheckIfFileExists
        /// </summary>
        /// <param name="datalakeFilePath"></param>
        /// <param name="downloadPath"></param>
        /// <returns>string</returns>
        public static string CheckIfFileExists(string datalakeFilePath, string downloadPath, int SupportId)
        {
            string result = string.Empty;
            try
            {
                downloadPath = string.Concat(downloadPath, Path.GetFileName(datalakeFilePath));
                var response = CheckWhetherFileExists(datalakeFilePath, downloadPath, SupportId);
                if (response.ToString().ToLower() == "ok")
                {
                    result = downloadPath;
                }
                else
                {
                    result = response.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        /// <summary>
        /// Method to get the folder directory structure where the file to be place in datalake
        /// Sample File Path: DataLake => RFP/{{Bu Name}}/Vertical/{{AnalysisId}}/{{Version}}/DataFiles
        /// </summary>
        /// <param name="verticalHorizontalName"></param>
        /// <param name="debtAnalysisId"></param>
        /// <param name="version"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string CreateTargetPath(string verticalHorizontalName, int debtAnalysisId, int version,
            string fileName)
        {
            string directoryFolder = string.Empty;
            directoryFolder = string.Concat(new AppSettings().AppsSttingsKeyValues["DataLakePathRoot"],
                verticalHorizontalName.Replace(" ", string.Empty), "/Vertical/", debtAnalysisId,
                "/V", version, "/", fileName);

            return directoryFolder;
        }

        /// <summary>
        /// Create/Upload Mapper InputFile to Datalake
        /// </summary>
        /// <param name="MappingFileFullPath"></param>
        /// <param name="targetPath"></param>
        /// <param name="rfpDestinationPath"></param>
        /// <param name="columnMappingDestinationPath"></param>
        /// <returns></returns>
        private static string CreateMapperInputFile(string sourceFilePath, string dataLaketargetPath,
            string Columnname, int SupportTypeID)
        {
            ErrorLOGInfra("1 . in CreateMapperInputFile  sourceFilePath =" + sourceFilePath
                + "dataLaketargetPath is" + dataLaketargetPath + "SupportTypeID " + SupportTypeID,
               "SubmitNoiseEliminationMapReduceJob", 10337);
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            dt.Columns.Add(Columnname);

            string inputfilepath = sourceFilePath.Replace(Path.GetFileNameWithoutExtension(sourceFilePath),
                "Inputfilepaths");

            string inputDestinationPath = string.Format(InputDestPath, dataLaketargetPath);
            string dataLaketargetPathwithDump = string.Format("/{0}/{1}", dataLaketargetPath, Path.
                GetFileName(sourceFilePath));

            dt.Rows.Add(dataLaketargetPathwithDump);

            try
            {
                ErrorLOGInfra("1.1 . in ExportDatatableToExcel inputfilepath is =" + inputfilepath,
               "CreateMapperInputFile", 10337);
                ExportDatatableToExcel(dt, inputfilepath);

            }
            catch (Exception ex)
            {
                ErrorLOGInfra("1.2 In Catch block CreateMapperInputFile" + ex.Message, "", 1);
                ErrorLOGInfra("1.2 In Catch block CreateMapperInputFile" + ex.InnerException.Message, "", 1);
                throw ex;
            }
            UploadFileToDL(inputfilepath, dataLaketargetPath, SupportTypeID);
            ErrorLOGInfra("1.3 after uploading in CreateMapperInputFile", "", 1);
            return inputDestinationPath;
        }


        /// <summary>
        /// CreateMapperInputFileForSampling
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="DescFilepath"></param>
        /// <param name="ResFilePath"></param>
        /// <param name="dataLaketargetPath"></param>
        /// <returns>string</returns>
        private static string CreateMapperInputFileForSampling(string sourceFilePath, string DescFilepath,
            string ResFilePath, string dataLaketargetPath, int SupportTypeID)
        {


            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            dt.Columns.Add("Inputfile");

            string inputfilepath = sourceFilePath.Replace(Path.GetFileNameWithoutExtension(sourceFilePath),
                "Inputfilepaths");

            string inputDestinationPath = string.Format(InputDestPath, dataLaketargetPath);
            string dataLaketargetPathwithDump = string.Format("/{0}/{1}", dataLaketargetPath, Path.
                GetFileName(sourceFilePath));

            dt.Rows.Add(dataLaketargetPathwithDump);

            dt.Columns.Add("DescNoisefile");

            dataLaketargetPathwithDump = string.Format("/{0}/{1}", dataLaketargetPath, Path.
                GetFileName(DescFilepath));
            dt.Rows[0]["DescNoisefile"] = dataLaketargetPathwithDump;
            if (!string.IsNullOrEmpty(ResFilePath))
            {
                dt.Columns.Add("ResNoisefile");

                dataLaketargetPathwithDump = string.Format("/{0}/{1}", dataLaketargetPath, Path.
                    GetFileName(ResFilePath));
                dt.Rows[0]["ResNoisefile"] = dataLaketargetPathwithDump;
            }


            try
            {
                ExportDatatableToExcel(dt, inputfilepath);

            }
            catch (Exception ex)
            {
                ErrorLOG(string.Empty, string.Concat("in CreateMapperInputFile", ex.Message), 2);
                throw ex;
            }
            UploadFileToDL(inputfilepath, dataLaketargetPath, SupportTypeID);

            return inputDestinationPath;
        }

        /// <summary>
        /// Method to export columnMapping DataTable to excel
        /// </summary>
        /// <param name="dtColumnMapping"></param>
        /// <param name="filePath"></param>
        private static void ExportDatatableToExcel(DataTable dtColumnMapping, string filePath)
        {


            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                DataTableToCSV(dtColumnMapping, filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to connect datalake and upload files
        /// </summary>
        /// <param name="sourcepath"></param>
        /// <param name="targetPath"></param>
        private static void UploadFileToDL(string sourcepath, string targetPath, int SupportTypeId)
        {
            ErrorLOGInfra("3. In UploadFileToDL sourcepath is " + sourcepath + "targetPath is"
                            + targetPath, "ILUploadFileToDL", 1);
            string hdfcusername = new AppSettings().AppsSttingsKeyValues["DataLakeIP"];
            if (SupportTypeId == 2)
            {
                hdfcusername = new AppSettings().AppsSttingsKeyValues["DataLakeInfraIP"];
            }

            string dataLakeUsername = new AppSettings().AppsSttingsKeyValues["DataLakeUsername"];
            string dataLakePort = new AppSettings().AppsSttingsKeyValues["DataLakePort"];

            
        }

        /// <summary>
        /// Method to connect datalake and download files
        /// </summary>
        /// <param name="datalakeFilePath"></param>
        /// <param name="downloadPath"></param>
        /// <returns></returns>
        private static HttpStatusCode DownloadFileFromDL(string datalakeFilePath, string downloadPath,
            int SupportTypeId)
        {
            string hdfcusername = new AppSettings().AppsSttingsKeyValues["DataLakeIP"];
            if (SupportTypeId == 2)
            {
                hdfcusername = new AppSettings().AppsSttingsKeyValues["DataLakeInfraIP"];
            }

            string dataLakeUsername = new AppSettings().AppsSttingsKeyValues["DataLakeUsername"];
            string dataLakePort = new AppSettings().AppsSttingsKeyValues["DataLakePort"];

            return HttpStatusCode.OK;
        }
        /// <summary>
        /// Check file exist or not
        /// </summary>
        /// <param name="datalakeFilePath"></param>
        /// <param name="downloadPath"></param>
        /// <returns></returns>
        private static HttpStatusCode CheckWhetherFileExists(string datalakeFilePath, string downloadPath,
            int SupportTypeId)
        {
            string hdfcusername = new AppSettings().AppsSttingsKeyValues["DataLakeIP"];
            if (SupportTypeId == 2)
            {
                hdfcusername = new AppSettings().AppsSttingsKeyValues["DataLakeInfraIP"];
            }
            string dataLakeUsername = new AppSettings().AppsSttingsKeyValues["DataLakeUsername"];
            string dataLakePort = new AppSettings().AppsSttingsKeyValues["DataLakePort"];
            return HttpStatusCode.Continue;
        }

        /// <summary>
        /// Export CS file to DataTable
        /// </summary>
        /// <param name="csv_file_path"></param>
        /// <returns></returns>
        public static DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            csvData.Locale = CultureInfo.InvariantCulture;
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();

                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return csvData;
        }

        /// <summary>
        /// DataTableToCSV
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        public static void DataTableToCSV(DataTable dt, string filePath)
        {
            StringBuilder text = new StringBuilder();
            try
            {
                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName);
                text.AppendLine(string.Join(",", columnNames));

                if (!Directory.Exists(filePath.Replace(Path.GetFileName(filePath), string.Empty)))
                {
                    Directory.CreateDirectory(filePath.Replace(Path.GetFileName(filePath), string.Empty));
                }
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {

                        IEnumerable<string> fields1 = row.ItemArray.Select(field1 => string.Concat("\"",
                        field1.ToString().Replace("\"", "\"\""), "\""));
                        text.AppendLine(string.Join(",", fields1));

                    }
                }
                File.WriteAllText(filePath, text.ToString());
            }
            catch (IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// If uploaded RFP file is validated upload the RFP with mapping file to datalake
        /// </summary>
        /// <param name="deptAnalysis"></param>
        /// <param name="dtColumnMapping"></param>
        public static GetMLJobDetails SumbmitSamplingMapReduceJob(string esaProjectID,
            string initialLearningId, string sourceFilePath, string DescPath, string ResPath,
            string UserID, Int64 ProjectID, int SupportTypeID)
        {
            GetMLJobDetails mlJobDetails = new GetMLJobDetails();
            try
            {

                string dataLaketargetPath = string.Concat(new AppSettings().AppsSttingsKeyValues["DataLakePathRoot"],
                    esaProjectID, "/", initialLearningId);
                string fileName = Path.GetFileName(sourceFilePath);
                string inputDestinationPath = CreateMapperInputFileForSampling(sourceFilePath, DescPath,
                    ResPath, dataLaketargetPath, SupportTypeID);
                string result = string.Empty;
                UploadFileToDL(sourceFilePath, dataLaketargetPath, SupportTypeID);
                if (SupportTypeID == 2)
                {
                    string fileNamedesc = Path.GetFileName(DescPath);
                    UploadFileToDL(DescPath, dataLaketargetPath, SupportTypeID);

                    if (!string.IsNullOrEmpty(ResPath))
                    {
                        string fileNameres = Path.GetFileName(ResPath);

                        UploadFileToDL(ResPath, dataLaketargetPath, SupportTypeID);

                    }

                    //using (var client = new HttpClient())
                    var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
                    var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                    using (var client = httpClientFactory.CreateClient())
                    {
                        ServiceParamDetails objParamDetails = new ServiceParamDetails();
                        objParamDetails.Input = inputDestinationPath;
                        objParamDetails.Output = string.Concat(UtilityResource.MapReduceMapperOutputPath,
                    DateTimeOffset.Now.DateTime.ToString("yyyyMMdd_HH-mm-ss-fff"));
                        var json = JsonConvert.SerializeObject(objParamDetails);
                        HttpContent httpContent = new StringContent(json);
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        client.BaseAddress = new Uri(ApplicationConstants.HadoopServices);
                        HttpResponseMessage response = client.PostAsync($"InitialLearning/DebtSamplingJobInfra", httpContent).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            result = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                }
                mlJobDetails.MLJobId = Convert.ToString(result);
                mlJobDetails.DataPath = dataLaketargetPath;
                mlJobDetails.FileName = fileName;

            }
            catch (Exception ex)
            {
                ErrorLOG(ex.Message, "SumbmitSamplingMapReduceJob JOB ID CREATION", Convert.ToInt32(ProjectID), UserID);
                throw ex;
            }
            return mlJobDetails;
        }

        /// <summary>
        /// SubmitNoiseEliminationMapReduceJob
        /// </summary>
        /// <param name="bUName"></param>
        /// <param name="accountName"></param>
        /// <param name="projectName"></param>
        /// <param name="esaProjectID"></param>
        /// <param name="initialLearningId"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="UserID"></param>
        /// <param name="ProjectID"></param>
        /// <returns>GetMLJobDetails</returns>
        public static GetMLJobDetails SubmitNoiseEliminationMapReduceJob(string bUName, string accountName,
            string projectName, string esaProjectID, string initialLearningId, string sourceFilePath,
            string UserID, int ProjectID, int SupportTypeID = 1)
        {
            ErrorLOGInfra(". in SubmitNoiseEliminationMapReduceJob  initialLearningId =" + initialLearningId,
                "SubmitNoiseEliminationMapReduceJob", 10337);
            GetMLJobDetails mlJobDetails = new GetMLJobDetails();
            try
            {

                string dataLaketargetPath = string.Concat(new AppSettings().AppsSttingsKeyValues["DataLakePathRoot"],
                    esaProjectID, "/", initialLearningId);
                string fileName = Path.GetFileName(sourceFilePath);
                string inputDestinationPath = CreateMapperInputFile(sourceFilePath, dataLaketargetPath,
                    "Inputfile", SupportTypeID);
                string result = string.Empty;
                ErrorLOGInfra("2 . in in side try  initialLearningId =" + initialLearningId,
                "SubmitNoiseEliminationMapReduceJob", 10337);
                UploadFileToDL(sourceFilePath, dataLaketargetPath, SupportTypeID);
                ErrorLOGInfra("13 . after upload initialLearningId =" + initialLearningId,
                "SubmitNoiseEliminationMapReduceJob", 10337);
                if (SupportTypeID == 2)
                {
                    // using (var client = new HttpClient())
                    var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
                    var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                    using (var client = httpClientFactory.CreateClient())
                    {
                        ServiceParamDetails objParamDetails = new ServiceParamDetails();
                        objParamDetails.Input = inputDestinationPath;
                        objParamDetails.Output = string.Concat(UtilityResource.MapReduceMapperOutputPath,
                                 DateTimeOffset.Now.DateTime.ToString("yyyyMMdd_HH-mm-ss-fff"));
                        var json = JsonConvert.SerializeObject(objParamDetails);
                        HttpContent httpContent = new StringContent(json);
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        client.BaseAddress = new Uri(ApplicationConstants.HadoopServices);
                        HttpResponseMessage response = client.PostAsync($"InitialLearning/DebtNoiseEliminationJobInfra", httpContent).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            result = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                }
                mlJobDetails.MLJobId = Convert.ToString(result);
                mlJobDetails.DataPath = dataLaketargetPath;
                mlJobDetails.FileName = fileName;

            }
            catch (Exception ex)
            {
                ErrorLOGInfra(ex.Message, "SubmitNoiseEliminationMapReduceJob JOB ID CREATION",
                    ProjectID, UserID);
                ErrorLOGInfra(ex.InnerException.Message, "SubmitNoiseEliminationMapReduceJob JOB ID CREATION",
                    ProjectID, UserID);
                throw ex;
            }

            return mlJobDetails;
        }


        /// <summary>
        /// If uploaded RFP file is validated upload the RFP with mapping file to datalake
        /// </summary>
        /// <param name="deptAnalysis"></param>
        /// <param name="dtColumnMapping"></param>
        public static GetMLJobDetails SumbmitClassificationMapReduceJob(string esaProjectID,
            string initialLearningId, string sourceFilePath, string DescPath,
            string ResPath, string UserID, int ProjectID, int SupportTypeID)
        {

            GetMLJobDetails mlJobDetails = new GetMLJobDetails();
            try
            {


                string dataLaketargetPath = string.Concat(new AppSettings().AppsSttingsKeyValues["DataLakePathRoot"],
                    esaProjectID, "/", initialLearningId);
                string fileName = Path.GetFileName(sourceFilePath);
                string inputDestinationPath = CreateMapperInputFileForSampling(sourceFilePath, DescPath,
                    ResPath, dataLaketargetPath, SupportTypeID);
                string result = string.Empty;
                UploadFileToDL(sourceFilePath, dataLaketargetPath, SupportTypeID);
                if (SupportTypeID == 2)
                {
                    string fileNamedesc = Path.GetFileName(DescPath);
                    UploadFileToDL(DescPath, dataLaketargetPath, SupportTypeID);

                    if (!string.IsNullOrEmpty(ResPath))
                    {
                        string fileNameres = Path.GetFileName(ResPath);
                        UploadFileToDL(ResPath, dataLaketargetPath, SupportTypeID);
                    }

                    //using (var client = new HttpClient())
                    var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
                    var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                    using (var client = httpClientFactory.CreateClient())
                    {
                        ServiceParamDetails objParamDetails = new ServiceParamDetails();
                        objParamDetails.Input = inputDestinationPath;
                        objParamDetails.Output = string.Concat(UtilityResource.MapReduceMapperOutputPath,
                           DateTimeOffset.Now.DateTime.ToString("yyyyMMdd_HH-mm-ss-fff"));
                        var json = JsonConvert.SerializeObject(objParamDetails);
                        HttpContent httpContent = new StringContent(json);
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        client.BaseAddress = new Uri(ApplicationConstants.HadoopServices);
                        HttpResponseMessage response = client.PostAsync($"InitialLearning/DebtRuleExtractionJobInfra", httpContent).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            result = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                }
                mlJobDetails.MLJobId = Convert.ToString(result);
                mlJobDetails.DataPath = dataLaketargetPath;
                mlJobDetails.FileName = fileName;

            }
            catch (Exception ex)
            {
                ErrorLOG(ex.Message, "SumbmitClassificationMapReduceJob JOB ID CREATION", ProjectID, UserID);
                throw ex;
            }
            return mlJobDetails;
        }

        /// <summary>
        /// ErrorLOG
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <param name="step"></param>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        public static void ErrorLOG(string ErrorMessage, string step, int ProjectID)
        {
            ErrorLOG(ErrorMessage, step, ProjectID, null);
        }
        public static void ErrorLOG(string ErrorMessage, string step, int ProjectID, string UserID)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[4];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@step", step);
                prms[2] = new SqlParameter("@ErrorMessage", ErrorMessage);
                prms[3] = new SqlParameter("@UserID", UserID);
                (new DBHelper()).ExecuteNonQuery("ML_InsertErrorLog", prms, connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// ErrorLOGInfra
        /// </summary>
        /// <param name="ErrorMessage">ErrorMessage</param>
        /// <param name="step">step</param>
        /// <param name="ProjectID">ProjectID</param>
        /// <param name="UserID">UserID</param>
        public static void ErrorLOGInfra(string ErrorMessage, string step, int ProjectID)
        {
            ErrorLOGInfra(ErrorMessage, step, ProjectID, null);
        }
        public static void ErrorLOGInfra(string ErrorMessage, string step, int ProjectID, string UserID)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@Step", step);
                prms[1] = new SqlParameter("@Content", ErrorMessage);
                (new DBHelper()).ExecuteNonQuery("InsertErrorLogInfra", prms, connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
