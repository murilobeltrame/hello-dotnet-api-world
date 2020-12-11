using System.Reflection;
using System.Threading.Tasks;
using ApiShowcase.Rest.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ApiShowcase.Rest.Utils.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseJsonExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseExceptionHandler(application => application.Run(async context =>
            {
                //var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerFeature?.Error;

                var result = JsonConvert.SerializeObject(new ErrorResponse{ Message = exception?.Message ?? string.Empty });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));
        }
    }
}
