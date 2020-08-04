using System;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Ticket.Events;
using Erazer.Infrastructure.ReadStore;
using Erazer.Infrastructure.ServiceBus;
using Erazer.Infrastructure.Websockets;
using Erazer.Read.Mapping;
using Erazer.Syncing.Handlers;
using Erazer.Syncing.Handlers.TicketSyncHandlers;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erazer.Syncing.ConsoleRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}