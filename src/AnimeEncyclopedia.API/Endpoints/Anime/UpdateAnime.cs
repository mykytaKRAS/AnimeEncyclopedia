using AnimeEncyclopedia.API.DTOs;
using AnimeEncyclopedia.Infrastructure;

namespace AnimeEncyclopedia.API.Endpoints.Anime;
public static class UpdateAnime
{
    public static void MapUpdateAnimeEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPut("anime/{id:int}", async (CreateAnimeDto dto, int id, AppDbContext db) =>
        {
            var anime = await db.Animes.FindAsync(id);
            if (anime == null)
                return Results.NotFound();

            var genre = await db.Genres.FindAsync(dto.GenreId);
            if (genre == null)
                return Results.BadRequest($"Genre with Id {dto.GenreId} does not exist.");

            anime.Title = dto.Title;
            anime.Description = dto.Description;
            anime.ReleaseDate = dto.ReleaseDate;
            anime.GenreId = dto.GenreId;

            await db.SaveChangesAsync();

            return Results.Ok(new AnimeDto(
                anime.Id,
                anime.Title,
                anime.Description,
                anime.ReleaseDate,
                genre.Name
            ));
        });
    }
}
