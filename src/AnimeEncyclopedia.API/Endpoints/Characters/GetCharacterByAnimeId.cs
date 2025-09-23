using AnimeEncyclopedia.API.DTOs;
using AnimeEncyclopedia.Infrastructure;
using Microsoft.EntityFrameworkCore;
namespace AnimeEncyclopedia.API.Endpoints.Characters;

public static class GetCharacterByAnimeId
{
    public static void MapGetCharcterByAnimeIdEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/anime/{animeId:int}/characters", async (int animeId, AppDbContext db) =>
            await db.Characters
                .Where(c => c.AnimeId == animeId)
                .Select(c => new CharacterDto(
                    c.Id,
                    c.Name,
                    c.Role,
                    new AnimeDto(c.Anime.Id, c.Anime.Title, c.Anime.Description, c.Anime.ReleaseDate, c.Anime.Genre.Name)
                ))
                .ToListAsync()
        );
    }
}