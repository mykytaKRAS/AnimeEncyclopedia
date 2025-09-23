using AnimeEncyclopedia.Infrastructure;
namespace AnimeEncyclopedia.API.Endpoints.Anime;
public static class DeleteAnime
{
    public static void MapDeleteAnimeEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapDelete("anime/{id:int}", async (int id, AppDbContext db) =>
        {
            var anime = await db.Animes.FindAsync(id);
            if (anime == null)
                return Results.NotFound();

            db.Animes.Remove(anime);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}