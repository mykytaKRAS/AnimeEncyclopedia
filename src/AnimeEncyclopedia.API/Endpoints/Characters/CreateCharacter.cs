using AnimeEncyclopedia.API.DTOs;
using AnimeEncyclopedia.Infrastructure;
namespace AnimeEncyclopedia.API.Endpoints.Characters;

public static class CreateCharacer
{
    public static void MapCreateCharacterEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/anime/{animeId:int}/characters", async (int animeId, CreateCharacterDto dto, AppDbContext db) =>
        {
            var anime = await db.Animes.FindAsync(animeId);
            if (anime == null)
                return Results.NotFound($"Anime with ID {animeId} not found.");

            var newCharacter = new Domain.Character
            {
                Name = dto.Name,
                Role = dto.Role,
                AnimeId = animeId
            };

            db.Characters.Add(newCharacter);
            await db.SaveChangesAsync();
            
            var Genre = await db.Genres.FindAsync(anime.GenreId);
            if (Genre == null)
                return Results.NotFound($"Genre with ID {anime.GenreId} not found.");

            return Results.Created(
                $"/anime/{animeId}/characters/{newCharacter.Id}",
                new CharacterDto(newCharacter.Id, newCharacter.Name, newCharacter.Role,
                    new AnimeDto(anime.Id, anime.Title, anime.Description, anime.ReleaseDate, Genre.Name))
            );
            
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));;
    }
}