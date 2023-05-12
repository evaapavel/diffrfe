using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.Interfaces.Repos;
using Rfe.DiffSvc.WebApi.Interfaces.Services;
using Rfe.DiffSvc.WebApi.Repos;
using Rfe.DiffSvc.WebApi.Services;



namespace Rfe.DiffSvc.WebApi
{



    /// <summary>
    /// The REST API web app configuration.
    /// </summary>
    public class Startup
    {



        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        public IConfiguration Configuration { get; }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rfe.DiffSvc.WebApi", Version = "v1" });
            });


            // Add repos and services for DI.
            services.AddSingleton<IDiffRepo, DiffRepo>();
            services.AddScoped<IDiffService, DiffService>();

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rfe.DiffSvc.WebApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }



    }



}
