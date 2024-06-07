using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.API.Utilities;
using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;

namespace CTS.Applens.WorkProfiler.API
{
    /// <summary>
    /// Action Filter to watch all the Exception
    /// </summary>
    public class BaseExceptionFilter : IExceptionFilter
    {

        private readonly IApplensLogging logger;
        //KeyCLoak Added Code
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;
        //KeyCloak End
        public BaseExceptionFilter(IApplensLogging logger, IHttpContextAccessor _httpContextAccessor, IConfiguration _configuration)
        {
            this.logger = logger;
            this.configuration = _configuration;
            this.httpContextAccessor = _httpContextAccessor;
        }

        /// <summary>
        /// Global method to log the Exception
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            try
            {
                //KeyClaok Added Code
                string associateId;
                (associateId, _) = new KeyCloak().GetUserId(httpContextAccessor, configuration);
                associateId = !string.IsNullOrEmpty(associateId) ? associateId : "UnAuthenticated";
                //KeyClaok End
                HttpStatusCode statusCode = (context.Exception as WebException != null &&
                          ((HttpWebResponse)(context.Exception as WebException).Response) != null) ?
                           ((HttpWebResponse)(context.Exception as WebException).Response).StatusCode
                           : GetHTTPErrorCode(context.Exception.GetType());

                context.ExceptionHandled = true;
                HttpResponse response = context.HttpContext.Response;
                response.StatusCode = (int)statusCode;
                response.ContentType = "application/json";
                logger.Error(context.Exception);

                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.LogSeverity = Convert.ToString(LogSeverity.Medium, CultureInfo.InvariantCulture);
                errorLogDetails.LogLevel = Convert.ToString(LogLevel.Error, CultureInfo.InvariantCulture);
                errorLogDetails.HostName = Environment.MachineName;
                errorLogDetails.AssociateId = context.HttpContext.User.Identity.IsAuthenticated ?
                                             context.HttpContext.User.Identity.Name :
                                             "UnAuthenticated";
                errorLogDetails.CreatedDate = DateTimeOffset.Now.DateTime.ToString(CultureInfo.CurrentCulture);
                errorLogDetails.Technology = Convert.ToString(Technology.DotNetCore, CultureInfo.InvariantCulture);
                errorLogDetails.ModuleName = Constants.ApplicationName;
                errorLogDetails.FeatureName = Convert.ToString(context.RouteData.Values["controller"], CultureInfo.InvariantCulture);
                errorLogDetails.ClassName = Convert.ToString(context.RouteData.Values["controller"], CultureInfo.InvariantCulture);
                errorLogDetails.MethodName = Convert.ToString(context.RouteData.Values["action"], CultureInfo.InvariantCulture);
                errorLogDetails.ProcessId = Process.GetCurrentProcess().Id;
                errorLogDetails.ErrorCode = Convert.ToString(HttpStatusCode.InternalServerError, CultureInfo.InvariantCulture);
                errorLogDetails.ErrorMessage = context.Exception.InnerException?.Message ?? context.Exception.Message;
                errorLogDetails.StackTrace = context.Exception.Message;
                errorLogDetails.AdditionalField_1 = string.Empty;
                errorLogDetails.AdditionalField_2 = string.Empty;
                LoggerAPI.LogError(context.Exception, errorLogDetails);
            }
            catch (Exception ex)
            {
                new ExceptionLogging().LogException(ex, context.HttpContext.User.Identity.IsAuthenticated ?
                                             context.HttpContext.User.Identity.Name :
                                             "UnAuthenticated");
            }

        }

              
        /// <summary>  
        /// This method will return the status code based on the exception type.  
        /// </summary>  
        /// <param name="exceptionType"></param>  
        /// <returns>HttpStatusCode</returns>  
        private static HttpStatusCode GetHTTPErrorCode(Type exceptionType)
        {
            Exceptions tryParseResult;
            if (Enum.TryParse<Exceptions>(exceptionType.Name, out tryParseResult))
            {
                switch (tryParseResult)
                {
                    case Exceptions.NullReferenceException:
                        return HttpStatusCode.LengthRequired;

                    case Exceptions.FileNotFoundException:
                        return HttpStatusCode.NotFound;

                    case Exceptions.OverflowException:
                        return HttpStatusCode.RequestedRangeNotSatisfiable;

                    case Exceptions.OutOfMemoryException:
                        return HttpStatusCode.ExpectationFailed;

                    case Exceptions.InvalidCastException:
                        return HttpStatusCode.PreconditionFailed;

                    case Exceptions.ObjectDisposedException:
                        return HttpStatusCode.Gone;

                    case Exceptions.UnauthorizedAccessException:
                        return HttpStatusCode.Unauthorized;

                    case Exceptions.NotImplementedException:
                        return HttpStatusCode.NotImplemented;

                    case Exceptions.NotSupportedException:
                        return HttpStatusCode.NotAcceptable;

                    case Exceptions.InvalidOperationException:
                        return HttpStatusCode.MethodNotAllowed;

                    case Exceptions.TimeoutException:
                        return HttpStatusCode.RequestTimeout;

                    case Exceptions.ArgumentException:
                        return HttpStatusCode.BadRequest;

                    case Exceptions.StackOverflowException:
                        return HttpStatusCode.RequestedRangeNotSatisfiable;

                    case Exceptions.FormatException:
                        return HttpStatusCode.UnsupportedMediaType;

                    case Exceptions.IOException:
                        return HttpStatusCode.NotFound;

                    case Exceptions.IndexOutOfRangeException:
                        return HttpStatusCode.ExpectationFailed;

                    default:
                        return HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}

