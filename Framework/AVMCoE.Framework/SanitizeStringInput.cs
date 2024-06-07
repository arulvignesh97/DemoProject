using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMCoE.Framework
{
    /// <summary>
    /// Created By Dinesh Babu B (536555)
    /// To filter the special charecters in the Input Parameter!
    /// </summary>
    public sealed class SanitizeStringInput
    {
        static bool ErrorLogEnabled = Convert.ToBoolean(ConfigurationManager.ConnectionStrings["ErrorLogEnabled"]);
        /// <summary>
        /// Constuctor to Sanitize the Input
        /// </summary>
        /// <param name="s"></param>
        public SanitizeStringInput(dynamic s)
        {
            string str = Convert.ToString(s);
            try
            {
                Value = str.Replace("<","").Replace(">", "");
            }
 
            catch (Exception ex)
            {
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(ex);
                }
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
    }
}
