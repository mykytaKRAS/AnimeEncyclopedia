using AnimeEncyclopedia.Domain;
using Microsoft.EntityFrameworkCore;

namespace AnimeEncyclopedia.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Anime> Animes => Set<Anime>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Character> Characters => Set<Character>();
}