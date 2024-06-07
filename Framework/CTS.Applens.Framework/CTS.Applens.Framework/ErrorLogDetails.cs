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
    public class ErrorLogDetails
    {
       public string LogSeverity { get; set; }
        public string LogLevel { get; set; }
        public string HostName { get; set; }
        public string AssociateId { get; set; }
        public string CreatedDate { get; set; }
        public string ProjectId { get; set; }
        public string Technology { get; set; }
        public string ModuleName { get; set; }
        public string FeatureName { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public int ProcessId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string AdditionalField_1 { get; set; }
        public string AdditionalField_2 { get; set; }
    }



    public enum LogSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }
    public enum LogLevels
    {
        Info,
        Audit,
        Error,
        Warn,
        Debug
    }
    public enum Technology
    {
         Angular,
         AngularJS,
         CSharp,
         DotNet,
         DotNetCore,
         Java,
         JavaScript,
         JQuery,
         JSP,
         MongoDB,
         MVCCSharp,
         MVCCSHTML,
         MySQL,
         PHP,
         Python,
         R_Script,
         SQL
    }
    public static class ModuleName
    {
        public const string BidModule = "Bid Module";
        public const string DealMetadataCollection = "Deal Metadata Collection";
    }
    public static class FeatureName
    {
        public const string CMI = "CMI";
    }
}
