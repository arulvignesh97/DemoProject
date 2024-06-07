using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace CTS.Applens.WorkProfiler.Common
{
    /// <summary>
    /// Created By Dinesh Babu B (536555)
    /// To filter the special charecters in the Input Parameter!
    /// </summary>
    /// <typeparam name="TModel"> This parameter holds values with type of TModel</typeparam>
    public sealed class SanitizedObjectList<TModel> where TModel : new()
    {
        /// <summary>
        /// Constuctor to Sanitize the Input
        /// </summary>
        /// <param name="tmodelList"> This parameter is to get TModelList</param>
        public SanitizedObjectList(List<TModel> tmodelList)
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
                            SanitizeStringInput sInput = Convert.ToString(property.GetValue(item), CultureInfo.CurrentCulture);
                            property.SetValue(x, Cast(sInput.Value, property.GetValue(item).GetType()));
                        }
                    }
                    returnList.Add(x);
                }
                Value = returnList;
            }
            catch (Exception)
            {
                Value = tmodelList;
            }
        }

        private List<TModel> values;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public List<TModel> Value
        {
            get { return values; }
            set { values = value; }
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
    }
}