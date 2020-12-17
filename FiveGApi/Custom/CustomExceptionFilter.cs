using DnsClient;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace FiveGApi.Custom
{
    public class CustomExceptionFilter: ExceptionFilterAttribute
    {
        private static ILog _logger = null;
        private static log4net.ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = LogManager.GetLogger(typeof(Logging));
                    log4net.Config.XmlConfigurator.Configure();
                }
                return _logger;
            }
        }
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string exceptionMessage = string.Empty;
            if (actionExecutedContext.Exception.InnerException == null)
            {
                exceptionMessage = actionExecutedContext.Exception.Message;
                
            }
            else
            {
                exceptionMessage = actionExecutedContext.Exception.InnerException.Message;
            }
            string date = DateTime.Now.ToString();
            
            
            //We can log this exception message
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An unhandled exception was thrown by service."),
                ReasonPhrase = "Internal Server Error.Please Contact your Administrator."
            };
            Logger.Error("Exception:", actionExecutedContext.Exception);
            actionExecutedContext.Response = response;
        }
    }
}