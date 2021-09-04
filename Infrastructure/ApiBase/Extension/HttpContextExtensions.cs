using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using ApiBase.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApiBase.Extension
{
    internal static class HttpContextExtensions
    {
        internal static Task HandleExceptionAsync(this HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;

            var exceptionMessage = new StringBuilder();

            exceptionMessage.AppendLine(exception.Message);
            exceptionMessage.AppendLine(exception.StackTrace);
            while (exception.InnerException != null)
            {
                var innerException = exception.InnerException;
                exceptionMessage.AppendLine(innerException.Message);
                exceptionMessage.AppendLine(innerException.StackTrace);
                exception = exception.InnerException;

            }

            var responseText = JsonConvert.SerializeObject(exceptionMessage.ToString(), new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                Formatting = Formatting.Indented,
            });

            return context.Response.WriteAsync(responseText);
        }
    }
}