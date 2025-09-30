using AnimeEncyclopedia.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AnimeEncyclopedia.API.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Anime Encyclopedia API is running!");

app.MapApiEndpoints();

app.Run();
