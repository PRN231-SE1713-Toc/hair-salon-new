using System.Text.Json.Serialization;
using System.Text.Json;
using System.Reflection;
using Asp.Versioning;
using HairSalon.Infrastructure;
using Microsoft.EntityFrameworkCore;
using HairSalon.Core.Contracts.Services;
using HairSalon.Service;
using HairSalon.Core;

namespace HairSalon.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Register(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.WriteIndented = true;
                opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "HairSalon.API",
                    Description = "An ASP.NET Core Web API for HairSalon project - PRN231",
                    Version = "v1",
                });
                opt.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and your JWT token in the text input below. Example: 'Bearer abcdef12345'."
                });
                opt.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference()
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });

                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("HairSalon", builder =>
                {
                    builder.WithOrigins("")   // TODO: Add client web URL here
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

            services.AddApplicationServices();
            services.AddApiVersioning();
            services.AddDatabase(connectionString);
            services.AddAutoMapper(typeof(MappingProfileExtension));

            return services;
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeScheduleService, EmployeeScheduleService>();

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(), new HeaderApiVersionReader("X-Api-Version"));
            })
            .AddApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'V";
                opt.SubstituteApiVersionInUrl = true;
            });
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<HairSalonDbContext>(opt => opt.UseSqlServer(connectionString,
                sqlOptions => sqlOptions.CommandTimeout(120)));

            return services;
        }
    }
}
