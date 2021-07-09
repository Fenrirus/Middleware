using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MvcSandbox.Middleware
{
    public class FeatureSwitchMiddleware
    {
        private readonly RequestDelegate _next;

        public FeatureSwitchMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IConfiguration configuration)
        {
            if (httpContext.Request.Path.Value.Contains("/Features"))
            {
                var switches = configuration.GetSection("FeaturesWitches");

                var report = switches.GetChildren().Select(x => $"{x.Key} : {x.Value}");

                await httpContext.Response.WriteAsync(string.Join("\n", report));
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}
