using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeWebApi.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exceptionDetails = $"Exception occurred at: {DateTime.Now}\n" +
                                 $"Message: {context.Exception.Message}\n" +
                                 $"Stack Trace: {context.Exception.StackTrace}\n" +
                                 $"Request Path: {context.HttpContext.Request.Path}\n" +
                                 new string('-', 50) + "\n";

            try
            {
                string logPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                string fileName = Path.Combine(logPath, $"exceptions_{DateTime.Now:yyyyMMdd}.log");
                File.AppendAllText(fileName, exceptionDetails);
            }
            catch { }

            context.Result = new ObjectResult("An internal server error occurred")
            {
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }
}