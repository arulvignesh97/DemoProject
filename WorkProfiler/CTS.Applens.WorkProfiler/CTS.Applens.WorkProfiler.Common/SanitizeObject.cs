using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CTS.Applens.WorkProfiler.Common
{
    /// <summary>
    ///  Created By Dinesh Babu B (536555)
    ///  To filter the special charecters in the Input Parameter!
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public sealed class SanitizeObject<TModel> where TModel : class
    {
        /// <summary>
        /// Constuctor to Sanitize the Input
        /// </summary>
        /// <param name="tmodelObj"></param>
        public SanitizeObject(TModel tmodelObj)
        {
            try
            {
                Type tModelType = tmodelObj.GetType();

                PropertyInfo[] arrayPropertyObj = tModelType.GetProperties();

                foreach (PropertyInfo property in arrayPropertyObj)
                {
                    Type tListType = property.GetValue(tmodelObj).GetType();
                    if (tListType.IsGenericType && tListType.GetGenericTypeDefinition().Name == "List`1")
                    {
                        property.SetValue(tmodelObj, SanitizedObjectList((dynamic)property.GetValue(tmodelObj)));
                    }
                    else if (property.GetValue(tmodelObj) != null)
                    {
                        SanitizeStringInput sInput = Convert.ToString(property.GetValue(tmodelObj), CultureInfo.CurrentCulture);
                        property.SetValue(tmodelObj, Cast(sInput.Value, property.GetValue(tmodelObj).GetType()));
                    }
                    else
                    {
                        //mandatory else
                    }
                }
                Value = tmodelObj;
            }
            catch (Exception)
            {
                Value = tmodelObj;
            }
        }
        private TModel values;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public TModel Value
        {
            get { return values; }
            set { values = value; }
        }
        /// <summary>
        /// Implicit Operator to Sanitize the Input Object
        /// </summary>
        /// <param name="tmodelObj"></param>
        static public implicit operator SanitizeObject<TModel>(TModel tmodelObj)
        {
            return new SanitizeObject<TModel>(tmodelObj);
        }
        /// <summary>
        /// This Method is used to Convert the object type
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="castTo"></param>
        /// <returns></returns>
        private dynamic Cast(dynamic obj, Type castTo)
        {
            return Convert.ChangeType(obj, castTo, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// This Method is used to Sanitize the List inside Object 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TList"></param>
        /// <returns></returns>
        private dynamic SanitizedObjectList<T>(List<T> TList) where T : new()
        {
            try
            {
                return new SanitizedObjectList<T>(TList).Value;
            }
            catch (Exception)
            {
                return TList;
            }
        }
    }
}