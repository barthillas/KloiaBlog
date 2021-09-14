using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Abstraction.Exceptions;
using ApiBase.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Formatting = System.Xml.Formatting;

namespace ApiBase.Extension
{
    internal static class HttpContextExtensions
    {
        internal static Task HandleExceptionAsync(this HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;

            BaseProblemDetails details;
            switch (exception)
            {
                case BusinessException ex:
                    details = new KloiaProblemDetails(ex)
                    {
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Title = "UnprocessableEntity",
                        ErrorCode = StatusCodes.Status422UnprocessableEntity
                    };
                    break;
                case ValidationException ex:
                    details = new ValidationProblemDetails(ex)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation Exception",
                        ErrorCode = StatusCodes.Status400BadRequest
                    };
                    break;
                default:
                    details = new KloiaProblemDetails(exception)
                    {
                        StackTrace = exception.StackTrace,
                        Message = exception.Message,
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Unknown Exception",
                    };
                    break;
            }
            return context.ProblemDetailResponseAsync(details);
        }

        private static Task ProblemDetailResponseAsync<T>(this HttpContext context, T details) where T : BaseProblemDetails
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = details.Status;
            
            var result = Response<T>.Fail(details, details.Status);

            var responseText = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                Formatting = (Newtonsoft.Json.Formatting) Formatting.Indented,
            });
            return context.Response.WriteAsync(responseText);
        }
    }
}