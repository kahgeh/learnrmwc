using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using AppOps;
using Serilog;
using Serilog.Exceptions;
using System;
using Amazon.Lambda.ApplicationLoadBalancerEvents;
using System.Threading.Tasks;
using Amazon.Lambda.Serialization.Json;
using Amazon.Lambda.RuntimeSupport;

namespace SampleWebApp.Bff
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME")))
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            else
            {
                var lambdaEntry = new LambdaEntryPoint();
                var functionHandler = (Func<ApplicationLoadBalancerRequest, Amazon.Lambda.Core.ILambdaContext, Task<ApplicationLoadBalancerResponse>>)lambdaEntry.FunctionHandlerAsync;
                using (var handlerWrapper = HandlerWrapper.GetHandlerWrapper(functionHandler, new JsonSerializer()))
                using (var bootstrap = new LambdaBootstrap(handlerWrapper))
                {
                    bootstrap.RunAsync().Wait();
                }
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return ConfigureWebHostBuilder(WebHost.CreateDefaultBuilder(args));
        }

        public static IWebHostBuilder ConfigureWebHostBuilder(IWebHostBuilder builder) =>
                    builder
                        .UseStartup<Startup>()
                        .AssignAppInfo((appInfo) =>
                        {
                            appInfo.GitCommitId = ThisAssembly.GitCommitId;
                            appInfo.Name = ThisAssembly.AssemblyName;
                        })
                        .UseSerilog((hostingContext, loggerConfiguration) =>
                        {
                            loggerConfiguration
                                .Enrich.FromLogContext()
                                .Enrich.WithProperty("Commit", ThisAssembly.GitCommitId)
                                .Enrich.WithExceptionDetails()
                                .ReadFrom.Configuration(hostingContext.Configuration);
                        });
    }
}
