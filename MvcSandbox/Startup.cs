// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvcSandbox.Middleware;
using MvcSandbox.ModelBind;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSandbox
{
    public class Startup
    {
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                    new WebHostBuilder()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .ConfigureLogging(factory =>
                        {
                            factory
                                .AddConsole()
                                .AddDebug();
                        })
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.AddJsonFile("appsettings.json");
                        })
                        .UseIISIntegration()
                        .UseKestrel()
                        .UseStartup<Startup>();

        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args)
                .Build();

            host.Run();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            //app.Use(async (contex, next) =>
            //{
            //    contex.Items.Add("greeting", "Hello");
            //    Debug.WriteLine("Before");
            //    await next.Invoke();
            //    Debug.WriteLine("After");
            //});
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("The Page says " + context.Items["greeting"]);
            //});

            app.UseMiddleware<FeatureSwitchMiddleware>();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<FeatureSwitchAuthMiddleware>();
            app.UseEndpoints(builder =>
            {
                builder.MapDefaultControllerRoute();
                //builder.MapGet(
                //    requestDelegate: WriteEndpoints,
                //    pattern: "/endpoints").WithDisplayName("Endpoints");

                //builder.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");

                //builder.MapControllerRoute(
                //    name: "transform",
                //    pattern: "Transform/{controller:slugify=Home}/{action:slugify=Index}/{id?}",
                //    defaults: null,
                //    constraints: new { controller = "Home" });

                //builder.MapGet(
                //    "/graph",
                //    async (httpContext) =>
                //    {
                //        await using var writer = new StringWriter();
                //        var graphWriter = httpContext.RequestServices.GetRequiredService<DfaGraphWriter>();
                //        var dataSource = httpContext.RequestServices.GetRequiredService<EndpointDataSource>();
                //        graphWriter.Write(dataSource, writer);
                //        await httpContext.Response.WriteAsync(writer.ToString());
                //    }).WithDisplayName("DFA Graph");

                //builder.MapControllers();
                //builder.MapRazorPages();
                //builder.MapBlazorHub();
                //builder.MapFallbackToPage("/Components");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options =>
            {
                options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
            });
            services.AddServerSideBlazor();
            services.AddMvc(opt =>
                {
                    opt.ModelBinderProviders.Insert(0, new CSVModelBinderProvider());
                });
        }

        private static Task WriteEndpoints(HttpContext httpContext)
        {
            var dataSource = httpContext.RequestServices.GetRequiredService<EndpointDataSource>();

            var sb = new StringBuilder();
            sb.AppendLine("Endpoints:");
            foreach (var endpoint in dataSource.Endpoints.OfType<RouteEndpoint>().OrderBy(e => e.RoutePattern.RawText, StringComparer.OrdinalIgnoreCase))
            {
                sb.AppendLine($"- {endpoint.RoutePattern.RawText} '{endpoint.DisplayName}'");
            }

            var response = httpContext.Response;
            response.StatusCode = 200;
            response.ContentType = "text/plain";
            return response.WriteAsync(sb.ToString());
        }
    }
}