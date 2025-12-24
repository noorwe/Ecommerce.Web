using Ecommerce.Domain.Exceptions;
using Ecommerce.Shared.ErrorModels;
using System.Text.Json;

namespace Ecommerce.Web.CustomMiddlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate Next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = Next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpcontext)
        {
            try
            {
                await _next.Invoke(httpcontext);

                await HandleNotFoundEndpointAsync(httpcontext);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(httpcontext, ex);

            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpcontext, Exception ex)
        {
            var response = new ErrorToReturn
            {
                ErrorMessage = ex.Message
            };

            response.StatusCode = ex switch
            {

                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                BadRequestException badReqEx => GetBadRequestErrors(response, badReqEx),
                _ => StatusCodes.Status500InternalServerError

            }
        ;
            httpcontext.Response.ContentType = "application/json";

            //var responseToReturn = JsonSerializer.Serialize(response);

            await httpcontext.Response.WriteAsJsonAsync(response);
        }

        private static int GetBadRequestErrors(ErrorToReturn response, BadRequestException exception)
        {
            response.Errors = exception.Errors;
            return StatusCodes.Status400BadRequest;
        }

        private static async Task HandleNotFoundEndpointAsync(HttpContext httpcontext)
        {
            if (httpcontext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var response = new ErrorToReturn
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"End Point {httpcontext.Request.Path} Is Not Found"
                };
                await httpcontext.Response.WriteAsJsonAsync(response);

            }
        }
    }
}
