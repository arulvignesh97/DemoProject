using CTS.Applens.WorkProfiler.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Authorize("AzureADAuth")]
    public class DeleteFileAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        public override void OnResultExecuted(Microsoft.AspNetCore.Mvc.Filters.ResultExecutedContext context)
        {
            try 
            {                
                //convert the current filter context to file and get the file path
                string filePath = (context.Result as PhysicalFileResult).FileName;

                //delete the file after download
                System.IO.File.Delete(filePath);
            }
            catch(Exception ex)
            {
                new ExceptionUtility().LogExceptionMessage(ex); 
            }

        }
    }
}