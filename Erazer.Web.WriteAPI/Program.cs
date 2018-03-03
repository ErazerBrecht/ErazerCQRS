using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

namespace Erazer.Web.WriteAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .UseUrls("http://localhost:5001")
                .Build();
    }
}
