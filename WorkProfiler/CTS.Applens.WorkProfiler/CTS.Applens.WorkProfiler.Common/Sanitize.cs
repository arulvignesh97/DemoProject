using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CTS.Applens.WorkProfiler.Common
{
    /// <summary>
    /// Created By Dinesh Babu B (536555)
    /// To filter the special charecters in the Input Parameter!
    /// </summary>
    public static class Sanitize
    {
        /// <summary>
        ///  Method to Sanitize the SqlParameters
        /// </summary>
        /// <param name="prms"></param>
        /// <returns></returns>
        public static SqlParameter[] SanitizeSqlParameters(this SqlParameter[] prms)
        {
            try
            {
                SqlParameter[] objPrms = new SqlParameter[prms.Length];
                for (int i = 0; i < prms.Length; i++)
                {
                    objPrms[i] = SanitizeSqlParameterObject(prms[i]);
                }
                return objPrms;
            }
            catch (Exception)
            { 
                return prms;
            }
        }
        /// <summary>
        /// Method to Sanitize the SqlParameter Object
        /// </summary>
        /// <param name="sqlParm"></param>
        /// <returns></returns>
        public static SqlParameter SanitizeSqlParameterObject(this SqlParameter sqlParm)
        {

            if (sqlParm.Value == null)
            {
                return new SqlParameter(sqlParm.ParameterName, sqlParm.Value);
            }
            Type tModelType = sqlParm.Value.GetType();
            bool x1 = tModelType.Namespace == null || !tModelType.Namespace.StartsWith("System");
            if ((tModelType.IsValueType || tModelType.Name == "String")) // String
            {
                return new SqlParameter(sqlParm.ParameterName, Cast(new SanitizeStringInput(sqlParm.Value).
                    Value, sqlParm.Value.GetType()));
            }
            
            else if (!tModelType.IsValueType && !tModelType.IsPrimitive && x1 && (sqlParm.SqlDbType != SqlDbType.Structured)) // Reference Type Object
            {
                return new SqlParameter(sqlParm.ParameterName, SanitizeObject(sqlParm.Value));
            }
            else if (tModelType.IsGenericType) // Reference Type List
            {
                if (tModelType.GetGenericTypeDefinition().Name == "List`1") // Check for Type List
                {
                    return new SqlParameter(sqlParm.ParameterName, SanitizeList((dynamic)sqlParm.Value));
                }
            }
            else if (tModelType.Name == "DataTable") // DataTable
            {
                var sqlParmObj = new SqlParameter(sqlParm.ParameterName,
                    SanitizeDataTable((DataTable)(sqlParm.Value)));
                if (!string.IsNullOrEmpty(sqlParm.TypeName))
                {
                    sqlParmObj.TypeName = sqlParm.TypeName;
                }
                return sqlParmObj;
            }
            else
            {
                //mandatory else
            }
            return sqlParm;
        }
        /// <summary>
        /// This Method is used to Convert the object List type
        /// </summary>
        /// <param name="arrList"></param>
        /// <returns></returns>
        public static ArrayList SanitizeArrayList(this ArrayList arrList)
        {
            try
            {
                for (int i = 0; i < arrList.Count; i++)
                {
                    if (arrList[i] != null)
                    {
                        SanitizeStringInput sInput = Convert.ToString(arrList[i], CultureInfo.CurrentCulture);
                        arrList[i] = Cast(sInput.Value, arrList[i].GetType());
                    }
                }
            }
            catch (Exception)
            {
                return arrList;
            }
            return arrList;
        }
        /// <summary>
        /// This Method is used to Sanitize the object type
        /// </summary>
        /// <param name="objArray"></param>
        /// <returns></returns>
        public static dynamic SanitizeObject(this object objArray)
        {
            try
            {
                return new SanitizeObject<object>(objArray).Value;
            }
            catch (Exception)
            {
                return objArray;
            }
        }
        /// <summary>
        /// This Method is used to Sanitize the object List type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TList"></param>
        /// <returns></returns>
        public static dynamic SanitizeList<T>(this List<T> TList) where T : new()
        {
            try
            {
                return new SanitizedList<T>(TList).Value;
            }
            catch (Exception)
            {
                return TList;
            }
        }
        /// <summary>
        /// This Method is used to Convert the object type
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="castTo"></param>
        /// <returns></returns>
        private static dynamic Cast(dynamic obj, Type castTo)
        {
            if (obj == "")
            {
                return "";
            }
            return Convert.ChangeType(obj, castTo, CultureInfo.CurrentCulture);
        }
        /// <summary>
        /// This Method is used to Sanitize the DataTable type
        /// </summary>
        /// <param name="objDt"></param>
        /// <returns></returns>
        public static DataTable SanitizeDataTable(this DataTable objDt)
        {
            try
            {
                for (int i = 0; i < objDt.Rows.Count; i++)
                {
                    for (int j = 0; j < objDt.Columns.Count; j++)
                    {
                        if (objDt.Rows[i][j] == null || objDt.Rows[i][j].ToString() == "" ||
                            objDt.Columns[j].ColumnName.ToLower(CultureInfo.CurrentCulture) == "ticketdescription")
                        {
                            continue;
                        }
                        SanitizeStringInput sInput = Convert.ToString(objDt.Rows[i][j], CultureInfo.CurrentCulture);
                        objDt.Rows[i][j] = Cast(sInput.Value, objDt.Rows[i][j].GetType());
                    }
                }
                return objDt;
            }
            catch (Exception)
            {
                return objDt;
            }
        }
        /// <summary>
        /// This Method is used to Sanitize the DataSet type
        /// </summary>
        /// <param name="objDs"></param>
        /// <returns></returns>
        public static DataSet SanitizeDataSet(this DataSet objDs)
        {
            try
            {
                for (int i = 0; i < objDs.Tables.Count; i++)
                {
                    for (int j = 0; j < objDs.Tables[i].Rows.Count; j++)
                    {
                        for (int k = 0; k < objDs.Tables[i].Columns.Count; k++)
                        {
                            if (objDs.Tables[i].Rows[j][k] == DBNull.Value || objDs.Tables[i].Columns[k].
                                ColumnName.ToLower(CultureInfo.CurrentCulture) == "ticketdescription")
                            {
                                continue;
                            }
                            SanitizeStringInput sInput = Convert.ToString(objDs.Tables[i].Rows[j][k], CultureInfo.CurrentCulture);
                            objDs.Tables[i].Rows[j][k] = string.IsNullOrEmpty(sInput.Value) ? sInput.Value :
                                Cast(sInput.Value, objDs.Tables[i].Rows[j][k].GetType());
                        }
                    }
                }
                return objDs;
            }
            catch (Exception)
            {
                return objDs;
            }
        }
    }
}