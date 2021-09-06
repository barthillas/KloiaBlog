using System;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Abstraction.Exceptions;
using ApiBase.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace ApiBase.Extension
{
    internal static class HttpContextExtensions
    {
        internal static Task HandleExceptionAsync(this HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;

            var details = exception switch
            {
                BusinessException ex => new KloiaProblemDetails{
                    StackTrace = exception.StackTrace,
                    Message = exception.Message,
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Title = "Exception",
                }, 
                ValidationException ex => new KloiaProblemDetails{    
                    StackTrace = exception.StackTrace,
                    Message = exception.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Exception",
                },             
                _ => new KloiaProblemDetails
                {
                    StackTrace = exception.StackTrace,
                    Message = exception.Message,
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Unknown Exception",
                    
                }            
            };

            return ProblemDetailResponseAsync(context, details );
        }
        internal static Task ProblemDetailResponseAsync(this HttpContext context, KloiaProblemDetails details)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;

            var result = Response<KloiaProblemDetails>.Fail(details, details.Status);

            var responseText = JsonConvert.SerializeObject(result, new JsonSerializerSettings
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