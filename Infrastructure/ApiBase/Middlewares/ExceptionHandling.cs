using System;
using System.IO;
using System.Threading.Tasks;
using ApiBase.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ApiBase.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
  

        public ExceptionHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next;
            this._logger = loggerFactory.CreateLogger<ExceptionHandlingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Body.CanSeek)
            {
                context.Request.Body.Position = 0;
                using var reader = new StreamReader(context.Request.Body);
                await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            try
            {
                await this._next(context).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                    await context.HandleExceptionAsync(exception ).ConfigureAwait(false);
                    this._logger.LogError(exception, context.Request.Path); 
            }
        }
    }
}