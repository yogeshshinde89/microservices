﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace JWTAuthentication_Service.Extensions
{
    /// <summary>
    /// MiddlewareExtensions
    /// </summary>
    public static class MiddlewareExtensions
    {
        private const string UriString = "https://fulcrumdigital.com/"; 

        /// <summary>
        /// Adds the custom swagger.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "F1 Rule Engine Service",
                    Version = "v3",
                    Description = "Example API that shows how to validate rules, using JSON rules files  with ASP.NET Core 3.1, built from scratch.",
                    Contact = new OpenApiContact
                    {
                        Name = "Fulcrum",
                        Url = new Uri(UriString)
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                    },
                });

                cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JSON Web Token to access resources. Example: Bearer {token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new [] { string.Empty }
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                cfg.IncludeXmlComments(xmlPath);
            });
            return services;
        }

        /// <summary>
        /// Uses the custom swagger.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger().UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "F1 Rule Engine Service");
                options.DocumentTitle = "F1 Rule Engine Service";
            });
            return app;
        }
    }
}
