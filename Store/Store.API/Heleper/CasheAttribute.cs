using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Service.Services.CacheService;
using System.Text;

namespace Store.API.Heleper
{
    public class CasheAttribut : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;
        

        public CasheAttribut(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _cacheService = context.HttpContext.RequestServices.GetRequiredService<ICachService>();

            var cachekey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cacheResponse = await _cacheService.GetCachResponseAsync(cachekey);

            if(!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var executedContext = await next();
            if (executedContext.Result is OkObjectResult response)
                await _cacheService.SetCachResponseAsync(cachekey, response.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            StringBuilder cachekey = new StringBuilder();
            cachekey.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
                cachekey.Append($"|{key}-{value}");

            return cachekey.ToString();
        }
    }
}
