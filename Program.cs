using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using NLog; 
namespace CRMParkBot
{
    public class Program
    {
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger(); 

        public static void Main(string[] args)
        {
            try
            {
                

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An unhandled exception occurred during application startup.");
                throw;
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
