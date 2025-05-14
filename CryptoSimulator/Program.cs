using CryptoSimulator.DataContext;
using CryptoSimulator.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

namespace CryptoSimulator;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Controllers
        builder.Services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        builder.Services.AddOpenApi();

        // Database connection
        builder.Services.AddDbContext<SQL>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SQL")));

        // Local services
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IWalletService, WalletService>();
        builder.Services.AddScoped<ICryptoService, CryptoService>();
        builder.Services.AddScoped<ITradeService, TradeService>();
        builder.Services.AddScoped<IPortfolioService, PortfolioService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddScoped<IPriceUpdateService, PriceUpdateService>();
        builder.Services.AddHostedService<PriceUpdateService>();
        builder.Services.AddScoped<IProfitService, ProfitService>();
        builder.Services.AddScoped<IInitService, InitService>();

        // JWT Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // Set to true in production
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero
            };
        });


        // Authorization
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("UserPolicy", policy => policy.RequireRole("User", "Admin"));
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
        });


        // Cors...
        builder.Services.AddCors();


        // Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoSimulator", Version = "v1" });


            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }});
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SQL>();
                context.Database.Migrate();

                var initService = scope.ServiceProvider.GetRequiredService<IInitService>();
                // Seed data
                initService.SeedDatabaseAsync().GetAwaiter().GetResult();
            }

            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoSimulator API v1");
                options.RoutePrefix = string.Empty;
            });
        }


        app.UseHttpsRedirection();
        app.UseCors(options =>
        {
            options.AllowAnyMethod();
            options.AllowAnyOrigin();
            options.AllowAnyHeader();
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
