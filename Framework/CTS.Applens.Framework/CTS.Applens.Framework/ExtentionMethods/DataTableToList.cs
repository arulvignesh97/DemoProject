/***************************************************************************
 * COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET
 * Copyright [2018] – [2021] Cognizant. All rights reserved.
 * NOTICE: This unpublished material is proprietary to Cognizant and
 * its suppliers, if any. The methods, techniques and technical
 * concepts herein are considered Cognizant confidential and/or trade secret information.
 * This material may be covered by U.S. and/or foreign patents or patent applications.
 * Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace CTS.Applens.Framework
{
    public static class DataTableToList
    {
        public static List<T> ToDTList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (string.Compare(pro.Name, column.ColumnName, true) == 0 && dr[column.ColumnName] != DBNull.Value) 
                    {
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else 
                    {
                        continue;
                    }
                }
            }
            return obj;
        }
    }
}
