using AnimeEncyclopedia.Api.DTOs;
using AnimeEncyclopedia.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AnimeEncyclopedia.API.Endpoints.Anime;

public static class GetAnime
{
    public static void MapGetAnimeEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/anime", async (AppDbContext db) =>
            await db.Animes
                .Include(a => a.Genre)
                .Select(a => new AnimeDto(
                    a.Id,
                    a.Title,
                    a.Description,
                    a.ReleaseDate,
                    a.Genre.Name
                ))
                .ToListAsync()
        );
    }
}