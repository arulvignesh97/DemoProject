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
using System.ComponentModel.DataAnnotations;

namespace CTS.Applens.Framework
{

    /// <summary>
    /// To filter the special charecters in the Input Parameter!
    /// </summary>
    public sealed class SanitizeString
    {
        /// <summary>
        /// Constuctor to Sanitize the Input
        /// </summary>
        /// <param name="s"></param>
        public SanitizeString(dynamic input)
        {
            string inputValue = Convert.ToString(input);
            Value = inputValue.Replace("<", "").Replace(">", "");
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
        public static implicit operator SanitizeString(string s)
        {
            return new SanitizeString(s);
        }
    }
}
