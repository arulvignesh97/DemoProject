/***************************************************************************
 * COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET
 * Copyright [2018] – [2021] Cognizant. All rights reserved.
 * NOTICE: This unpublished material is proprietary to Cognizant and
 * its suppliers, if any. The methods, techniques and technical
 * concepts herein are considered Cognizant confidential and/or trade secret information.
 * This material may be covered by U.S. and/or foreign patents or patent applications.
 * Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
 ***************************************************************************/

using System.Text.RegularExpressions;
namespace CTS.Applens.Framework
{
    public static class Validations
    {
        public static bool ValidFilePath(string filePath, out string validpath)
        {
            validpath = string.Empty;
            Regex rgx = new Regex("(\\\\?([^\\/]*[\\/])*)([^\\/]+)");
            if (filePath != null && rgx.IsMatch(filePath))
            {
                validpath = filePath.Replace("..", "").Replace(">", "").Replace("<", "");
                return true;
            }
            return false;
        }
    }
}

