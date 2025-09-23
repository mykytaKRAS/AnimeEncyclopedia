namespace AnimeEncyclopedia.API.DTOs;

public record GenreDto(int Id, string Name);
public record CreateGenreDto(string Name);