using Microsoft.AspNetCore.Hosting;

namespace SampleWebApp.Bff
{
    public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.ApplicationLoadBalancerFunction<Startup>
    {
        protected override void Init(IWebHostBuilder builder)
        {
            Program.ConfigureWebHostBuilder(builder);
        }
    }
}