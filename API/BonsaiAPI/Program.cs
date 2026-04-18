using Microsoft.EntityFrameworkCore;
using BonsaiAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BonsaiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BonsaiContext")
        ?? throw new InvalidOperationException("Connection string 'BonsaiContext' not found.")));

var app = builder.Build();

app.MapGet("/", () => "BonsaiAPI is running");

app.Run();