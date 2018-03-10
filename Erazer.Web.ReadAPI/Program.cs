using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Erazer.Web.ReadAPI.Extensions;

namespace Erazer.Web.ReadAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            host.Services.Seed();
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .UseUrls("http://localhost:5000")
                .Build();
    }
}
