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

using Microsoft.AspNetCore.RequestDecompression;

using System.Text.Json;
using System.Text.Json.Serialization;

using Rfe.DiffSvc.WebApi.Interfaces.Repos;
using Rfe.DiffSvc.WebApi.Interfaces.Services;
using Rfe.DiffSvc.WebApi.Repos;
using Rfe.DiffSvc.WebApi.Services;

using Rfe.DiffSvc.WebApi.Middleware;



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

            // Enable controllers.
            services.AddControllers();

            // Enable Swagger.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rfe.DiffSvc.WebApi", Version = "v1" });
            });

            // Force the usage of symbolic names from enums instead of their underlying integers.
            // To be used when converting an enum value to JSON.
            // The following does NOT work:
            //services.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            //});
            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            // Add repos and services for DI.
            services.AddSingleton<IDiffRepo, DiffRepo>();
            services.AddScoped<IDiffService, DiffService>();
            services.AddScoped<ICompareService, CompareService>();

            // Add "decompression" that is about to be Base64 decoding.
            services.AddRequestDecompression(options =>
            {
                options.DecompressionProviders.Add("base64", new Base64DecompressionProvider());
            });

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

            // Use the "request decompression" as a temporary workaround for "request decoding".
            app.UseRequestDecompression();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }



    }



}
