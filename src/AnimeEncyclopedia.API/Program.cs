using AnimeEncyclopedia.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AnimeEncyclopedia.Domain;
using AnimeEncyclopedia.API.Endpoints.Anime;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/", () => "Anime Encyclopedia API is running!");

app.MapGetAnimeEndpoint();
app.MapCreateAnimeEndpoint();
app.MapGetAnimeByIdEndpoint();
app.Run();
