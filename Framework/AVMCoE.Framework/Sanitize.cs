using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AVMCoE.Framework
{
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
            catch (Exception ex)
            {
                Logger.Error(ex);
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
                return sqlParm;
            }
            if (sqlParm.Direction == ParameterDirection.Output)
            {
                return sqlParm;
            }
            Type tModelType = sqlParm.Value.GetType();
            if ((tModelType.IsValueType || tModelType.Name == "String"))
            {
                return new SqlParameter(sqlParm.ParameterName, Cast(new SanitizeStringInput(sqlParm.Value).Value,
                    sqlParm.Value.GetType()));
            }
            else if (!tModelType.IsValueType && !tModelType.IsPrimitive &&
                (tModelType.Namespace == null || !tModelType.Namespace.StartsWith("System")) &&
                (sqlParm.SqlDbType != SqlDbType.Structured))
            {
                return new SqlParameter(sqlParm.ParameterName, SanitizeObject(sqlParm.Value));
            }
            else if (tModelType.IsGenericType)
            {
                if (tModelType.GetGenericTypeDefinition().Name == "List`1")
                {
                    return new SqlParameter(sqlParm.ParameterName, SanitizeList((dynamic)sqlParm.Value));
                }
            }
            else if (tModelType.Name == "DataTable")
            {
                var sqlParmObj = new SqlParameter(sqlParm.ParameterName, SanitizeDataTable((DataTable)(sqlParm.Value)));
                if (!string.IsNullOrEmpty(sqlParm.TypeName))
                {
                    sqlParmObj.TypeName = sqlParm.TypeName;
                }
                return sqlParmObj;
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
                        SanitizeStringInput sInput = Convert.ToString(arrList[i]);
                        arrList[i] = Cast(sInput.Value, arrList[i].GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return arrList;
            }
            return arrList;
        }
        /// <summary>
        /// SanitizeStringArray
        /// </summary>
        /// <param name="strArr"></param>
        /// <returns></returns>
        public static string[] SanitizeStringArray(this string[] strArr)
        {
            try
            {
                for (int i = 0; i < strArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(strArr[i]))
                    {
                        SanitizeStringInput sInput = Convert.ToString(strArr[i]);
                        strArr[i] = Cast(sInput.Value, strArr[i].GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return strArr;
            }
            return strArr;
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
            catch (Exception ex)
            {
                Logger.Error(ex);
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
            catch (Exception ex)
            {
                Logger.Error(ex);
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
            return Convert.ChangeType(obj, castTo);
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
                        if (objDt.Rows[i][j] == null || objDt.Rows[i][j].ToString() == ""
                            || objDt.Columns[j].ColumnName.ToLower() == "ticketdescription") continue;
                        SanitizeStringInput sInput = Convert.ToString(objDt.Rows[i][j]);
                        objDt.Rows[i][j] = Cast(sInput.Value, objDt.Rows[i][j].GetType());
                    }
                }
                return objDt;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return objDt;
            }
        }
        /// <summary>
        /// This Method is used to Sanitize the DataSet type
        /// </summary>
        /// <param name="objDs"></param>
        /// <returns></returns>
        public static TypedDataSet SanitizeDataSet(this TypedDataSet objDs)
        {
            try
            {
                for (int i = 0; i < objDs.Tables.Count; i++)
                {
                    for (int j = 0; j < objDs.Tables[i].Rows.Count; j++)
                    {
                        for (int k = 0; k < objDs.Tables[i].Columns.Count; k++)
                        {
                            if (objDs.Tables[i].Rows[j][k] == DBNull.Value ||
                                objDs.Tables[i].Columns[k].ColumnName.ToLower() == "ticketdescription")
                                continue;
                            SanitizeStringInput sInput = Convert.ToString(objDs.Tables[i].Rows[j][k]);
                            objDs.Tables[i].Rows[j][k] = string.IsNullOrEmpty(sInput.Value) ? sInput.Value
                                : Cast(sInput.Value, objDs.Tables[i].Rows[j][k].GetType());
                        }
                    }
                }
                return objDs;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return objDs;
            }
        }
    }
}
