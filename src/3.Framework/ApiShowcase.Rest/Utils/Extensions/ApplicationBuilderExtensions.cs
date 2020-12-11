using System.Reflection;
using System.Threading.Tasks;
using ApiShowcase.Rest.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace ApiShowcase.Rest.Utils.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseJsonExceptionHandler(this IApplicationBuilder app)
        {
            app.Run(async context => await Task.Run(() =>
            {
                var _errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var _exception = _errorFeature.Error;

                var _problemDetails = new ErrorResponse();

                if (_exception is BadHttpRequestException badHttpRequestException)
                {
                    _problemDetails.Message = "Invalid request";
                    context.Response.StatusCode = (int)typeof(BadHttpRequestException).GetProperty("StatusCode", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(badHttpRequestException);
                }
                else
                {
                    _problemDetails.Message = "An unexpected error occurred!";
                    context.Response.StatusCode = 500;
                }

                
                context.Response.WriteJson(_problemDetails);
            }));

            return app;
        }
    }
}
