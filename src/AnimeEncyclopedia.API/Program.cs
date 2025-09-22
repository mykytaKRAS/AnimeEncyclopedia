using AnimeEncyclopedia.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AnimeEncyclopedia.Domain;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/", () => "Anime Encyclopedia API is running!");


app.MapGet("/anime", async (AppDbContext db) =>
    await db.Animes
            .Include(a => a.Genre)
            .Select(a => new AnimeDto(a.Id, a.Title, a.Description, a.ReleaseDate, a.Genre.Name))
            .ToListAsync());

app.MapPost("/anime", async (Anime anime, AppDbContext db) =>
{
    db.Animes.Add(anime);
    await db.SaveChangesAsync();
    return Results.Created($"/anime/{anime.Id}", anime);
});

app.Run();
public record AnimeDto(int Id, string Title, string Description, DateTime ReleaseDate, string GenreName);
