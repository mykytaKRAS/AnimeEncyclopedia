namespace AnimeEncyclopedia.Domain;

public class Anime
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime ReleaseDate { get; set; }
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = default!;
    public List<Character> Characters { get; set; } = new();
}