using AnimeEncyclopedia.API.DTOs;
using AnimeEncyclopedia.Infrastructure;
using AnimeEncyclopedia.Domain;
namespace AnimeEncyclopedia.API.Endpoints.Anime;

public static class CreateAnime
{
    public static void MapCreateAnimeEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/anime", async (CreateAnimeDto dto, AppDbContext db) =>
        {
            var genre = await db.Genres.FindAsync(dto.GenreId);
            if (genre == null)
                return Results.BadRequest($"Genre with Id {dto.GenreId} does not exist.");

            var anime = new Domain.Anime
            {
                Title = dto.Title,
                Description = dto.Description,
                ReleaseDate = dto.ReleaseDate,
                GenreId = dto.GenreId
            };

            db.Animes.Add(anime);
            await db.SaveChangesAsync();

            return Results.Created($"/anime/{anime.Id}", new AnimeDto(
                anime.Id,
                anime.Title,
                anime.Description,
                anime.ReleaseDate,
                genre.Name
            ));
        });
    }
}
