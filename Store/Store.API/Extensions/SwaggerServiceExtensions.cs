using Microsoft.OpenApi.Models;

namespace Store.API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwagerDocumentation(this IServiceCollection services)

        {
            services.AddSwaggerGen(options =>
            {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "StoreAPI", Version = "v1" });

            var securityschema = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example :\"Authorization :Bearer{token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearer"
                }
            };
            options.AddSecurityDefinition("bearer", securityschema);
                var SecurityRequirement= (new OpenApiSecurityRequirement
                {
                    {securityschema , new[] { "bearer" } }
                });
                options.AddSecurityRequirement(SecurityRequirement);
            });   
            return services;
        }
    }
}
