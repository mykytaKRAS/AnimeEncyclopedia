namespace AnimeEncyclopedia.Domain;

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Role { get; set; } = default!;
    public int AnimeId { get; set; }
    public Anime Anime { get; set; } = default!;
}