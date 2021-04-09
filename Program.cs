using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Middleware.Multiplexer;
using Ocelott.Aggregators;
using Ocelot.Provider.Consul;
using Microsoft.AspNetCore;
using System.IO;

namespace Ocelott
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
               .UseKestrel()
               
               .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config
                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, true)
                       .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                       //.AddOcelot(hostingContext.HostingEnvironment)
                       .AddOcelot($"{hostingContext.HostingEnvironment.ContentRootPath}"+"\\OcelotConfigs", hostingContext.HostingEnvironment)
                       .AddEnvironmentVariables();
               })
               .ConfigureServices(services => {
                   services.AddOcelot().
                            AddConsul().
                          //AddConfigStoredInConsul()
                           AddSingletonDefinedAggregator<PingAggregator>();
               })
               .ConfigureLogging((hostingContext, logging) =>
               {
                   //add your logging
               })
               .UseIISIntegration()
               .Configure(app =>
               {
                   app.UseOcelot().Wait();
               }).UseUrls("http://localhost:5000")
               .Build()
               
               .Run();
        }
    }
}

//    public static void Main(string[] args)
//    {
//        CreateHostBuilder(args).Build().Run();

//    }

//    public static IWebHostBuilder CreateHostBuilder(string[] args) =>
//        WebHost.CreateDefaultBuilder(args).ConfigureServices(services => {
//            services.AddOcelot().
//            AddConsul().
//            AddConfigStoredInConsul().
//            AddSingletonDefinedAggregator<PingAggregator>();


//        }).ConfigureAppConfiguration((host, config) => {

//            config
//                  .SetBasePath(host.HostingEnvironment.ContentRootPath)
//                  .AddJsonFile("appsettings.json", true, true)
//                  .AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", true, true)
//                  .AddJsonFile($"ocelot.{host.HostingEnvironment.EnvironmentName.ToLower()}.json")
//                  .AddOcelot("/OcelotConfigs",host.HostingEnvironment)
//                  .AddEnvironmentVariables();
//        })
//            .ConfigureWebHostDefaults(webBuilder =>
//            {
//                webBuilder.UseStartup<Startup>().Configure(async app => 
//                await app.UseOcelot())
//                .UseUrls("http://localhost:5000");
//            });
//}