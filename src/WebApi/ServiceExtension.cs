namespace WebApi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;

    public static class ServiceExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "AuthExample API", Version = "v1",
                    Description = "A simple authentication example ASP.NET Core Web API",
                    Contact = new Contact
                    {
                        Name = "Witzke Leandro",
                        Email = "leanwitzke@gmail.com",
                        Url = "https://github.com/Leanwit"
                    }
                });
                
                c.AddSecurityDefinition("basic", new BasicAuthScheme {Type = "basic", Description = "basic authentication" }); 
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { "basic", new string[] { } },});
                
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}