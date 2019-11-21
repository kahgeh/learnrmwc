using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace AppOps
{
    public class AppInfo
    {
        public string GitCommitId { get; set; }
        public string Name { get; set; }

        public DateTime StartedUtc { get; }

        public AppInfo()
        {
            StartedUtc = DateTime.UtcNow;
        }
    }

    public static class AppOpsExtensions
    {
        public static IWebHostBuilder AssignAppInfo(this IWebHostBuilder builder, Action<AppInfo> assignAppInfo)
        {
            builder.ConfigureServices(services =>
            {
                var appInfo = new AppInfo();
                assignAppInfo(appInfo);
                services.AddSingleton(appInfo);
            });

            return builder;
        }

        public static IApplicationBuilder UseAppInfo(this IApplicationBuilder app, string path = "/appinfo")
        {
            app.Map(path, (appBuilder) => appBuilder.Run(
                 (httpContext) =>
                {
                    var appInfo = appBuilder.ApplicationServices.GetService<AppInfo>();
                    var appInfoJson = JsonSerializer.Serialize(appInfo);
                    httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    httpContext.Response.ContentLength = appInfoJson.Length;
                    httpContext.Response.ContentType = "application/json";
                    return httpContext.Response.WriteAsync(appInfoJson);
                }
            ));
            return app;
        }
    }
}
