using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AVMCoE.Framework

{
    public class DBHelper
    {
        static bool ErrorLogEnabled = Convert.ToBoolean(ConfigurationManager.ConnectionStrings["ErrorLogEnabled"]);

        public SqlConnectionStringBuilder GetSqlConnectionStringBuilder(string connectionString)
        {
            SqlConnectionStringBuilder connectionBuilder = new SqlConnectionStringBuilder(connectionString);
            connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
           // connectionBuilder.IntegratedSecurity = true;

            return connectionBuilder;
        }

        /// <summary>
        /// Method used to get result from script
        /// </summary>
        /// <param name="sp">Procedure</param>
        /// <param name="parameter">Parametre</param>
        /// <returns></returns>
        public virtual DataTable GetTableFromSP(string sp, SqlParameter[] parameter, string connectionString)
        {
            try
            {
              
                    SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                    //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                    using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout })
                    {
                        connection.Open();
                        command.Parameters.AddRange(parameter);

                        TypedDataSet dataSet = new TypedDataSet();
                        (new SqlDataAdapter(command)).Fill(dataSet);
                        command.Parameters.Clear();

                        if (dataSet.Tables.Count > 0)
                        {
                            return dataSet.Tables[0];
                        }
                        else
                        {
                            return null;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;
            }

        }
        /// <summary>
        /// This is to Get Table From SP
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="parametersCollection"></param>
        /// <returns></returns>
        public DataTable GetTableFromSP(string sp, Dictionary<string, object> parametersCollection, string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout })
                    {
                        connection.Open();
                        foreach (KeyValuePair<string, object> parameter in parametersCollection)
                            command.Parameters.AddWithValue(parameter.Key, parameter.Value);

                        TypedDataSet dataSet = new TypedDataSet();
                        (new SqlDataAdapter(command)).Fill(dataSet);
                        command.Parameters.Clear();

                        if (dataSet.Tables.Count > 0)
                        {
                            return dataSet.Tables[0].SanitizeDataTable();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;
            }
        }
        /// <summary>
        /// This is GetNoiseTableFromSP
        /// </summary>
        /// <typeparam name="sp"></typeparam>
        /// <param name="prms"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public TypedDataSet GetNoiseTableFromSP(string sp, SqlParameter[] prms, string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    {
                        CommandType = CommandType.
                        StoredProcedure,
                        CommandTimeout = connection.ConnectionTimeout
                    })
                    {
                        connection.Open();
                        command.Parameters.AddRange(prms);

                        TypedDataSet dataSet = new TypedDataSet();
                        (new SqlDataAdapter(command)).Fill(dataSet);
                        command.Parameters.Clear();
                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;
            }
        }
        /// <summary>
        /// To Get DataSet
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="prms"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sp, SqlParameter[] prms, string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    {

                        CommandType = CommandType.
                        StoredProcedure,
                        CommandTimeout = connection.ConnectionTimeout
                    })
                    {
                        connection.Open();
                        command.Parameters.AddRange(prms);

                        DataSet dataSet = new DataSet();
                        (new SqlDataAdapter(command)).Fill(dataSet);
                        command.Parameters.Clear();
                        if (dataSet.Tables.Count > 0)
                        {
                            return dataSet;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;               
            }
        }

        /// <summary>
        /// This is CreatePara Common Function
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SqlParameter[] CreatePara<TSource>(TSource data)
        {
            PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            SqlParameter[] Cusprms = new SqlParameter[props.Length];
            try
            {
                StringBuilder paramName = new StringBuilder();
                StringBuilder strAtSymbol = new StringBuilder("@");
                StringBuilder typeName = new StringBuilder();
                StringBuilder strAVL = new StringBuilder("AVL.");
                StringBuilder strTVP = new StringBuilder("TVP_");

                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(data, null);
                    paramName.Clear();
                    paramName.Append(strAtSymbol).Append(props[i].Name);
                    if (values[i].GetType().Name.ToUpper().Trim() == "DATATABLE")
                    {
                        typeName.Clear();
                        typeName.Append(strAVL).Append(strTVP).Append(props[i].Name);
                        Cusprms[i] = new SqlParameter(Convert.ToString(paramName), values[i]);
                        Cusprms[i].SqlDbType = SqlDbType.Structured;
                        Cusprms[i].TypeName = Convert.ToString(typeName);
                    }
                    else
                    {
                        Cusprms[i] = new SqlParameter(Convert.ToString(paramName), values[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;
            }
            return Cusprms;
        }
        public SqlParameter[] CreateSinglePara(string ParaNama, string ParaValue)
        {
            SqlParameter[] Cusprms = new SqlParameter[1];

            Cusprms[0] = new SqlParameter("@" + ParaNama, ParaValue);

            return Cusprms;
        }
        public string ExecuteScalarFunction(string CommandText, string connectionString)
        {
            string Result = "";
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(CommandText, connection)
                    { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout })
                    {
                        connection.Open();

                        DataTable dt = new DataTable();
                        (new SqlDataAdapter(command)).Fill(dt);
                        command.Parameters.Clear();

                        Result= dt.Rows[0][0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;
            }

            return Result;

        }
        /// This is to Get Table From SP
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public DataTable GetTableFromSP(string sp, string connectionString, bool isSanitize = true)
        {
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout })
                    {
                        connection.Open();

                        TypedDataSet dataSet = new TypedDataSet();
                        (new SqlDataAdapter(command)).Fill(dataSet);
                        command.Parameters.Clear();

                        if (dataSet.Tables.Count > 0)
                        {
                            return isSanitize ? dataSet.Tables[0].SanitizeDataTable() : dataSet.Tables[0];
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;
            }
        }
        /// <summary>
        /// This is to Get Dataset From SP
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="prms"></param>
        /// <param name="IsNotSanitize"></param>
        /// <returns></returns>
        public TypedDataSet GetDatasetFromSP(string sp, SqlParameter[] prms, string connectionString, bool IsNotSanitize = false)
        {
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout })
                    {
                        connection.Open();

                        command.Parameters.AddRange(IsNotSanitize ? prms : prms.SanitizeSqlParameters());

                        TypedDataSet dataSet = new TypedDataSet();
                        (new SqlDataAdapter(command)).Fill(dataSet);
                        command.Parameters.Clear();

                        return (IsNotSanitize ? dataSet : dataSet.SanitizeDataSet());
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;
            }
        }

        /// <summary>
        /// This is to Get Table From SP
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="prms"></param>
        /// <returns></returns>
        public DataTable GetTableFromSP(string sp, SqlParameter[] prms, string connectionString, bool isSanitize = true)
        {
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout })
                    {
                        connection.Open();
                        command.Parameters.AddRange(isSanitize ? prms.SanitizeSqlParameters() : prms);

                        TypedDataSet dataSet = new TypedDataSet();
                        (new SqlDataAdapter(command)).Fill(dataSet);
                        command.Parameters.Clear();

                        if (dataSet.Tables.Count > 0)
                        {
                            return isSanitize ? dataSet.Tables[0].SanitizeDataTable() : dataSet.Tables[0];
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;
            }
        }
        /// <summary>
        /// used to call the Gatway server SP
        /// </summary>
        /// <param name="sp">Procdure</param>
        /// <returns>dataset values</returns>
        public DataTable GetTableFromSP(string sp, string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = connection.ConnectionTimeout
                    })
                    {
                        connection.Open();

                        DataSet dataSet = new DataSet();
                        (new SqlDataAdapter(command)).Fill(dataSet);
                        command.Parameters.Clear();

                        if (dataSet.Tables.Count > 0)
                        {
                            return dataSet.Tables[0];
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;
            }

        }

        /// <summary>
        /// Used to call Development DB SP 
        /// </summary>
        /// <param name="sp">Procedure</param>
        /// <returns>Data Table values</returns>
        public DataTable GetDataTableFromSP(string sp, string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout })
                    {
                        connection.Open();

                        DataSet dataSet = new DataSet();
                        (new SqlDataAdapter(command)).Fill(dataSet);
                        command.Parameters.Clear();

                        if (dataSet.Tables.Count > 0)
                        {
                            return dataSet.Tables[0];
                        }
                        else
                        {
                            return null;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);

                }
                return null;
            }
        }


        /// <summary>
        /// To Call Dev DB SP
        /// </summary>
        /// <param name="sp">Procedure</param>
        /// <param name="parameter">Parameter</param>
        public void ExecuteNonQuery(string sp, SqlParameter[] parameter, string connectionString)
        {

            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout })
                    {
                        connection.Open();

                        command.Parameters.AddRange(parameter);

                        command.ExecuteNonQuery();
                    }
                }


            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
            }
        }

        /// <summary>
        /// To Call Dev DB SP
        /// </summary>
        /// <param name="sp">procedure</param>
        /// <param name="parameter">parameter</param>
        public void ExecuteNonQuery(string sp, SqlParameter parameter, string connectionString)
        {

            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout })
                    {
                        connection.Open();
                        parameter.SqlDbType = SqlDbType.Structured;
                        command.Parameters.Add(parameter);
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
            }
        }

        /// <summary>
        /// To call dev db
        /// </summary>
        /// <param name="sp">procedure</param>
        /// <param name="parameter">parameter</param>
        /// <param name="parameters">parameters</param>
        public void ExecuteNonQuery(string sp, SqlParameter parameter, SqlParameter[] parameters, string connectionString)
        {

            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout })
                    {
                        connection.Open();
                        parameter.SqlDbType = SqlDbType.Structured;
                        command.Parameters.Add(parameter);
                        command.Parameters.AddRange(parameters);
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
            }
        }

        public void ExecuteNonQueryNoParams(string sp, string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout })
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }                
            }           
        }
        /// <summary>
        /// To Call Dev DB's SP
        /// </summary>
        /// <param name="sp">procdure</param>
        /// <param name="parameter">parameter</param>
        /// <returns></returns>
        public DataTable GetTableRow(string sp, SqlParameter[] parameter, string connectionString)
        {

            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout })
                    {
                        command.Parameters.AddRange(parameter);
                        connection.Open();

                        DataSet dataSet = new DataSet();
                        (new SqlDataAdapter(command)).Fill(dataSet);
                        command.Parameters.Clear();
                        if (dataSet.Tables.Count > 0)
                        {
                            return dataSet.Tables[0];
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;
            }
        }
        /// <summary>
        /// This is to Get Dataset From SP
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public TypedDataSet GetDatasetFromSP(string sp, string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout })
                    {
                        connection.Open();

                        TypedDataSet dataSet = new TypedDataSet();
                        new SqlDataAdapter(command).Fill(dataSet);
                        command.Parameters.Clear();

                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;
            }

        }
        /// <summary>
        /// This is to convert DataTable FromList 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTableFromList<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            try
            {
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition()
                        == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                    dataTable.Columns.Add(prop.Name, type);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
            }
            return dataTable;
        }
        /// <summary>
        /// To Call Dev DB'S SP
        /// </summary>
        /// <param name="sp">procedure</param>
        /// <param name="parameter">parameter</param>
        /// <returns></returns>
        public TypedDataSet GetDatasetFromSP(string sp, SqlParameter[] parameter, string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = connection.ConnectionTimeout
                    })
                    {
                        connection.Open();
                        command.Parameters.AddRange(parameter);
                        TypedDataSet dataSet = new TypedDataSet();
                        (new SqlDataAdapter(command)).Fill(dataSet);
                        command.Parameters.Clear();
                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;
            }
        }

        /// <summary>
        /// To call Dev DB'S SP
        /// </summary>
        /// <param name="sp">procdure</param>
        /// <param name="parameter">parameter</param>
        /// <returns></returns>
        public int ExecuteNonQueryReturn(string sp, SqlParameter[] parameter, string connectionString)
        {

            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = connection.ConnectionTimeout
                    })
                    {
                        connection.Open();
                        command.Parameters.AddRange(parameter);
                        int result = command.ExecuteNonQuery();
                        return result;
                    }
                }
            }

            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return 0;
            }
        }

        /// <summary>
        /// To Call dev db's sp
        /// </summary>
        /// <param name="sp">procedure</param>
        public void ExecuteNonQuery(string sp, string connectionString)
        {

            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = connection.ConnectionTimeout
                    })
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
            }
        }

        /// <summary>
        /// This is To DataTable 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                for (int i = 0; i < Props.Length; i++)
                {
                    dataTable.Rows.Add(Props[i].GetValue(item, null));
                }
            }
            return dataTable;
        }

        public void ErrorLOG(string ErrorMessage, string step, int ProjectID, string connectionString)
        {
            SqlParameter[] prms = new SqlParameter[3];
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);

                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@step", step);
                prms[2] = new SqlParameter("@ErrorMessage", ErrorMessage);
                (new DBHelper()).ExecuteNonQuery("ML_InsertErrorLog", prms, connectionBuilder.ConnectionString);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }


        /// <summary>
        /// This Method Is Used To InsertDatatable
        /// </summary>
        /// <param name="dtBulk"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string InsertDatatable(DataTable dtBulk, string tableName, string connectionString)
        {
            string strResult = string.Empty;
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionBuilder.ConnectionString);
                if (dtBulk != null && dtBulk.Rows.Count > 0)
                {
                    for (int y = 0; y < dtBulk.Columns.Count; y++)
                    {
                        bulkCopy.ColumnMappings.Add(dtBulk.Columns[y].ColumnName, dtBulk.Columns[y].ColumnName);
                    }
                    try
                    {
                        strResult = string.Empty;
                        using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                        {
                            connection.Open();
                            bulkCopy.DestinationTableName = tableName;
                            bulkCopy.WriteToServer(dtBulk);
                        }
                    }
                    catch (InvalidOperationException strEx)
                    {
                        string ex = string.Empty;
                        ex = strEx.InnerException.Message;

                        if (strEx.InnerException.Message == "String or binary data would be truncated.")
                        {
                            strResult = "Data length is exceeding the maximum limit." +
                                " Please refer the Help file for data limits";
                        }
                        else
                        {
                            strResult = "Data Type mismatch. Please refer the Help file for data DataType";
                        }
                        Logger.Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Received an invalid column length from the bcp client for colid 12.")
                {
                    strResult = "Data length is exceeding the maximum limit. Please refer " +
                        "the Help file for data limits";
                    //new ExceptionUtility().LogExceptionMessage(ex);                    
                }
                else
                {
                    strResult = "Error: " + ex.Message + " Stack Trace: " + ex.StackTrace;
                }
                Logger.Error(ex);
            }
            return strResult;
        }

        public DataTable ToDataTableSingleRow<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() ==
                    typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
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
        public DataSet GetDataSetFromReport(string sp, SqlParameter[] prms, string connectionString)
        {           
            try
            {
                SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
                //connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sp, connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = connection.ConnectionTimeout
                    })
                    {
                        connection.Open();
                        command.Parameters.AddRange(prms);
                        TypedDataSet dataSet = new TypedDataSet();
                        (new SqlDataAdapter(command)).Fill(dataSet);
                        command.Parameters.Clear();
                        if (dataSet.Tables.Count > 0)
                        {
                            return dataSet;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return null;
            }          
        }
    }


}