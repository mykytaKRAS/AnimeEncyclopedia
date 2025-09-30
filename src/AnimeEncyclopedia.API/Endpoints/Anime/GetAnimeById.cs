using AnimeEncyclopedia.API.DTOs;
using AnimeEncyclopedia.Infrastructure;
using Microsoft.EntityFrameworkCore;
namespace AnimeEncyclopedia.API.Endpoints.Anime;

public static class GetAnimeById
{
    public static void MapGetAnimeByIdEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("anime/{id:int}", async (int id, AppDbContext db) =>
        {
            var anime = await db.Animes
                .Include(a => a.Genre)
                .Where(a => a.Id == id)
                .Select(a => new AnimeDto(
                    a.Id,
                    a.Title,
                    a.Description,
                    a.ReleaseDate,
                    a.Genre.Name
                ))
                .FirstOrDefaultAsync();

            return anime is not null ? Results.Ok(anime) : Results.NotFound();
        }).RequireAuthorization();;
    }
}