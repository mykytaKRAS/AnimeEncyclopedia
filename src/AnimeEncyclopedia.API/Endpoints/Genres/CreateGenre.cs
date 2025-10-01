using AnimeEncyclopedia.Domain;
using AnimeEncyclopedia.Infrastructure;
using AnimeEncyclopedia.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AnimeEncyclopedia.API.Endpoints.Genres;
public static class CreateGenre
{
    public static void MapCreateGenreEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/genres", async (CreateGenreDto dto, AppDbContext db) =>
        {
            var genre = new Genre
            {
                Name = dto.Name
            };

            db.Genres.Add(genre);
            await db.SaveChangesAsync();

            return Results.Created($"/genres/{genre.Id}", new GenreDto(
                genre.Id,
                genre.Name
            ));
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));;
    }
}
