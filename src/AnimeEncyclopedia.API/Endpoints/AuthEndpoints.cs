using AnimeEncyclopedia.API.DTOs;
using AnimeEncyclopedia.API.Services;
using AnimeEncyclopedia.Domain;
using AnimeEncyclopedia.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AnimeEncyclopedia.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/register/user", async (RegisterDto dto, AppDbContext db, JwtService jwt) =>
        {
            // простые валидации
            if (await db.Users.AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email))
                return Results.BadRequest("Username or email already taken.");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            var accessLifetime = TimeSpan.FromHours(2);
            var access = jwt.GenerateAccessToken(user, accessLifetime);
            var (refreshToken, refreshExpires) = jwt.GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken { Token = refreshToken, Expires = refreshExpires, IsRevoked = false });
            await db.SaveChangesAsync();

            return Results.Ok(new AuthResponseDto(access, refreshToken, DateTime.UtcNow.Add(accessLifetime)));
        });

        app.MapPost("/auth/register/admin", async (RegisterDto dto, AppDbContext db, JwtService jwt) =>
        {
            // простые валидации
            if (await db.Users.AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email))
                return Results.BadRequest("Username or email already taken.");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Admin"
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            var accessLifetime = TimeSpan.FromHours(2);
            var access = jwt.GenerateAccessToken(user, accessLifetime);
            var (refreshToken, refreshExpires) = jwt.GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken { Token = refreshToken, Expires = refreshExpires, IsRevoked = false });
            await db.SaveChangesAsync();

            return Results.Ok(new AuthResponseDto(access, refreshToken, DateTime.UtcNow.Add(accessLifetime)));
        });

        app.MapPost("/auth/login", async (LoginDto dto, AppDbContext db, JwtService jwt) =>
        {
            var user = await db.Users.Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail);

        if (user == null) return Results.Unauthorized();

        var verified = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!verified) return Results.Unauthorized();

        // --- Удаляем или аннулируем старые токены ---
        var oldTokens = user.RefreshTokens.Where(t => !t.IsRevoked || t.Expires <= DateTime.UtcNow).ToList();
        foreach (var token in oldTokens)
        {
            token.IsRevoked = true;       // помечаем как аннулированные
     }
        db.RefreshTokens.RemoveRange(oldTokens.Where(t => t.Expires <= DateTime.UtcNow)); // удаляем просроченные

        // --- Создаём новый токен ---
        var accessLifetime = TimeSpan.FromHours(2);
        var access = jwt.GenerateAccessToken(user, accessLifetime);
        var (refreshToken, refreshExpires) = jwt.GenerateRefreshToken();

        user.RefreshTokens.Add(new RefreshToken
        {
           Token = refreshToken,
          Expires = refreshExpires,
          IsRevoked = false
        });

        await db.SaveChangesAsync();

        return Results.Ok(new AuthResponseDto(access, refreshToken, DateTime.UtcNow.Add(accessLifetime)));
    });


        app.MapPost("/auth/refresh", async (RefreshRequestDto req, AppDbContext db, JwtService jwt) =>
        {
            var stored = await db.RefreshTokens.Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == req.RefreshToken);

            if (stored == null || stored.IsRevoked || stored.Expires <= DateTime.UtcNow) 
                return Results.Unauthorized();

            var user = stored.User!;
            // revoke current refresh if you want or keep it; for safety, revoke it and issue new one
            stored.IsRevoked = true;

            var accessLifetime = TimeSpan.FromHours(2);
            var access = jwt.GenerateAccessToken(user, accessLifetime);
            var (newRefresh, newRefreshExpires) = jwt.GenerateRefreshToken();

            var newRefreshEntity = new RefreshToken { Token = newRefresh, Expires = newRefreshExpires, IsRevoked = false, UserId = user.Id };
            db.RefreshTokens.Add(newRefreshEntity);

            await db.SaveChangesAsync();

            return Results.Ok(new AuthResponseDto(access, newRefresh, DateTime.UtcNow.Add(accessLifetime)));
        });

        app.MapPost("/auth/revoke", async (RevokeRequestDto req, AppDbContext db) =>
        {
            var stored = await db.RefreshTokens.FirstOrDefaultAsync(r => r.Token == req.RefreshToken);
            if (stored == null) return Results.NotFound();
            stored.IsRevoked = true;
            await db.SaveChangesAsync();
            return Results.Ok();
        });

        app.MapGet("/auth/me", [Microsoft.AspNetCore.Authorization.Authorize] async (ClaimsPrincipal user, AppDbContext db) =>
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(idClaim, out var id)) return Results.Unauthorized();
            var u = await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (u == null) return Results.NotFound();
            return Results.Ok(new { u.Id, u.Username, u.Email, u.Role, u.CreatedAt });
        });
    }
}
