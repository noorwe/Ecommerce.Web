using Ecommerce.ServiceAbstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Attributes
{
    public class CacheAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            // Create CacheKey
            string cacheKey = CreateCacheKey(context.HttpContext.Request);

            // Search For Vakue With This Key
            var _cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cachedValue = await _cacheService.GetAsync(cacheKey);

            // Return Cached Value If Found
            if (cachedValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cachedValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            // If Not Fount Invoce Next
            var ExecutedContext = await next.Invoke();
            // Set Value With CacheKey 
            if (ExecutedContext.Result is ObjectResult objectResult)
            {
                await _cacheService.SetAsync(cacheKey, objectResult.Value!, TimeSpan.FromMinutes(5));
            }


        }

        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder key = new StringBuilder();
            key.Append($"{request.Path}?");
            foreach (var item in request.Query.OrderBy(x => x.Key))
            {
                key.Append($"{item.Key}={item.Value}&");
            }
            return key.ToString();
        }
    }
}
