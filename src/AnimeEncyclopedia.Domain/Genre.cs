namespace AnimeEncyclopedia.Domain;
public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public List<Anime> Animes { get; set; } = new();
}