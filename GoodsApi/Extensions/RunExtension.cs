using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;
using GoodsApi.AuthorizationRequirements.Handlers;
using GoodsApi.Infrastructure.AutoMapperProfiles;
using GoodsApi.Infrastructure.Models;
using GoodsApi.Infrastructure.Models.Database;
using GoodsApi.Infrastructure.Models.Enums;
using GoodsApi.Infrastructure.Models.Strategies;
using GoodsApi.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using WebApi.AuthorizationRequirements.Requirements;

namespace GoodsApi.Extensions;

public static class RunExtension
{
    extension(IServiceCollection services)
    {
        public void AddAutoMapperConfiguration()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<ModelsMappingProfile>();
                },
                new LoggerFactory());

            var mapper = mapperConfiguration.CreateMapper();
            AutoMapperService.Initialize(mapper);
        }
        
        public void ConnectionCreate()
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")!;

            services.AddEndpointsApiExplorer();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowedOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => $"{type.Namespace}.{type.Name}");
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });

            services.AddTransient<AppDbContext>(_ => new AppDbContext(connectionString));
            services.AddScoped(_ => new DataComponent(connectionString));
        }

        public void AddJwtAuthentication()
        {
            var secretKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? "super_secret_key_12345";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["AuthToken"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }
    }

    extension(WebApplicationBuilder builder)
    {
        public void AddAuthorization()
        {
            builder.Services.AddScoped<IAuthorizationHandler, NotBlockedHandler>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("NotBlocked", policy =>
                    policy.RequireAuthenticatedUser()
                        .AddRequirements(new NotBlockedRequirement()));
            });
        }

        public void RegisterServices()
        {
            builder.Services.AddScoped<AdminService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<CatalogService>();
            builder.Services.AddScoped<ManagerService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<StorekeeperService>();
            builder.Services.AddScoped<ReportService>();

            builder.Services.AddTransient<IDocumentGenerationStrategy, CheckGenerationStrategy>(sp =>
            {
                var contentRootPath = sp.GetRequiredService<IWebHostEnvironment>().ContentRootPath;
                return new CheckGenerationStrategy(contentRootPath);
            });
            
            builder.Services.AddSingleton<DocumentGenerationService>(sp =>
            {
                var env = sp.GetRequiredService<IWebHostEnvironment>();
                var strategies = sp.GetRequiredService<IEnumerable<IDocumentGenerationStrategy>>();
                return new DocumentGenerationService(strategies, env.ContentRootPath);
            });
            
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
        }
    }

    extension(WebApplication app)
    {
        public void MappingEndpoints()
        {
            app.MigrateDatabase();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowedOrigins");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi");
                options.RoutePrefix = "swagger";
            });
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");
            
            app.MapGet("/", async context =>
            {
                if (!context.Request.Cookies.TryGetValue("AuthToken", out var authToken))
                {
                    context.Response.Redirect("/Auth/Login");
                    return;
                }
            
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(authToken);
            
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            
                context.Response.Redirect($"/{roleClaim}/Index");
                
                await Task.CompletedTask;
            });
        }

        private void MigrateDatabase()
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                
                if (context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}