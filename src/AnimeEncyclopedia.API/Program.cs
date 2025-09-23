using AnimeEncyclopedia.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AnimeEncyclopedia.Domain;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

//remove all to other files

app.MapGet("/", () => "Anime Encyclopedia API is running!");

app.MapPost("/anime", async (Anime anime, AppDbContext db) =>
{
    db.Animes.Add(anime);
    await db.SaveChangesAsync();
    return Results.Created($"/anime/{anime.Id}", anime);
});

app.Run();
