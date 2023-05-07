using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopBackend.Contexts;
using ShopBackend.Repositories;
using ShopBackend.Security;
using System.Text;
using System.Text.Json.Serialization;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Support for Tracing
        builder.Services.AddOpenTelemetry()
            .WithTracing(builder => builder
            .SetResourceBuilder(ResourceBuilder
            .CreateDefault().AddService("ShopServiceTracing"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri("http://otel-collector:4317"); // Trace calls gRPC (can be used for Jaeger)
            })
          );

        // Support for Metrics
        builder.Services.AddOpenTelemetry()
            .WithMetrics(builder => builder
            .SetResourceBuilder(ResourceBuilder
            .CreateDefault().AddService("ShopServiceMetrics"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri("http://otel-collector:4317"); // OTLP metrics
            })
        );

        // Support for logging
        builder.Logging.ClearProviders();
        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.SetResourceBuilder(ResourceBuilder
            .CreateDefault().AddService("ShopServiceLogging"));
            //options.AddConsoleExporter();
            options.AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri("http://otel-collector:4317"); // OTLP logging
            });
        }
        );

        // Add services to the container.
        builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        // Built with inspiration from https://www.youtube.com/watch?v=iIsaEzNXhoo&ab_channel=TrevoirWilliams
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1", 
                new OpenApiInfo 
                { 
                    Title = "ShopBackend", 
                    Version = "v1" 
                });
            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter 'Bearer' [whitespace] and then your JWT token in the input field below. For example: 'Bearer 123token'",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });
        });

        // Environment variables configurationBuilder
        var configBuilder =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables();
        IConfigurationRoot configuration = configBuilder.Build();

        // CORS configurations
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("FrontendPolicy",
                policy =>
                {
                    policy.WithOrigins("http://localhost:5173").AllowAnyHeader();
                    policy.WithOrigins("https://web-shop-app.netlify.app/").AllowAnyHeader();
                    policy.WithOrigins("https://dtu.herogamers.dev/").AllowAnyHeader();
                });
        });

        // Database connection string and DBContext service
        var connectionString = configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        builder.Services.AddDbContext<DBContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        // JWT authentication settings
        var jwtSettings = configuration.GetSection("Jwt");
        var key = jwtSettings.GetSection("Key");
        builder.Services.AddAuthentication(options => 
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => 
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false, //for dev
                ValidateAudience = false, //for dev
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key.Value!)),
            };
        });

        // Scoped repository services for dependency injection
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IAddressRepository, AddressRepository>();
        builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        builder.Services.AddScoped<IPasswordAuth, PasswordAuth>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();

        app.UseCors();

        app.MapControllers();

        app.Run();
    }
}