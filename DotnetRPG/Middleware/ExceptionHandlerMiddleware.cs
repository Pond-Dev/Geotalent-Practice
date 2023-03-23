using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetRpg.Core;
using Serilog;

namespace DotnetRpg.Middleware
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // throw new NotImplementedException();
            try {
               await next(context);
            }
            catch (System.Exception ex)
            {
                Log.Error(ex,"Error :");
                ErrorResponse error = new()
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Message= ex.Message
                };

                await context.Response.WriteAsJsonAsync(error);

            }
        }
    }

    public static class UseExceptionHandlerMiddlewareExtension 
    {
        public static void UseMyExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}