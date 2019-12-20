using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Erazer.Web.ReadAPI.Extensions;

namespace Erazer.Web.ReadAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            host.Services.Seed();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5000");
    }
}
