namespace AnimeEncyclopedia.Api.DTOs;
public record CharacterDto(int Id, string Name, string Role, AnimeDto Anime);
public record CreateCharacterDto(string Name, string Role, int AnimeId);
