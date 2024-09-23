using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace RedSocial.WebApi.Extensions
{
    public static class ServiceExtension
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                List<string> xmlfiles = Directory.GetFiles(AppContext.BaseDirectory, "*xml", searchOption: SearchOption.TopDirectoryOnly).ToList();
                xmlfiles.ForEach(xmlfile => option.IncludeXmlComments(xmlfile));


                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "RedSocial api",
                    Description = "Funciona si o si",
                    Contact = new OpenApiContact
                    {
                        Name = "Malvin Jimenez",
                        Email = "Sylcredjimenez19@gmail.com",
                        Url = new Uri("https://www.itla.edu.do")
                    }

                });

                option.DescribeAllParametersInCamelCase();
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your bearer token in this format: - Bearer {your token here}"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header

                        }, new List<string>()
                    }
                }); ;
            });
        }

        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }
    }
}
