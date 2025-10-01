using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using AnimeEncyclopedia.API.Services;

namespace AnimeEncyclopedia.API.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var jwtKey = config["Jwt:Key"];
        var jwtIssuer = config["Jwt:Issuer"];

        if (string.IsNullOrWhiteSpace(jwtKey))
            throw new ArgumentNullException(nameof(jwtKey), "Jwt:Key must be provided in configuration and be at least 32 chars.");
        if (jwtKey.Length < 32)
            throw new ArgumentException("Jwt:Key must be at least 32 characters long for HS256.", nameof(jwtKey));
        if (string.IsNullOrWhiteSpace(jwtIssuer))
            throw new ArgumentNullException(nameof(jwtIssuer), "Jwt:Issuer must be provided in configuration.");

        services.AddSingleton<JwtService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtIssuer,
                    ValidAudience = config["Jwt:Audience"] ?? jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };

                // Чтобы можно было ловить ошибки токена
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        Console.WriteLine($"[JWT ERROR] {ctx.Exception.Message}");
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
        });

        return services;
    }
}
