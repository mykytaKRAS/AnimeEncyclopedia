using AnimeEncyclopedia.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AnimeEncyclopedia.API.Extensions;
using AnimeEncyclopedia.API.Middlewares;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();
app.UseErrorHandling();

app.MapGet("/", () => "Anime Encyclopedia API is running!");

app.MapGet("/protected", () => "You are authorized!")
   .RequireAuthorization();



app.UseAuthentication();
app.UseAuthorization();
app.MapApiEndpoints();



app.Run();

