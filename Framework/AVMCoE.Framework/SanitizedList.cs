using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AVMCoE.Framework
{
    /// <summary>
    /// Created By Dinesh Babu B (536555)
    /// To filter the special charecters in the Input Parameter!
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public sealed class SanitizedList<TModel> where TModel : new()
    {
        static bool ErrorLogEnabled = Convert.ToBoolean(ConfigurationManager.ConnectionStrings["ErrorLogEnabled"]);
        /// <summary>
        /// Constuctor to Sanitize the Input
        /// </summary>
        /// <param name="tmodelList"></param>
        public SanitizedList(List<TModel> tmodelList)
        {
            try
            {
                List<TModel> returnList = new List<TModel>();
                TModel x;
                foreach (var item in tmodelList)
                {
                    x = new TModel();

                    Type tModelType = item.GetType();

                    PropertyInfo[] arrayPropertyInfos = tModelType.GetProperties();

                    foreach (PropertyInfo property in arrayPropertyInfos)
                    {
                        if (property.GetValue(item) != null)
                        {
                            Type tListType = property.GetValue(item).GetType();
                            if (tListType.IsGenericType && tListType.GetGenericTypeDefinition().Name == "List`1")
                            {
                                property.SetValue(x, SanitizedObjectList((dynamic)property.GetValue(item)));
                            }
                            else
                            {
                                SanitizeStringInput sInput = Convert.ToString(property.GetValue(item));
                                property.SetValue(x, Cast(sInput.Value, property.GetValue(item).GetType()));
                            }
                        }
                    }
                    returnList.Add(x);
                }
                Value = returnList;
            }
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                Value = tmodelList;
            }
        }

        private List<TModel> values;
        public List<TModel> Value
        {
            get { return values; }
            set { values = value; }
        }
        /// <summary>
        /// Implicit Operator to Sanitize the Input Object
        /// </summary>
        /// <param name="tmodelList"></param>
        static public implicit operator SanitizedList<TModel>(List<TModel> tmodelList)
        {
            return new SanitizedList<TModel>(tmodelList);
        }
        /// <summary>
        /// This Method is used to Convert the object type
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="castTo"></param>
        /// <returns></returns>
        private dynamic Cast(dynamic obj, Type castTo)
        {
            return Convert.ChangeType(obj, castTo);
        }
        /// <summary>
        /// This Method is used to Convert the object List type
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
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
                return TList;
            }   
        }
    }
}
