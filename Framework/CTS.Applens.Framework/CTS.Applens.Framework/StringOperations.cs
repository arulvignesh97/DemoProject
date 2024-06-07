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
using System.Text;

namespace CTS.Applens.Framework
{
    /// <summary>
    /// Place holder for all the string operations
    /// </summary>
    public static class StringOperations
    {
        static char Backslash = '\\';
        /// <summary>
        /// This is used to remove the domain prefrix and Backslash
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string RemoveDomain(string userID)
        {
            if (!string.IsNullOrEmpty(userID) && userID.Contains(Backslash))
            {
                int indexOfSlash = userID.IndexOf(Backslash);
                return userID.Substring(indexOfSlash + 1);
            }
            else
            {
                return userID;
            }
        }
    }
}
