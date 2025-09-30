using AnimeEncyclopedia.API.Endpoints.Anime;
using AnimeEncyclopedia.API.Endpoints.Genres;
using AnimeEncyclopedia.API.Endpoints.Characters;
using AnimeEncyclopedia.API.Endpoints;

namespace AnimeEncyclopedia.API.Extensions;

public static class EndpointExtensions
{
    public static void MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        // Auth
        app.MapAuthEndpoints();

        // Anime
        app.MapGetAnimeEndpoint();
        app.MapCreateAnimeEndpoint();
        app.MapGetAnimeByIdEndpoint();
        app.MapUpdateAnimeEndpoint();
        app.MapDeleteAnimeEndpoint();

        // Genres
        app.MapGetGenreEndpoint();
        app.MapCreateGenreEndpoint();

        // Characters
        app.MapGetCharcterByAnimeIdEndpoint();
        app.MapCreateCharacterEndpoint();
    }
}
