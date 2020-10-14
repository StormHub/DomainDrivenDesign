using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Domain.WebApi.Bootstrappers
{
    public static class SwaggerConfig
    {
        public static void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment)
        {
            if (environment.IsProduction()) return;

            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "HTTP API",
                            Version = "v1",
                            Description = "Domain Driver Design HTTP API"
                        });

                    // Api documentation
                    string xmlFile = $"{typeof(IAssemblyMarker).Assembly.GetName().Name}.xml";
                    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);

                    options.CustomSchemaIds(SchemaNamingConvention.SelectSchemaId);
                });
        }

        public static void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsProduction()) return;

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "HTTP API V1"); });
        }
    }
}