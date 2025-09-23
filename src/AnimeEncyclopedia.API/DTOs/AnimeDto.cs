namespace AnimeEncyclopedia.Api.DTOs;

public record AnimeDto(
    int Id, 
    string Title, 
    string Description, 
    DateTime ReleaseDate, 
    string GenreName
);

public record CreateAnimeDto(
    string Title,
    string Description,
    DateTime ReleaseDate,
    int GenreId
);