using System;
using System.ComponentModel.DataAnnotations;

namespace CTS.Applens.WorkProfiler.Common
{
    /// <summary>
    /// Created By Dinesh Babu B (536555)
    /// To filter the special charecters in the Input Parameter!
    /// </summary>
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
                Value = str.Replace("<", "").Replace(">", "");
            }
            catch (Exception)
            {
                //CCAP FIX
            }
        }

        private string values;

        /// <summary>
        /// gets or sets the value
        /// </summary>
        [MaxLength(int.MaxValue)]
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