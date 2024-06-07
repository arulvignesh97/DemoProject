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
using System.IO;
using System.Text;
using System.Web;

namespace CTS.Applens.Framework
{
    public static class Canonicalization
    {
        public static string Canonicalize(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            path = HttpUtility.UrlDecode(path);

            // Check for invalid characters
            if (path.IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
            {
                throw new FileNotFoundException(string.Concat("FileName not valid: ", path));
            }

            return path;
        }
    }
}
