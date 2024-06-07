using System;
using System.Collections.Generic;
using System.Text;

namespace CTS.Applens.WorkProfiler.DAL.BaseDetails
{
    public sealed class SanitizeStringInput
    {
        /// <summary>
        /// Constuctor to Sanitize the Input
        /// </summary>
        /// <param name="s"></param>
        public SanitizeStringInput(dynamic s)
        {
            string str = Convert.ToString(s);
            try
            {
                Value = CropStripTagsCharArray(str);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private string values;

        /// <summary>
        /// gets or sets the value
        /// </summary>
        public string Value
        {
            get { return values; }
            set { values = value; }
        }
        /// <summary>
        /// Implicit Operator to Sanitize the Input
        /// </summary>
        /// <param name="s"></param>
        static public implicit operator SanitizeStringInput(string s)
        {
            return new SanitizeStringInput(s);
        }
        /// <summary>
        /// This Method is to Crop Strip Tags into CharArray
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string CropStripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
