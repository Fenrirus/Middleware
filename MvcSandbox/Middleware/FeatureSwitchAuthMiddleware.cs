using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MvcSandbox.Middleware
{
    public class FeatureSwitchAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public FeatureSwitchAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IConfiguration configuration)
        {
            var endpoint = httpContext.GetEndpoint()?.Metadata.GetMetadata<RouteAttribute>();

            if (endpoint != null)
            {
                var FeatureSwitch = configuration.GetSection("FeaturesWitches").GetChildren().FirstOrDefault(x => x.Key == endpoint.Name); ;

                if (FeatureSwitch != null && !bool.Parse(FeatureSwitch.Value))
                {
                    httpContext.SetEndpoint(new Endpoint((context) =>
                        {
                            context.Response.StatusCode = StatusCodes.Status404NotFound;
                            return Task.CompletedTask;
                        },
                        EndpointMetadataCollection.Empty, "Featured Not Found"));
                }
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}
