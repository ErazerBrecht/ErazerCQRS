﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Erazer.Web.DocumentStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .UseUrls("http://localhost:5002");
    }
}
