using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Rfe.Diff.Svc
{



    /// <summary>
    /// Exposes the main application entry point.
    /// </summary>
    public class Program
    {



        /// <summary>
        /// Main application entry point.
        /// </summary>
        /// <param name="args">Command line parameters.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }



        /// <summary>
        /// Helps in building the app host.
        /// </summary>
        /// <param name="args">Command line parameters.</param>
        /// <returns>Returns an object to configure and start the web app.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });



    }



}
