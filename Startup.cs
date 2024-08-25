using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NLog; 
using CRMParkBot.Core;
using Microsoft.EntityFrameworkCore;
using System;

namespace CRMParkBot
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger(); 

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services
                    .AddMvc(option => option.EnableEndpointRouting = false)
                    .AddNewtonsoftJson()
                    .AddTelegramBotAsync(_configuration).Wait();
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v2", new OpenApiInfo { Title = "CRMParkTest v1.0", Version = "v1" });
                });

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error configuring services.");
                throw;
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "CRMPark");
                    c.RoutePrefix = "";
                });

                app.UseHttpsRedirection();

                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error configuring application.");
                throw;
            }
        }
    }
}
