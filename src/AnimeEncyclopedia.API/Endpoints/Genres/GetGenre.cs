using AnimeEncyclopedia.API.DTOs;
using AnimeEncyclopedia.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AnimeEncyclopedia.API.Endpoints.Genres;

public static class GetGenre
{
    public static void MapGetGenreEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/genres", async (AppDbContext db) =>
            await db.Genres
                .Select(g => new GenreDto(
                    g.Id,
                    g.Name
                ))
                .ToListAsync()
        );
    }
}